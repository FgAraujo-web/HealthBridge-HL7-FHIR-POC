using System;
using System.Collections.Generic;
using System.Text;
//Esta entidade representa o resultado de uma análise clínica realizada a um paciente. Posteriormente, será transformada no recurso Observation do HL7 FHIR R4
namespace HealthBridge.Domain.Entities;

public class ResultadoLaboratorial
{
    public int Id { get; set; }

    public int PacienteId { get; set; }

    public Paciente Paciente { get; set; } = null!;

    public int? ConsultaId { get; set; }

    public Consulta? Consulta { get; set; }

    public string CodigoAnalise { get; set; } = string.Empty;

    public string NomeAnalise { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public string Unidade { get; set; } = string.Empty;

    public DateTime DataColheita { get; set; }

    public DateTime? DataResultado { get; set; }

    public string Estado { get; set; } = "Final";

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
