
using HealthBridge.Application.Interfaces;
using HealthBridge.Domain.Entities;
using HealthBridge.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthBridge.Infrastructure.Repositories;

public class PacienteRepository : IPacienteRepository
{
    private readonly HealthBridgeDbContext _context;

    public PacienteRepository(HealthBridgeDbContext context)
    {
        _context = context;
    }

    public async Task<Paciente?> ObterPorIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Pacientes
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);
    }

    public async Task<Paciente?> ObterPorNumeroProcessoAsync(
        string numeroProcesso,
        CancellationToken cancellationToken = default)
    {
        return await _context.Pacientes
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => p.NumeroProcesso == numeroProcesso,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Paciente>> ListarAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Pacientes
            .AsNoTracking()
            .OrderBy(p => p.NomeCompleto)
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(
        Paciente paciente,
        CancellationToken cancellationToken = default)
    {
        await _context.Pacientes.AddAsync(
            paciente,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
