# Guía de Uso de Handlers RNDC

## Handlers Implementados

Se han creado todos los handlers necesarios para las transacciones del RNDC. A continuación se describe cómo usar cada uno.

---

## 1. Registro de Terceros (Proceso 11)

### Comando: `RegistrarTerceroCommand`

```csharp
using ApiRndc.Application.Commands;
using ApiRndc.Shared.DTOs;
using MediatR;

// Inyectar IMediator en tu controlador o servicio
public class TercerosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TercerosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] TerceroDto tercero)
    {
        var command = new RegistrarTerceroCommand
        {
            Tercero = tercero,
            CreatedBy = User.Identity?.Name
        };

        var result = await _mediator.Send(command);

        if (result.Success)
        {
            return Ok(new {
                message = "Tercero registrado exitosamente",
                ingresoId = result.IngresoId,
                transactionId = result.TransactionId
            });
        }

        return BadRequest(new { error = result.ErrorMessage });
    }
}
```

### Ejemplo de Uso

```csharp
var terceroDto = new TerceroDto
{
    NumNitEmpresaTransporte = "860504882",
    CodTipoIdTercero = "N",
    NumIdTercero = "75145705",
    NomIdTercero = "Francisco",
    PrimerApellidoIdTercero = "Montoya",
    SegundoApellidoIdTercero = "Lopez",
    NumTelefonoContacto = "3166917374",
    NomenclaturaDireccion = "Carrera 59 # 70-125",
    CodMunicipioRndc = "5023112",
    CodSedeTercero = "1",
    NomSedeTercero = "Planta origen",
    NumLicenciaConduccion = "75145705",
    CodCategoriaLicenciaConduccion = "C3",
    FechaVencimientoLicencia = DateTime.Parse("2026-10-17"),
    Latitud = 4.563256m,
    Longitud = -74.14451m,
    RegimenSimple = "N"
};

var command = new RegistrarTerceroCommand { Tercero = terceroDto, CreatedBy = "admin" };
var result = await _mediator.Send(command);

Console.WriteLine($"Success: {result.Success}");
Console.WriteLine($"IngresoId: {result.IngresoId}");
```

---

## 2. Registro de Vehículos (Proceso 12)

### Comando: `RegistrarVehiculoCommand`

```csharp
var vehiculoDto = new VehiculoDto
{
    NumNitEmpresaTransporte = "860504882",
    NumPlaca = "SYL262",
    CodConfiguracionUnidadCarga = "55",
    CodMarcaVehiculoCarga = "1",
    CodLineaVehiculoCarga = "373",
    AnoFabricacionVehiculoCarga = 2020,
    CodTipoIdPropietario = "N",
    NumIdPropietario = "860504882",
    CodTipoIdTenedor = "N",
    NumIdTenedor = "860504882",
    CodTipoCombustible = "1",
    PesoVehiculoVacio = 8645m,
    CodColorVehiculoCarga = "9439",
    CodTipoCarroceria = "0",
    NumNitAseguradoraSoat = "8110191907",
    FechaVencimientoSoat = DateTime.Parse("2026-10-14"),
    NumSeguroSoat = "AT131811151729",
    UnidadMedidaCapacidad = "1"
};

var command = new RegistrarVehiculoCommand
{
    Vehiculo = vehiculoDto,
    CreatedBy = "admin"
};

var result = await _mediator.Send(command);

if (result.Success)
{
    Console.WriteLine($"Vehículo registrado. IngresoId: {result.IngresoId}");
}
else
{
    Console.WriteLine($"Error: {result.ErrorMessage}");
}
```

### Validaciones Automáticas

- ✅ Verifica si la placa ya está registrada
- ✅ Almacena el vehículo en BD después de envío exitoso
- ✅ Registra la transacción completa (request y response XML)

---

## 3. Expedición de Remesa (Proceso 3)

### Comando: `ExpedirRemesaCommand`

```csharp
var remesaDto = new RemesaDto
{
    NumNitEmpresaTransporte = "860504882",
    ConsecutivoRemesa = "12345789",
    ConsecutivoInformacionCarga = "1",
    CodOperacionTransporte = "1",
    CodNaturalezaCarga = "1",
    CantidadCargada = 32000m,
    UnidadMedidaCapacidad = "1",
    CodTipoEmpaque = "1",
    PesoContenedorVacio = 6236m,
    MercanciaRemesa = "1",
    DescripcionCortaProducto = "maiz",

    // Remitente
    CodTipoIdRemitente = "N",
    NumIdRemitente = "860504882",
    CodSedeRemitente = "9439",

    // Destinatario
    CodTipoIdDestinatario = "N",
    NumIdDestinatario = "860504882",
    CodSedeDestinatario = "9349",

    // Póliza
    DuenoPoliza = "E",
    NumPolizaTransporte = "19874542121",
    CompaniaSeguro = "860504882",
    FechaVencimientoPolizaCarga = DateTime.Parse("2026-08-08"),

    // Tiempos
    HorasPactoCarga = 4,
    MinutosPactoCarga = 60,
    HorasPactoDescargue = 4,
    MinutosPactoDescargue = 60,

    // Propietario
    CodTipoIdPropietario = "N",
    NumIdPropietario = "860504882",
    CodSedePropietario = "5997",
    OrdenServicioGenerador = "OS4565887",

    // Fechas de cita
    FechaCitaPactadaCargue = DateTime.Parse("2025-10-20"),
    HoraCitaPactadaCargue = "09:10",
    FechaCitaPactadaDescargue = DateTime.Parse("2025-10-20"),
    HoraCitaPactadaDescargueRemesa = "09:30",

    PermisoCargaExtra = "N",
    NumIdGps = "454545",
    CodigoUn = "45454",
    SubpartidaCode = "45454",
    CodigoArancelCode = "454545",
    GrupoEmbalajeEnvase = "48454",
    EstadoMercancia = "OK",
    UnidadMedidaProducto = "1",
    CantidadProducto = 1620m
};

var command = new ExpedirRemesaCommand
{
    Remesa = remesaDto,
    CreatedBy = "admin"
};

var result = await _mediator.Send(command);

if (result.Success)
{
    Console.WriteLine($"Remesa expedida. IngresoId: {result.IngresoId}");
    Console.WriteLine($"Consecutivo: {result.ConsecutivoRemesa}");
}
```

### Validaciones Automáticas

- ✅ Verifica si el consecutivo ya existe
- ✅ Almacena la remesa en BD
- ✅ Guarda XMLs de request y response

---

## 4. Expedición de Manifiesto (Proceso 4)

### Comando: `ExpedirManifiestoCommand`

```csharp
var manifiestoDto = new ManifiestoDto
{
    NumNitEmpresaTransporte = "860504882",
    NumManifiestoCarga = "MAN20251020001",
    ConsecutivoInformacionViaje = "1",
    ManNroManifiestoTransbordo = "1",
    CodOperacionTransporte = "1",
    FechaExpedicionManifiesto = DateTime.Now,
    CodMunicipioOrigenManifiesto = "658985",
    CodMunicipioDestinoManifiesto = "454574",

    // Titular
    CodIdTitularManifiesto = "N",
    NumIdTitularManifiesto = "860504882",

    // Vehículo y conductor
    NumPlaca = "SYL262",
    NumPlacaRemolque = "S243658",
    CodIdConductor = "C",
    NumIdConductor = "75145705",
    CodIdConductor2 = "N",
    NumIdConductor2 = "5944156",

    // Valores
    ValorFletePactadoViaje = 18965236m,
    RetencionFuenteManifiesto = 1.2m,
    RetencionIcaManifiestoCarga = 1.1m,
    ValorAnticipoManifiesto = 598639m,
    CodMunicipioPagoSaldo = "594565",

    CodResponsablePagoCargue = "R",
    CodResponsablePagoDescargue = "E",
    FechaPagoSaldoManifiesto = DateTime.Parse("2025-10-20"),
    NitMonitoreoFlota = "860504882",
    AceptacionElectronica = "S",
    Observaciones = "Sin observaciones",

    // Remesas asociadas (IMPORTANTE: Deben existir previamente)
    ConsecutivosRemesas = new List<string>
    {
        "12345789",
        "12345790",
        "12345791"
    }
};

var command = new ExpedirManifiestoCommand
{
    Manifiesto = manifiestoDto,
    CreatedBy = "admin"
};

var result = await _mediator.Send(command);

if (result.Success)
{
    Console.WriteLine($"Manifiesto expedido. IngresoId: {result.IngresoId}");
}
```

### Validaciones Automáticas

- ✅ Verifica que el manifiesto no exista
- ✅ **Valida que todas las remesas asociadas existan**
- ✅ Crea relaciones manifiesto-remesa en BD
- ✅ Construye XML con formato especial para remesas

---

## 5. Cumplido de Remesa o Manifiesto (Procesos 5 y 6)

### Comando: `RegistrarCumplidoCommand`

```csharp
// Cumplido de REMESA (Proceso 5)
var cumplidoRemesa = new CumplidoDto
{
    NumNitEmpresaTransporte = "860504882",
    Consecutivo = "12345789", // Consecutivo de la remesa
    FechaCumplido = DateTime.Now,
    Latitud = 4.563256m,
    Longitud = -74.14451m,
    Observaciones = "Entrega exitosa"
};

var commandRemesa = new RegistrarCumplidoCommand
{
    Cumplido = cumplidoRemesa,
    TipoCumplido = TransactionType.CumplidoRemesa, // Importante!
    CreatedBy = "admin"
};

var resultRemesa = await _mediator.Send(commandRemesa);

// Cumplido de MANIFIESTO (Proceso 6)
var cumplidoManifiesto = new CumplidoDto
{
    NumNitEmpresaTransporte = "860504882",
    Consecutivo = "MAN20251020001", // Número de manifiesto
    FechaCumplido = DateTime.Now,
    Latitud = 4.563256m,
    Longitud = -74.14451m,
    Observaciones = "Viaje completado"
};

var commandManifiesto = new RegistrarCumplidoCommand
{
    Cumplido = cumplidoManifiesto,
    TipoCumplido = TransactionType.CumplidoManifiesto, // Importante!
    CreatedBy = "admin"
};

var resultManifiesto = await _mediator.Send(commandManifiesto);
```

### Validaciones Automáticas

- ✅ Verifica que la remesa/manifiesto exista
- ✅ Verifica que tenga IngresoId (fue expedida exitosamente)
- ✅ Envía al RNDC con el XML correcto según el tipo

---

## Flujo Típico de Uso

### 1. Preparación (Una sola vez)

```csharp
// 1.1 Registrar terceros (conductores, propietarios)
var tercero = await _mediator.Send(new RegistrarTerceroCommand { ... });

// 1.2 Registrar vehículos
var vehiculo = await _mediator.Send(new RegistrarVehiculoCommand { ... });
```

### 2. Por Cada Viaje

```csharp
// 2.1 Expedir remesas (una o más)
var remesa1 = await _mediator.Send(new ExpedirRemesaCommand { ... });
var remesa2 = await _mediator.Send(new ExpedirRemesaCommand { ... });

// 2.2 Expedir manifiesto con las remesas
var manifiesto = await _mediator.Send(new ExpedirManifiestoCommand
{
    Manifiesto = new ManifiestoDto
    {
        ConsecutivosRemesas = new List<string>
        {
            remesa1.ConsecutivoRemesa,
            remesa2.ConsecutivoRemesa
        },
        ...
    }
});

// 2.3 Registrar cumplidos cuando se entregue
var cumplido = await _mediator.Send(new RegistrarCumplidoCommand
{
    TipoCumplido = TransactionType.CumplidoManifiesto,
    ...
});
```

---

## Manejo de Errores

Todos los handlers siguen el mismo patrón de manejo de errores:

```csharp
try
{
    var result = await _mediator.Send(command);

    if (result.Success)
    {
        // Éxito - El IngresoId está disponible
        Console.WriteLine($"Transacción exitosa: {result.IngresoId}");

        // La transacción se guardó en BD con estado Success
        var transaction = await _unitOfWork.RndcTransactions
            .GetByIdAsync(result.TransactionId);
    }
    else
    {
        // Error del RNDC - Revisar ErrorMessage
        Console.WriteLine($"Error RNDC: {result.ErrorMessage}");

        // La transacción se guardó en BD con estado Failed
        // Incluye el XML de error para debugging
    }
}
catch (Exception ex)
{
    // Error de aplicación (BD, red, etc.)
    Console.WriteLine($"Error de sistema: {ex.Message}");
}
```

---

## Consultar Transacciones

```csharp
// Por ID
var transaction = await _unitOfWork.RndcTransactions.GetByIdAsync(transactionId);

// Por estado
var pending = await _unitOfWork.RndcTransactions.FindAsync(
    t => t.Status == TransactionStatus.Pending);

var failed = await _unitOfWork.RndcTransactions.FindAsync(
    t => t.Status == TransactionStatus.Failed);

// Por tipo
var remesas = await _unitOfWork.RndcTransactions.FindAsync(
    t => t.TransactionType == TransactionType.ExpedicionRemesa);

// Por fecha
var today = DateTime.Today;
var todayTransactions = await _unitOfWork.RndcTransactions.FindAsync(
    t => t.CreatedAt >= today && t.CreatedAt < today.AddDays(1));
```

---

## Consultar Entidades

```csharp
// Terceros
var tercero = await _unitOfWork.Terceros.FirstOrDefaultAsync(
    t => t.NumIdTercero == "75145705");

// Vehículos
var vehiculo = await _unitOfWork.Vehiculos.FirstOrDefaultAsync(
    v => v.NumPlaca == "SYL262");

// Remesas
var remesa = await _unitOfWork.Remesas.FirstOrDefaultAsync(
    r => r.ConsecutivoRemesa == "12345789");

// Manifiestos con remesas
var manifiesto = await _unitOfWork.Manifiestos.FirstOrDefaultAsync(
    m => m.NumManifiestoCarga == "MAN20251020001");

// Ver remesas asociadas al manifiesto
var remesasDelManifiesto = await _unitOfWork.ManifiestoRemesas.FindAsync(
    mr => mr.ManifiestoId == manifiesto.Id);
```

---

## Logging

Todos los handlers registran eventos importantes:

```
[INF] Iniciando registro de tercero: 75145705
[INF] Registro de tercero completado. Success: True, IngresoId: 123456
[WRN] El vehículo con placa SYL262 ya está registrado
[ERR] Error al expedir remesa: 12345789
```

Ubicación de logs: `Logs/log-YYYYMMDD.txt`

---

## Próximos Pasos

1. ✅ Todos los handlers están implementados
2. ⏳ Crear endpoints de API (Controllers o Minimal API)
3. ⏳ Crear páginas Blazor para UI
4. ⏳ Implementar validadores con FluentValidation
5. ⏳ Agregar sistema de reintentos automáticos

---

## Ejemplo Completo: Controlador de API

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RndcController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RndcController> _logger;

    public RndcController(IMediator mediator, ILogger<RndcController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("terceros")]
    public async Task<ActionResult<RegistrarTerceroResult>> RegistrarTercero(
        [FromBody] TerceroDto tercero)
    {
        var command = new RegistrarTerceroCommand
        {
            Tercero = tercero,
            CreatedBy = User.Identity?.Name
        };

        var result = await _mediator.Send(command);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("vehiculos")]
    public async Task<ActionResult<RegistrarVehiculoResult>> RegistrarVehiculo(
        [FromBody] VehiculoDto vehiculo)
    {
        var command = new RegistrarVehiculoCommand
        {
            Vehiculo = vehiculo,
            CreatedBy = User.Identity?.Name
        };

        var result = await _mediator.Send(command);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("remesas")]
    public async Task<ActionResult<ExpedirRemesaResult>> ExpedirRemesa(
        [FromBody] RemesaDto remesa)
    {
        var command = new ExpedirRemesaCommand
        {
            Remesa = remesa,
            CreatedBy = User.Identity?.Name
        };

        var result = await _mediator.Send(command);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("manifiestos")]
    public async Task<ActionResult<ExpedirManifiestoResult>> ExpedirManifiesto(
        [FromBody] ManifiestoDto manifiesto)
    {
        var command = new ExpedirManifiestoCommand
        {
            Manifiesto = manifiesto,
            CreatedBy = User.Identity?.Name
        };

        var result = await _mediator.Send(command);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("cumplidos/remesa")]
    public async Task<ActionResult<RegistrarCumplidoResult>> CumplidoRemesa(
        [FromBody] CumplidoDto cumplido)
    {
        var command = new RegistrarCumplidoCommand
        {
            Cumplido = cumplido,
            TipoCumplido = TransactionType.CumplidoRemesa,
            CreatedBy = User.Identity?.Name
        };

        var result = await _mediator.Send(command);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("cumplidos/manifiesto")]
    public async Task<ActionResult<RegistrarCumplidoResult>> CumplidoManifiesto(
        [FromBody] CumplidoDto cumplido)
    {
        var command = new RegistrarCumplidoCommand
        {
            Cumplido = cumplido,
            TipoCumplido = TransactionType.CumplidoManifiesto,
            CreatedBy = User.Identity?.Name
        };

        var result = await _mediator.Send(command);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
```

---

**Última actualización**: 20 de Octubre de 2025
**Estado**: Todos los handlers implementados y probados ✅
