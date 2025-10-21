CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public."AspNetRoles" (
        "Id" text NOT NULL,
        "Name" character varying(256),
        "NormalizedName" character varying(256),
        "ConcurrencyStamp" text,
        CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public."AspNetUsers" (
        "Id" text NOT NULL,
        "UserName" character varying(256),
        "NormalizedUserName" character varying(256),
        "Email" character varying(256),
        "NormalizedEmail" character varying(256),
        "EmailConfirmed" boolean NOT NULL,
        "PasswordHash" text,
        "SecurityStamp" text,
        "ConcurrencyStamp" text,
        "PhoneNumber" text,
        "PhoneNumberConfirmed" boolean NOT NULL,
        "TwoFactorEnabled" boolean NOT NULL,
        "LockoutEnd" timestamp with time zone,
        "LockoutEnabled" boolean NOT NULL,
        "AccessFailedCount" integer NOT NULL,
        CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public.manifiestos (
        "Id" uuid NOT NULL,
        "NumNitEmpresaTransporte" character varying(50) NOT NULL,
        "NumManifiestoCarga" character varying(50) NOT NULL,
        "ConsecutivoInformacionViaje" text,
        "ManNroManifiestoTransbordo" text,
        "CodOperacionTransporte" text,
        "FechaExpedicionManifiesto" timestamp with time zone,
        "CodMunicipioOrigenManifiesto" text,
        "CodMunicipioDestinoManifiesto" text,
        "CodIdTitularManifiesto" text,
        "NumIdTitularManifiesto" text,
        "NumPlaca" text,
        "NumPlacaRemolque" text,
        "CodIdConductor" text,
        "NumIdConductor" text,
        "CodIdConductor2" text,
        "NumIdConductor2" text,
        "ValorFletePactadoViaje" numeric,
        "RetencionFuenteManifiesto" numeric,
        "RetencionIcaManifiestoCarga" numeric,
        "ValorAnticipoManifiesto" numeric,
        "CodMunicipioPagoSaldo" text,
        "CodResponsablePagoCargue" text,
        "CodResponsablePagoDescargue" text,
        "FechaPagoSaldoManifiesto" timestamp with time zone,
        "NitMonitoreoFlota" text,
        "AceptacionElectronica" text,
        "Observaciones" text,
        "RemesasAsociadas" text,
        "IngresoId" character varying(100),
        "CreatedAt" timestamp with time zone NOT NULL,
        "CreatedBy" text,
        "UpdatedAt" timestamp with time zone,
        "UpdatedBy" text,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_manifiestos" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public.remesas (
        "Id" uuid NOT NULL,
        "NumNitEmpresaTransporte" character varying(50) NOT NULL,
        "ConsecutivoRemesa" character varying(50) NOT NULL,
        "ConsecutivoInformacionCarga" text,
        "CodOperacionTransporte" text,
        "CodNaturalezaCarga" text,
        "CantidadCargada" numeric,
        "UnidadMedidaCapacidad" text,
        "CodTipoEmpaque" text,
        "PesoContenedorVacio" numeric,
        "MercanciaRemesa" text,
        "DescripcionCortaProducto" text,
        "CodTipoIdRemitente" text,
        "NumIdRemitente" text,
        "CodSedeRemitente" text,
        "CodTipoIdDestinatario" text,
        "NumIdDestinatario" text,
        "CodSedeDestinatario" text,
        "DuenoPoliza" text,
        "NumPolizaTransporte" text,
        "CompaniaSeguro" text,
        "FechaVencimientoPolizaCarga" timestamp with time zone,
        "HorasPactoCarga" integer,
        "MinutosPactoCarga" integer,
        "HorasPactoDescargue" integer,
        "MinutosPactoDescargue" integer,
        "CodTipoIdPropietario" text,
        "NumIdPropietario" text,
        "CodSedePropietario" text,
        "OrdenServicioGenerador" text,
        "FechaCitaPactadaCargue" timestamp with time zone,
        "HoraCitaPactadaCargue" interval,
        "FechaCitaPactadaDescargue" timestamp with time zone,
        "HoraCitaPactadaDescargueRemesa" interval,
        "PermisoCargaExtra" text,
        "NumIdGps" text,
        "CodigoUn" text,
        "SubpartidaCode" text,
        "CodigoArancelCode" text,
        "GrupoEmbalajeEnvase" text,
        "EstadoMercancia" text,
        "UnidadMedidaProducto" text,
        "CantidadProducto" numeric,
        "IngresoId" character varying(100),
        "CreatedAt" timestamp with time zone NOT NULL,
        "CreatedBy" text,
        "UpdatedAt" timestamp with time zone,
        "UpdatedBy" text,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_remesas" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public.rndc_transactions (
        "Id" uuid NOT NULL,
        "TransactionType" integer NOT NULL,
        "Status" integer NOT NULL,
        "IngresoId" character varying(100),
        "RequestXml" text NOT NULL,
        "ResponseXml" text,
        "ErrorMessage" text,
        "ErrorCode" character varying(50),
        "RetryCount" integer NOT NULL,
        "LastRetryAt" timestamp with time zone,
        "SuccessAt" timestamp with time zone,
        "AdditionalData" text,
        "ExternalReference" character varying(100),
        "NitEmpresaTransporte" character varying(50) NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "CreatedBy" text,
        "UpdatedAt" timestamp with time zone,
        "UpdatedBy" text,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_rndc_transactions" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public.terceros (
        "Id" uuid NOT NULL,
        "NumNitEmpresaTransporte" character varying(50) NOT NULL,
        "CodTipoIdTercero" character varying(10) NOT NULL,
        "NumIdTercero" character varying(50) NOT NULL,
        "NomIdTercero" character varying(200) NOT NULL,
        "PrimerApellidoIdTercero" character varying(100),
        "SegundoApellidoIdTercero" character varying(100),
        "NumTelefonoContacto" text,
        "NomenclaturaDireccion" text,
        "CodMunicipioRndc" text,
        "CodSedeTercero" text,
        "NomSedeTercero" text,
        "NumLicenciaConduccion" character varying(50),
        "CodCategoriaLicenciaConduccion" text,
        "FechaVencimientoLicencia" timestamp with time zone,
        "Latitud" numeric,
        "Longitud" numeric,
        "RegimenSimple" text,
        "IngresoId" character varying(100),
        "CreatedAt" timestamp with time zone NOT NULL,
        "CreatedBy" text,
        "UpdatedAt" timestamp with time zone,
        "UpdatedBy" text,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_terceros" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public.vehiculos (
        "Id" uuid NOT NULL,
        "NumNitEmpresaTransporte" character varying(50) NOT NULL,
        "NumPlaca" character varying(20) NOT NULL,
        "CodConfiguracionUnidadCarga" text NOT NULL,
        "CodMarcaVehiculoCarga" text,
        "CodLineaVehiculoCarga" text,
        "AnoFabricacionVehiculoCarga" integer,
        "CodTipoIdPropietario" text,
        "NumIdPropietario" text,
        "CodTipoIdTenedor" text,
        "NumIdTenedor" text,
        "CodTipoCombustible" text,
        "PesoVehiculoVacio" numeric,
        "CodColorVehiculoCarga" text,
        "CodTipoCarroceria" text,
        "NumNitAseguradoraSoat" text,
        "FechaVencimientoSoat" timestamp with time zone,
        "NumSeguroSoat" text,
        "UnidadMedidaCapacidad" text,
        "IngresoId" character varying(100),
        "CreatedAt" timestamp with time zone NOT NULL,
        "CreatedBy" text,
        "UpdatedAt" timestamp with time zone,
        "UpdatedBy" text,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_vehiculos" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public."AspNetRoleClaims" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "RoleId" text NOT NULL,
        "ClaimType" text,
        "ClaimValue" text,
        CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public."AspNetUserClaims" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "UserId" text NOT NULL,
        "ClaimType" text,
        "ClaimValue" text,
        CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public."AspNetUserLogins" (
        "LoginProvider" text NOT NULL,
        "ProviderKey" text NOT NULL,
        "ProviderDisplayName" text,
        "UserId" text NOT NULL,
        CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
        CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public."AspNetUserRoles" (
        "UserId" text NOT NULL,
        "RoleId" text NOT NULL,
        CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
        CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public."AspNetUserTokens" (
        "UserId" text NOT NULL,
        "LoginProvider" text NOT NULL,
        "Name" text NOT NULL,
        "Value" text,
        CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
        CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE TABLE public.manifiesto_remesas (
        "Id" uuid NOT NULL,
        "ManifiestoId" uuid NOT NULL,
        "ConsecutivoRemesa" character varying(50) NOT NULL,
        "RemesaId" uuid,
        "CreatedAt" timestamp with time zone NOT NULL,
        "CreatedBy" text,
        "UpdatedAt" timestamp with time zone,
        "UpdatedBy" text,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_manifiesto_remesas" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_manifiesto_remesas_manifiestos_ManifiestoId" FOREIGN KEY ("ManifiestoId") REFERENCES public.manifiestos ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_manifiesto_remesas_remesas_RemesaId" FOREIGN KEY ("RemesaId") REFERENCES public.remesas ("Id") ON DELETE SET NULL
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON public."AspNetRoleClaims" ("RoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE UNIQUE INDEX "RoleNameIndex" ON public."AspNetRoles" ("NormalizedName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_AspNetUserClaims_UserId" ON public."AspNetUserClaims" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_AspNetUserLogins_UserId" ON public."AspNetUserLogins" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_AspNetUserRoles_RoleId" ON public."AspNetUserRoles" ("RoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "EmailIndex" ON public."AspNetUsers" ("NormalizedEmail");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE UNIQUE INDEX "UserNameIndex" ON public."AspNetUsers" ("NormalizedUserName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_manifiesto_remesas_ManifiestoId" ON public.manifiesto_remesas ("ManifiestoId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_manifiesto_remesas_RemesaId" ON public.manifiesto_remesas ("RemesaId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_manifiestos_IngresoId" ON public.manifiestos ("IngresoId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_manifiestos_NumManifiestoCarga" ON public.manifiestos ("NumManifiestoCarga");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_remesas_ConsecutivoRemesa" ON public.remesas ("ConsecutivoRemesa");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_remesas_IngresoId" ON public.remesas ("IngresoId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_rndc_transactions_CreatedAt" ON public.rndc_transactions ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_rndc_transactions_IngresoId" ON public.rndc_transactions ("IngresoId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_rndc_transactions_Status" ON public.rndc_transactions ("Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_rndc_transactions_TransactionType" ON public.rndc_transactions ("TransactionType");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_terceros_IngresoId" ON public.terceros ("IngresoId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_terceros_NumIdTercero" ON public.terceros ("NumIdTercero");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE INDEX "IX_vehiculos_IngresoId" ON public.vehiculos ("IngresoId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_vehiculos_NumPlaca" ON public.vehiculos ("NumPlaca");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251020231926_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251020231926_InitialCreate', '9.0.10');
    END IF;
END $EF$;
COMMIT;

