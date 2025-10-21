using ApiRndc.Application.Commands;
using ApiRndc.Domain.Interfaces;
using ApiRndc.Infrastructure.Data;
using ApiRndc.Infrastructure.Repositories;
using ApiRndc.Infrastructure.Services;
using ApiRndc.Web.Components;
using ApiRndc.Web.Services;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Agregar servicios de autenticación para Blazor
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

// Configurar DbContext con PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Configuración de contraseñas
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // Configuración de usuario
    options.User.RequireUniqueEmail = true;

    // Configuración de bloqueo
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configurar autenticación con cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

// Registrar MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegistrarTerceroCommand).Assembly));

// Registrar AutoMapper (Descomentar y configurar perfiles cuando sea necesario)
// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Registrar FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(RegistrarTerceroCommand).Assembly);

// Registrar HttpClient para SOAP
builder.Services.AddHttpClient<IRndcSoapClient, RndcSoapClient>(client =>
{
    client.Timeout = TimeSpan.FromMinutes(2);
});

// Registrar servicios de infraestructura
builder.Services.AddScoped<IRndcSoapClient, RndcSoapClient>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Registrar MudBlazor
builder.Services.AddMudServices();

// Agregar servicios de autorización
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("RequireOperadorRole", policy => policy.RequireRole("Administrador", "Operador"));
    options.AddPolicy("RequireConsultaRole", policy => policy.RequireRole("Administrador", "Operador", "Consulta"));
});

// Registrar servicios de memoria caché
builder.Services.AddMemoryCache();

// Registrar servicios de sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Aplicar migraciones automáticamente en desarrollo
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        try
        {
            db.Database.Migrate();
            Log.Information("Migraciones aplicadas exitosamente");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error al aplicar migraciones");
        }

        // Crear roles por defecto
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "Administrador", "Operador", "Consulta" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Log.Information("Rol creado: {Role}", role);
            }
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseAntiforgery();

app.UseSerilogRequestLogging();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

Log.Information("Aplicación ApiRndc iniciada correctamente");

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación finalizó inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}
