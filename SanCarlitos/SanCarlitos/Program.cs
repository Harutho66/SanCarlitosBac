using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SanCarlitos.Configuration;
using SanCarlitos.Data;
using SanCarlitos.Mappings;
using SanCarlitos.Models;
using SanCarlitos.Repositories;
using SanCarlitos.Repositories.Interfaces;
using SanCarlitos.Services;
using SanCarlitos.Services.Autenticacion;
using SanCarlitos.Services.Interfaces;
using SanCarlitos.UnitOfWork;
using SanCarlitos.UnitOfWork.Interfaces;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text;
using VeterinariaAPI.OpenApi;

var builder = WebApplication.CreateBuilder(args);



var jwtSettings = builder.Configuration
    .GetSection(JwtSettings.SectionName)
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException(
        "No se encontró la sección Jwt en appsettings.json.");

if (string.IsNullOrWhiteSpace(jwtSettings.Issuer) ||
    string.IsNullOrWhiteSpace(jwtSettings.Audience))
{
    throw new InvalidOperationException(
        "Jwt:Issuer y Jwt:Audience son obligatorios.");
}

if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey) ||
    Encoding.UTF8.GetByteCount(jwtSettings.SecretKey) < 32)
{
    throw new InvalidOperationException(
        "Jwt:SecretKey debe tener al menos 32 bytes UTF-8.");
}

if (jwtSettings.ExpirationMinutes <= 0)
{
    throw new InvalidOperationException(
        "Jwt:ExpirationMinutes debe ser mayor que cero.");
}

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.SectionName));



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<
        BearerSecuritySchemeTransformer>();

    options.AddOperationTransformer<
        BearerSecuritySchemeTransformer>();
});


builder.Services.AddDbContext<VeterinariaContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("conexion")));


builder.Services.AddAutoMapper(
    cfg => { },
    typeof(MappingProfile)
);


// REPOSITORIO GENÉRICO
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();


// Repositories

builder.Services.AddScoped<IMascotaRepository,MascotaRepository>();
builder.Services.AddScoped<IPropietarioRepository,PropietarioRepository>();
builder.Services.AddScoped<IVeterinarioRepository, VeterinarioRepository>();
builder.Services.AddScoped<ITratamientoRepository,TratamientoRepository>();
builder.Services.AddScoped<IConsultaRepository,ConsultaRepository>();
builder.Services.AddScoped<IReporteRepository, ReporteRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
// Servicios
builder.Services.AddScoped<IMascotaService, MascotaService>();
builder.Services.AddScoped<IPropietarioService,  PropietarioService>();
builder.Services.AddScoped<IVeterinarioService, VeterinarioService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();
builder.Services.AddScoped<ITratamientoService,TratamientoService>();
builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher<Usuario>,
    PasswordHasher<Usuario>>();



builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        jwtSettings.SecretKey)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30),

                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role
            };
    });

builder.Services.AddAuthorization();




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
        options.AddPreferredSecuritySchemes("Bearer"));
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
