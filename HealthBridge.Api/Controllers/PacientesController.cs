
using HealthBridge.Application.Interfaces;
using HealthBridge.Application.Services;
using HealthBridge.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HealthBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly PacienteService _pacienteService;
    private readonly IPacienteRepository _pacienteRepository;

    public PacientesController(
        PacienteService pacienteService,
        IPacienteRepository pacienteRepository)
    {
        _pacienteService = pacienteService;
        _pacienteRepository = pacienteRepository;
    }

    // GET: api/pacientes
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Paciente>>> Listar(
        CancellationToken cancellationToken)
    {
        var pacientes = await _pacienteRepository.ListarAsync(
            cancellationToken);

        return Ok(pacientes);
    }

    // GET: api/pacientes/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Paciente>> ObterPorId(
        int id,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest("Identificador inválido.");

        var paciente = await _pacienteService.ObterPorIdAsync(
            id,
            cancellationToken);

        if (paciente is null)
            return NotFound();

        return Ok(paciente);
    }

    // GET: api/pacientes/processo/HB0001
    [HttpGet("processo/{numeroProcesso}")]
    public async Task<ActionResult<Paciente>> ObterPorNumeroProcesso(
        string numeroProcesso,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(numeroProcesso))
            return BadRequest("Número de processo obrigatório.");

        var paciente =
            await _pacienteService.ObterPorNumeroProcessoAsync(
                numeroProcesso,
                cancellationToken);

        if (paciente is null)
            return NotFound();

        return Ok(paciente);
    }
}
