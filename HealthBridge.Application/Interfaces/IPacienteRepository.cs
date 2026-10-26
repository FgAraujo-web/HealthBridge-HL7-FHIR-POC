
using HealthBridge.Domain.Entities;

namespace HealthBridge.Application.Interfaces;
//define as operações que uma implementação concreta terá de disponibilizar
public interface IPacienteRepository
{
    Task<Paciente?> ObterPorIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<Paciente?> ObterPorNumeroProcessoAsync(
        string numeroProcesso,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Paciente>> ListarAsync(
        CancellationToken cancellationToken = default);

    Task AdicionarAsync(
        Paciente paciente,
        CancellationToken cancellationToken = default);
}
