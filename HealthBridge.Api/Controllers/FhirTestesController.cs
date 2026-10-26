
using HealthBridge.Domain.Entities;
using HealthBridge.Infrastructure.Fhir;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace HealthBridge.Api.Controllers;

[ApiController]
[Route("api/fhir/testes")]
public class FhirTestesController : ControllerBase
{
    // GET: api/fhir/testes/encounter
    [HttpGet("encounter")]
    public IActionResult TestarEncounter()
    {
        // Paciente inteiramente fictício
        var paciente = new Paciente
        {
            Id = 999,
            NumeroProcesso = "HBTEST001",
            NomeCompleto = "Paciente Ficticio HealthBridge",
            DataNascimento = new DateOnly(1990, 1, 15),
            Sexo = "Outro"
        };

        // Consulta fictícia
        var consulta = new Consulta
        {
            Id = 999,
            PacienteId = paciente.Id,
            Paciente = paciente,
            NumeroConsulta = "CONS-TEST-001",
            DataInicio = new DateTime(
                2026, 10, 26, 10, 0, 0,
                DateTimeKind.Utc),
            DataFim = new DateTime(
                2026, 10, 26, 10, 30, 0,
                DateTimeKind.Utc),
            TipoConsulta = "Medicina Geral",
            Estado = "concluída",
            MotivoConsulta = "Consulta fictícia de demonstração"
        };

        var mapper = new ConsultaFhirMapper();

        var encounter = mapper.Mapear(consulta);

        var serializer = new FhirJsonSerializer();

        var json = serializer.SerializeToString(encounter);

        return Content(
            json,
            "application/fhir+json; charset=utf-8");
    }


    // GET: api/fhir/testes/observation
    [HttpGet("observation")]
    public IActionResult TestarObservation()
    {
        // Paciente inteiramente fictício
        var paciente = new Paciente
        {
            Id = 999,
            NumeroProcesso = "HBTEST001",
            NomeCompleto = "Paciente Ficticio HealthBridge",
            DataNascimento = new DateOnly(1990, 1, 15),
            Sexo = "Outro"
        };

        // Resultado laboratorial fictício
        var resultado = new ResultadoLaboratorial
        {
            Id = 999,
            PacienteId = paciente.Id,
            Paciente = paciente,
            CodigoAnalise = "HB-GLIC-001",
            NomeAnalise = "Glicose",
            Valor = 92m,
            Unidade = "mg/dL",
            DataColheita = new DateTime(
                2026, 10, 26, 9, 30, 0,
                DateTimeKind.Utc),
            Estado = "final"
        };

        var mapper = new ResultadoLaboratorialFhirMapper();

        var observation = mapper.Mapear(resultado);

        var serializer = new FhirJsonSerializer();

        var json = serializer.SerializeToString(observation);

        return Content(
            json,
            "application/fhir+json; charset=utf-8");
    }

}
