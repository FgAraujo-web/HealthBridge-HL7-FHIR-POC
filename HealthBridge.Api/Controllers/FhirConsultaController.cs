
using HealthBridge.Infrastructure.Fhir;
using Microsoft.AspNetCore.Mvc;

namespace HealthBridge.Api.Controllers;

[ApiController]
[Route("api/fhir/consulta")]
public class FhirConsultaController : ControllerBase
{
    private readonly FhirServerClient _fhirClient;
    private readonly ILogger<FhirConsultaController> _logger;

    public FhirConsultaController(
        FhirServerClient fhirClient,
        ILogger<FhirConsultaController> logger)
    {
        _fhirClient = fhirClient;
        _logger = logger;
    }

    // GET: api/fhir/consulta/pacientes/272180
    [HttpGet("pacientes/{idFhir}")]
    public async Task<IActionResult> ObterPaciente(
        string idFhir,
        CancellationToken cancellationToken)
    {
        try
        {
            var json = await _fhirClient.ObterPacienteAsync(
                idFhir,
                cancellationToken);

            if (json is null)
            {
                return NotFound(new
                {
                    mensagem = "Paciente não encontrado no servidor FHIR."
                });
            }

            return Content(
                json,
                "application/fhir+json; charset=utf-8");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensagem = ex.Message
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "Erro na consulta do paciente FHIR {IdFhir}.",
                idFhir);

            return StatusCode(
                StatusCodes.Status502BadGateway,
                new
                {
                    mensagem = "Falha na comunicação com o servidor FHIR."
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


    [HttpGet("pacientes/processo/{numeroProcesso}")]
    public async Task<IActionResult> PesquisarPorProcesso(
        string numeroProcesso,
        CancellationToken cancellationToken)
    {
        try
        {
            var json = await _fhirClient
                .PesquisarPacientePorProcessoAsync(
                    numeroProcesso,
                    cancellationToken);

            return Content(
                json,
                "application/fhir+json; charset=utf-8");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensagem = ex.Message
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "Erro na pesquisa FHIR pelo processo {NumeroProcesso}.",
                numeroProcesso);

            return StatusCode(
                StatusCodes.Status502BadGateway,
                new
                {
                    mensagem = "Falha na pesquisa no servidor FHIR."
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
