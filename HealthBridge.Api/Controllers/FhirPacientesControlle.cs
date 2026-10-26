
using HealthBridge.Application.Services;
using HealthBridge.Infrastructure.Fhir;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace HealthBridge.Api.Controllers;

[ApiController]
[Route("api/fhir/pacientes")]
public class FhirPacientesController : ControllerBase
{
    private readonly PacienteService _pacienteService;
    private readonly PacienteFhirMapper _mapper;

    public FhirPacientesController(
        PacienteService pacienteService,
        PacienteFhirMapper mapper)
    {
        _pacienteService = pacienteService;
        _mapper = mapper;
    }

    // GET: api/fhir/pacientes/HB0001
    [HttpGet("{numeroProcesso}")]
    public async Task<IActionResult> ObterPacienteFhir(
        string numeroProcesso,
        CancellationToken cancellationToken)
    {
        var paciente =
            await _pacienteService.ObterPorNumeroProcessoAsync(
                numeroProcesso,
                cancellationToken);

        if (paciente is null)
            return NotFound();

        var recursoFhir = _mapper.Mapear(paciente);

        var serializer = new FhirJsonSerializer();

        var json = serializer.SerializeToString(recursoFhir);

        return Content(
            json,
            "application/fhir+json; charset=utf-8");
    }
}
