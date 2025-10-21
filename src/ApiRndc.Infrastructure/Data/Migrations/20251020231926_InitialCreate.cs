using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApiRndc.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "manifiestos",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NumNitEmpresaTransporte = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumManifiestoCarga = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ConsecutivoInformacionViaje = table.Column<string>(type: "text", nullable: true),
                    ManNroManifiestoTransbordo = table.Column<string>(type: "text", nullable: true),
                    CodOperacionTransporte = table.Column<string>(type: "text", nullable: true),
                    FechaExpedicionManifiesto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CodMunicipioOrigenManifiesto = table.Column<string>(type: "text", nullable: true),
                    CodMunicipioDestinoManifiesto = table.Column<string>(type: "text", nullable: true),
                    CodIdTitularManifiesto = table.Column<string>(type: "text", nullable: true),
                    NumIdTitularManifiesto = table.Column<string>(type: "text", nullable: true),
                    NumPlaca = table.Column<string>(type: "text", nullable: true),
                    NumPlacaRemolque = table.Column<string>(type: "text", nullable: true),
                    CodIdConductor = table.Column<string>(type: "text", nullable: true),
                    NumIdConductor = table.Column<string>(type: "text", nullable: true),
                    CodIdConductor2 = table.Column<string>(type: "text", nullable: true),
                    NumIdConductor2 = table.Column<string>(type: "text", nullable: true),
                    ValorFletePactadoViaje = table.Column<decimal>(type: "numeric", nullable: true),
                    RetencionFuenteManifiesto = table.Column<decimal>(type: "numeric", nullable: true),
                    RetencionIcaManifiestoCarga = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorAnticipoManifiesto = table.Column<decimal>(type: "numeric", nullable: true),
                    CodMunicipioPagoSaldo = table.Column<string>(type: "text", nullable: true),
                    CodResponsablePagoCargue = table.Column<string>(type: "text", nullable: true),
                    CodResponsablePagoDescargue = table.Column<string>(type: "text", nullable: true),
                    FechaPagoSaldoManifiesto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NitMonitoreoFlota = table.Column<string>(type: "text", nullable: true),
                    AceptacionElectronica = table.Column<string>(type: "text", nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    RemesasAsociadas = table.Column<string>(type: "text", nullable: true),
                    IngresoId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_manifiestos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "remesas",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NumNitEmpresaTransporte = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ConsecutivoRemesa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ConsecutivoInformacionCarga = table.Column<string>(type: "text", nullable: true),
                    CodOperacionTransporte = table.Column<string>(type: "text", nullable: true),
                    CodNaturalezaCarga = table.Column<string>(type: "text", nullable: true),
                    CantidadCargada = table.Column<decimal>(type: "numeric", nullable: true),
                    UnidadMedidaCapacidad = table.Column<string>(type: "text", nullable: true),
                    CodTipoEmpaque = table.Column<string>(type: "text", nullable: true),
                    PesoContenedorVacio = table.Column<decimal>(type: "numeric", nullable: true),
                    MercanciaRemesa = table.Column<string>(type: "text", nullable: true),
                    DescripcionCortaProducto = table.Column<string>(type: "text", nullable: true),
                    CodTipoIdRemitente = table.Column<string>(type: "text", nullable: true),
                    NumIdRemitente = table.Column<string>(type: "text", nullable: true),
                    CodSedeRemitente = table.Column<string>(type: "text", nullable: true),
                    CodTipoIdDestinatario = table.Column<string>(type: "text", nullable: true),
                    NumIdDestinatario = table.Column<string>(type: "text", nullable: true),
                    CodSedeDestinatario = table.Column<string>(type: "text", nullable: true),
                    DuenoPoliza = table.Column<string>(type: "text", nullable: true),
                    NumPolizaTransporte = table.Column<string>(type: "text", nullable: true),
                    CompaniaSeguro = table.Column<string>(type: "text", nullable: true),
                    FechaVencimientoPolizaCarga = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HorasPactoCarga = table.Column<int>(type: "integer", nullable: true),
                    MinutosPactoCarga = table.Column<int>(type: "integer", nullable: true),
                    HorasPactoDescargue = table.Column<int>(type: "integer", nullable: true),
                    MinutosPactoDescargue = table.Column<int>(type: "integer", nullable: true),
                    CodTipoIdPropietario = table.Column<string>(type: "text", nullable: true),
                    NumIdPropietario = table.Column<string>(type: "text", nullable: true),
                    CodSedePropietario = table.Column<string>(type: "text", nullable: true),
                    OrdenServicioGenerador = table.Column<string>(type: "text", nullable: true),
                    FechaCitaPactadaCargue = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HoraCitaPactadaCargue = table.Column<TimeSpan>(type: "interval", nullable: true),
                    FechaCitaPactadaDescargue = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HoraCitaPactadaDescargueRemesa = table.Column<TimeSpan>(type: "interval", nullable: true),
                    PermisoCargaExtra = table.Column<string>(type: "text", nullable: true),
                    NumIdGps = table.Column<string>(type: "text", nullable: true),
                    CodigoUn = table.Column<string>(type: "text", nullable: true),
                    SubpartidaCode = table.Column<string>(type: "text", nullable: true),
                    CodigoArancelCode = table.Column<string>(type: "text", nullable: true),
                    GrupoEmbalajeEnvase = table.Column<string>(type: "text", nullable: true),
                    EstadoMercancia = table.Column<string>(type: "text", nullable: true),
                    UnidadMedidaProducto = table.Column<string>(type: "text", nullable: true),
                    CantidadProducto = table.Column<decimal>(type: "numeric", nullable: true),
                    IngresoId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_remesas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rndc_transactions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IngresoId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RequestXml = table.Column<string>(type: "text", nullable: false),
                    ResponseXml = table.Column<string>(type: "text", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    ErrorCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    LastRetryAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SuccessAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AdditionalData = table.Column<string>(type: "text", nullable: true),
                    ExternalReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NitEmpresaTransporte = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rndc_transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "terceros",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NumNitEmpresaTransporte = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CodTipoIdTercero = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NumIdTercero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NomIdTercero = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PrimerApellidoIdTercero = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SegundoApellidoIdTercero = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NumTelefonoContacto = table.Column<string>(type: "text", nullable: true),
                    NomenclaturaDireccion = table.Column<string>(type: "text", nullable: true),
                    CodMunicipioRndc = table.Column<string>(type: "text", nullable: true),
                    CodSedeTercero = table.Column<string>(type: "text", nullable: true),
                    NomSedeTercero = table.Column<string>(type: "text", nullable: true),
                    NumLicenciaConduccion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CodCategoriaLicenciaConduccion = table.Column<string>(type: "text", nullable: true),
                    FechaVencimientoLicencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Latitud = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitud = table.Column<decimal>(type: "numeric", nullable: true),
                    RegimenSimple = table.Column<string>(type: "text", nullable: true),
                    IngresoId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_terceros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vehiculos",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NumNitEmpresaTransporte = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumPlaca = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CodConfiguracionUnidadCarga = table.Column<string>(type: "text", nullable: false),
                    CodMarcaVehiculoCarga = table.Column<string>(type: "text", nullable: true),
                    CodLineaVehiculoCarga = table.Column<string>(type: "text", nullable: true),
                    AnoFabricacionVehiculoCarga = table.Column<int>(type: "integer", nullable: true),
                    CodTipoIdPropietario = table.Column<string>(type: "text", nullable: true),
                    NumIdPropietario = table.Column<string>(type: "text", nullable: true),
                    CodTipoIdTenedor = table.Column<string>(type: "text", nullable: true),
                    NumIdTenedor = table.Column<string>(type: "text", nullable: true),
                    CodTipoCombustible = table.Column<string>(type: "text", nullable: true),
                    PesoVehiculoVacio = table.Column<decimal>(type: "numeric", nullable: true),
                    CodColorVehiculoCarga = table.Column<string>(type: "text", nullable: true),
                    CodTipoCarroceria = table.Column<string>(type: "text", nullable: true),
                    NumNitAseguradoraSoat = table.Column<string>(type: "text", nullable: true),
                    FechaVencimientoSoat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NumSeguroSoat = table.Column<string>(type: "text", nullable: true),
                    UnidadMedidaCapacidad = table.Column<string>(type: "text", nullable: true),
                    IngresoId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehiculos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "public",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "public",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "public",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "public",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "public",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "manifiesto_remesas",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManifiestoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsecutivoRemesa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RemesaId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_manifiesto_remesas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_manifiesto_remesas_manifiestos_ManifiestoId",
                        column: x => x.ManifiestoId,
                        principalSchema: "public",
                        principalTable: "manifiestos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_manifiesto_remesas_remesas_RemesaId",
                        column: x => x.RemesaId,
                        principalSchema: "public",
                        principalTable: "remesas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "public",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "public",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "public",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "public",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "public",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "public",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "public",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_manifiesto_remesas_ManifiestoId",
                schema: "public",
                table: "manifiesto_remesas",
                column: "ManifiestoId");

            migrationBuilder.CreateIndex(
                name: "IX_manifiesto_remesas_RemesaId",
                schema: "public",
                table: "manifiesto_remesas",
                column: "RemesaId");

            migrationBuilder.CreateIndex(
                name: "IX_manifiestos_IngresoId",
                schema: "public",
                table: "manifiestos",
                column: "IngresoId");

            migrationBuilder.CreateIndex(
                name: "IX_manifiestos_NumManifiestoCarga",
                schema: "public",
                table: "manifiestos",
                column: "NumManifiestoCarga");

            migrationBuilder.CreateIndex(
                name: "IX_remesas_ConsecutivoRemesa",
                schema: "public",
                table: "remesas",
                column: "ConsecutivoRemesa");

            migrationBuilder.CreateIndex(
                name: "IX_remesas_IngresoId",
                schema: "public",
                table: "remesas",
                column: "IngresoId");

            migrationBuilder.CreateIndex(
                name: "IX_rndc_transactions_CreatedAt",
                schema: "public",
                table: "rndc_transactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_rndc_transactions_IngresoId",
                schema: "public",
                table: "rndc_transactions",
                column: "IngresoId");

            migrationBuilder.CreateIndex(
                name: "IX_rndc_transactions_Status",
                schema: "public",
                table: "rndc_transactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_rndc_transactions_TransactionType",
                schema: "public",
                table: "rndc_transactions",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_terceros_IngresoId",
                schema: "public",
                table: "terceros",
                column: "IngresoId");

            migrationBuilder.CreateIndex(
                name: "IX_terceros_NumIdTercero",
                schema: "public",
                table: "terceros",
                column: "NumIdTercero");

            migrationBuilder.CreateIndex(
                name: "IX_vehiculos_IngresoId",
                schema: "public",
                table: "vehiculos",
                column: "IngresoId");

            migrationBuilder.CreateIndex(
                name: "IX_vehiculos_NumPlaca",
                schema: "public",
                table: "vehiculos",
                column: "NumPlaca",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "public");

            migrationBuilder.DropTable(
                name: "manifiesto_remesas",
                schema: "public");

            migrationBuilder.DropTable(
                name: "rndc_transactions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "terceros",
                schema: "public");

            migrationBuilder.DropTable(
                name: "vehiculos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "manifiestos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "remesas",
                schema: "public");
        }
    }
}
