using ApiRndc.Domain.Enums;
using ApiRndc.Domain.Interfaces;
using ApiRndc.Shared.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace ApiRndc.Infrastructure.Services;

/// <summary>
/// Implementación del cliente SOAP para el servicio RNDC
/// </summary>
public class RndcSoapClient : IRndcSoapClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RndcSoapClient> _logger;
    private readonly string _username;
    private readonly string _password;

    public RndcSoapClient(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<RndcSoapClient> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;

        // Obtener credenciales desde configuración
        _username = _configuration["Rndc:Username"] ?? throw new InvalidOperationException("RNDC Username no configurado");
        _password = _configuration["Rndc:Password"] ?? throw new InvalidOperationException("RNDC Password no configurado");
    }

    public async Task<RndcResponse> SendTransactionAsync(
        TransactionType transactionType,
        string xmlVariables,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Enviando transacción al RNDC. Tipo: {TransactionType}", transactionType);

            // Construir XML SOAP completo
            var soapXml = BuildSoapEnvelope(transactionType, xmlVariables);
            _logger.LogDebug("SOAP Request: {SoapXml}", soapXml);

            // Preparar solicitud HTTP
            var content = new StringContent(soapXml, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", "urn:BPMServicesIntf-IBPMServices#AtenderMensajeRNDC");

            // Enviar solicitud
            var response = await _httpClient.PostAsync(RndcConstants.ServiceUrl, content, cancellationToken);

            // Leer respuesta
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("SOAP Response: {ResponseContent}", responseContent);

            // Procesar respuesta
            return ParseResponse(responseContent);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error de comunicación con el servicio RNDC");
            return new RndcResponse
            {
                IsSuccess = false,
                ErrorCode = "HTTP_ERROR",
                ErrorDetails = $"Error de comunicación: {ex.Message}",
                Message = "No se pudo conectar con el servicio RNDC"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al enviar transacción al RNDC");
            return new RndcResponse
            {
                IsSuccess = false,
                ErrorCode = "UNEXPECTED_ERROR",
                ErrorDetails = ex.Message,
                Message = "Error inesperado al procesar la transacción"
            };
        }
    }

    public string BuildRequestXml(TransactionType transactionType, string xmlVariables)
    {
        return BuildSoapEnvelope(transactionType, xmlVariables);
    }

    private string BuildSoapEnvelope(TransactionType transactionType, string xmlVariables)
    {
        // Construir el XML interno de la solicitud
        var requestXml = new XElement("root",
            new XElement("acceso",
                new XElement("username", _username),
                new XElement("password", _password)
            ),
            new XElement("solicitud",
                new XElement("tipo", RndcConstants.RequestType),
                new XElement("procesoid", ((int)transactionType).ToString())
            ),
            XElement.Parse($"<variables>{xmlVariables}</variables>")
        );

        // Construir el sobre SOAP
        XNamespace soapEnv = "http://schemas.xmlsoap.org/soap/envelope/";
        XNamespace xsd = "http://www.w3.org/2001/XMLSchema";
        XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        XNamespace soapEnc = "http://schemas.xmlsoap.org/soap/encoding/";
        XNamespace ns1 = RndcConstants.SoapNamespace;

        var envelope = new XElement(soapEnv + "Envelope",
            new XAttribute(XNamespace.Xmlns + "SOAP-ENV", soapEnv),
            new XAttribute(XNamespace.Xmlns + "xsd", xsd),
            new XAttribute(XNamespace.Xmlns + "xsi", xsi),
            new XAttribute(XNamespace.Xmlns + "SOAP-ENC", soapEnc),
            new XAttribute(XNamespace.Xmlns + "ns1", ns1),
            new XAttribute(soapEnv + "encodingStyle", soapEnc),
            new XElement(soapEnv + "Body",
                new XElement(ns1 + "AtenderMensajeRNDC",
                    new XElement("Request",
                        new XAttribute(xsi + "type", "xsd:string"),
                        requestXml.ToString(SaveOptions.DisableFormatting)
                    )
                )
            )
        );

        return envelope.ToString(SaveOptions.DisableFormatting);
    }

    private RndcResponse ParseResponse(string responseXml)
    {
        try
        {
            var doc = XDocument.Parse(responseXml);

            // Buscar el elemento de respuesta
            XNamespace ns = RndcConstants.SoapNamespace;
            var responseElement = doc.Descendants(ns + "AtenderMensajeRNDCResponse").FirstOrDefault();

            if (responseElement == null)
            {
                responseElement = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "AtenderMensajeRNDCResponse");
            }

            if (responseElement == null)
            {
                return new RndcResponse
                {
                    IsSuccess = false,
                    ResponseXml = responseXml,
                    ErrorCode = "PARSE_ERROR",
                    ErrorDetails = "No se encontró elemento de respuesta en el XML",
                    Message = "Respuesta del RNDC en formato inesperado"
                };
            }

            var returnElement = responseElement.Element("return") ?? responseElement.Elements().FirstOrDefault();
            var responseContent = returnElement?.Value;

            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return new RndcResponse
                {
                    IsSuccess = false,
                    ResponseXml = responseXml,
                    ErrorCode = "EMPTY_RESPONSE",
                    ErrorDetails = "La respuesta del RNDC está vacía",
                    Message = "No se recibió información del RNDC"
                };
            }

            // Parsear el contenido de la respuesta (es otro XML)
            var responseDoc = XDocument.Parse(responseContent);
            var rootElement = responseDoc.Root;

            if (rootElement == null)
            {
                return new RndcResponse
                {
                    IsSuccess = false,
                    ResponseXml = responseXml,
                    ErrorCode = "INVALID_XML",
                    ErrorDetails = "XML de respuesta inválido",
                    Message = "Formato de respuesta inválido"
                };
            }

            // Verificar si hay errores
            var errorElement = rootElement.Element("error") ?? rootElement.Element("ERROR");
            if (errorElement != null)
            {
                var errorCode = errorElement.Element("codigo")?.Value ?? errorElement.Element("CODIGO")?.Value;
                var errorMessage = errorElement.Element("mensaje")?.Value ?? errorElement.Element("MENSAJE")?.Value;

                return new RndcResponse
                {
                    IsSuccess = false,
                    ResponseXml = responseXml,
                    ErrorCode = errorCode,
                    ErrorDetails = errorMessage,
                    Message = errorMessage ?? "Error reportado por el RNDC"
                };
            }

            // Buscar el IngresoId
            var ingresoId = rootElement.Element("ingresoid")?.Value ??
                           rootElement.Element("INGRESOID")?.Value ??
                           rootElement.Descendants().FirstOrDefault(e => e.Name.LocalName.ToLower() == "ingresoid")?.Value;

            if (string.IsNullOrWhiteSpace(ingresoId))
            {
                return new RndcResponse
                {
                    IsSuccess = false,
                    ResponseXml = responseXml,
                    ErrorCode = "NO_INGRESOID",
                    ErrorDetails = "No se recibió número de radicado (IngresoId)",
                    Message = "La transacción no retornó número de radicado"
                };
            }

            return new RndcResponse
            {
                IsSuccess = true,
                IngresoId = ingresoId,
                ResponseXml = responseXml,
                Message = "Transacción procesada exitosamente"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al parsear respuesta del RNDC");
            return new RndcResponse
            {
                IsSuccess = false,
                ResponseXml = responseXml,
                ErrorCode = "PARSE_EXCEPTION",
                ErrorDetails = ex.Message,
                Message = "Error al procesar la respuesta del RNDC"
            };
        }
    }
}
