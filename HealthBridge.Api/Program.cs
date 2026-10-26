
using HealthBridge.Application.Interfaces;
using HealthBridge.Application.Services;
using HealthBridge.Infrastructure.Data;
using HealthBridge.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using HealthBridge.Infrastructure.Fhir;


var builder = WebApplication.CreateBuilder(args);
// Serviço de transformação para HL7 FHIR R4
builder.Services.AddScoped<PacienteFhirMapper>();
// Controladores da API
builder.Services.AddControllers();

// Documentação OpenAPI
builder.Services.AddOpenApi();

// Configuração do Entity Framework Core
builder.Services.AddDbContext<HealthBridgeDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "HealthBridgeConnection")));


// Cliente HTTP para comunicação com o servidor FHIR
builder.Services.AddHttpClient<FhirServerClient>(client =>
{
    var baseUrl = builder.Configuration[
        "FhirServer:BaseUrl"];

    if (string.IsNullOrWhiteSpace(baseUrl))
    {
        throw new InvalidOperationException(
            "O endereço do servidor FHIR não está configurado.");
    }

    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});


// Registo dos repositórios
builder.Services.AddScoped<
    IPacienteRepository,
    PacienteRepository>();

// Registo dos serviços aplicacionais
builder.Services.AddScoped<PacienteService>();

var app = builder.Build();

// Documentação OpenAPI em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
