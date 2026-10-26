
using HealthBridge.Application.Interfaces;
using HealthBridge.Domain.Entities;
using System.Runtime.Intrinsics.X86;

namespace HealthBridge.Application.Services;

/*O serviço verifica se 123 é um identificador válido e chama o método correspondente do repositório.
Se o identificador for 0 ou negativo, lança uma exceção antes de tentar consultar a base de dados.*/
public class PacienteService
{
    private readonly IPacienteRepository _pacienteRepository;

    public PacienteService(IPacienteRepository pacienteRepository)
    {
        _pacienteRepository = pacienteRepository;
    }

    public async Task<Paciente?> ObterPorIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "O identificador do paciente deve ser superior a zero.",
                nameof(id));
        }

        return await _pacienteRepository.ObterPorIdAsync(
            id,
            cancellationToken);
    }

    public async Task<Paciente?> ObterPorNumeroProcessoAsync(
        string numeroProcesso,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroProcesso))
        {
            throw new ArgumentException(
                "O número de processo clínico é obrigatório.",
                nameof(numeroProcesso));
        }

        return await _pacienteRepository.ObterPorNumeroProcessoAsync(
            numeroProcesso.Trim(),
            cancellationToken);
    }
}
