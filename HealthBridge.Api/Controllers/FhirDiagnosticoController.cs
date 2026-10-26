
using HealthBridge.Infrastructure.Fhir;
using Microsoft.AspNetCore.Mvc;

namespace HealthBridge.Api.Controllers;

[ApiController]
[Route("api/fhir/diagnostico")]
public class FhirDiagnosticoController : ControllerBase
{
    private readonly FhirServerClient _fhirClient;
    private readonly ILogger<FhirDiagnosticoController> _logger;

    public FhirDiagnosticoController(
        FhirServerClient fhirClient,
        ILogger<FhirDiagnosticoController> logger)
    {
        _fhirClient = fhirClient;
        _logger = logger;
    }

    // GET: api/fhir/diagnostico/capacidades
    [HttpGet("capacidades")]
    public async Task<IActionResult> ObterCapacidades(
        CancellationToken cancellationToken)
    {
        try
        {
            var json = await _fhirClient.ObterCapacidadesAsync(
                cancellationToken);

            return Content(
                json,
                "application/fhir+json; charset=utf-8");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(
                ex,
                "Falha na comunicação com o servidor FHIR.");

            return StatusCode(
                StatusCodes.Status502BadGateway,
                new
                {
                    mensagem = "Não foi possível consultar o servidor FHIR."
                });
        }
        catch (TaskCanceledException)
            when (!cancellationToken.IsCancellationRequested)
        {
            return StatusCode(
                StatusCodes.Status504GatewayTimeout,
                new
                {
                    mensagem = "O servidor FHIR excedeu o tempo de resposta."
                });
        }
    }
}
