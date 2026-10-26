
namespace HealthBridge.Domain.Entities;
//Esta entidade vai representar uma consulta ou atendimento clínico realizado a um paciente. Posteriormente, será transformada no recurso Encounter do HL7 FHIR R4.
public class Consulta
{
    public int Id { get; set; }

    public int PacienteId { get; set; }

    public Paciente Paciente { get; set; } = null!;

    public string NumeroConsulta { get; set; } = string.Empty;

    public DateTime DataInicio { get; set; }

    public DateTime? DataFim { get; set; }

    public string TipoConsulta { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string? MotivoConsulta { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
