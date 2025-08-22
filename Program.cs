
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using ClinicaVitaliApi.Auth;
using ClinicaVitaliApi.Models;
using ClinicaVitaliApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IRepository<Paciente>, JsonRepository<Paciente>>();
builder.Services.AddSingleton<IRepository<Medico>, JsonRepository<Medico>>();
builder.Services.AddSingleton<IRepository<Cita>, JsonRepository<Cita>>();
builder.Services.AddSingleton<IRepository<Historial>, JsonRepository<Historial>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Clinica Vitali API",
        Version = "v1",
        Description = "API RESTful para clínica médica Vitali"
    });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basic" } }, Array.Empty<string>() }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vitali API v1"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
