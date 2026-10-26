
using HealthBridge.Application.Services;
using HealthBridge.Infrastructure.Fhir;
using Microsoft.AspNetCore.Mvc;

namespace HealthBridge.Api.Controllers;

[ApiController]
[Route("api/fhir/publicacao")]
public class FhirPublicacaoController : ControllerBase
{
    private readonly PacienteService _pacienteService;
    private readonly PacienteFhirMapper _mapper;
    private readonly FhirServerClient _fhirClient;
    private readonly ILogger<FhirPublicacaoController> _logger;

    public FhirPublicacaoController(
        PacienteService pacienteService,
        PacienteFhirMapper mapper,
        FhirServerClient fhirClient,
        ILogger<FhirPublicacaoController> logger)
    {
        _pacienteService = pacienteService;
        _mapper = mapper;
        _fhirClient = fhirClient;
        _logger = logger;
    }

    // POST: api/fhir/publicacao/pacientes/HB0001
    [HttpPost("pacientes/{numeroProcesso}")]
    public async Task<IActionResult> PublicarPaciente(
        string numeroProcesso,
        CancellationToken cancellationToken)
    {
        if (!string.Equals(
    numeroProcesso,
    "HBTEST001",
    StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new
            {
                mensagem = "Nesta fase, apenas é permitida a publicação do paciente fictício HBTEST001."
            });
        }
        var paciente = await _pacienteService
            .ObterPorNumeroProcessoAsync(
                numeroProcesso,
                cancellationToken);

        if (paciente is null)
        {
            return NotFound(new
            {
                mensagem = "Paciente não encontrado."
            });
        }

        var pacienteFhir = _mapper.Mapear(paciente);

        try
        {
            var resultado = await _fhirClient
                .PublicarPacienteAsync(
                    pacienteFhir,
                    cancellationToken);

            return Ok(new
            {
                mensagem = "Pedido de publicação aceite pelo servidor FHIR.",
                resposta = resultado
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "Operação de escrita FHIR bloqueada.");

            return StatusCode(
                StatusCodes.Status403Forbidden,
                new
                {
                    mensagem = "A publicação FHIR está desativada."
                });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "Erro na publicação do paciente FHIR.");

            return StatusCode(
                StatusCodes.Status502BadGateway,
                new
                {
                    mensagem = "Falha na publicação no servidor FHIR."
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
