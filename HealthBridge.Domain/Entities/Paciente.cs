
namespace HealthBridge.Domain.Entities;
//Esta será a primeira entidade do nosso modelo clínico. Representará um paciente fictício registado no sistema de origem, antes da transformação para o recurso Patient do HL7 FHIR.
public class Paciente
{
    public int Id { get; set; }

    public string NumeroProcesso { get; set; } = string.Empty;

    public string NomeCompleto { get; set; } = string.Empty;

    public DateOnly DataNascimento { get; set; }

    public string Sexo { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Telefone { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
