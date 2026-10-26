
using HealthBridge.Domain.Entities;
using Hl7.Fhir.Model;

namespace HealthBridge.Infrastructure.Fhir;

public class ConsultaFhirMapper
{
    public Encounter Mapear(Consulta consulta)
    {
        ArgumentNullException.ThrowIfNull(consulta);

        if (consulta.Paciente is null)
        {
            throw new ArgumentException(
                "A consulta deve incluir o paciente associado.");
        }

        return new Encounter
        {
            Identifier =
            [
                new Identifier
                {
                    System =
                        "https://healthbridge.example.org/identifiers/consultas",
                    Value = consulta.NumeroConsulta
                }
            ],

            Status = MapearEstado(consulta.Estado),

            Class = new Coding
            {
                System =
                    "http://terminology.hl7.org/CodeSystem/v3-ActCode",
                Code = "AMB",
                Display = "Ambulatory"
            },

            Subject = new ResourceReference
            {
                Identifier = new Identifier
                {
                    System =
                        "https://healthbridge.example.org/identifiers/pacientes",
                    Value = consulta.Paciente.NumeroProcesso
                }
            },

            Period = new Period
            {
                Start = consulta.DataInicio.ToString("o"),
                End = consulta.DataFim?.ToString("o")
            }
        };
    }

    private static Encounter.EncounterStatus MapearEstado(
        string estado)
    {
        return estado.Trim().ToLowerInvariant() switch
        {
            "planeada" => Encounter.EncounterStatus.Planned,
            "em curso" => Encounter.EncounterStatus.InProgress,
            "concluída" => Encounter.EncounterStatus.Finished,
            "cancelada" => Encounter.EncounterStatus.Cancelled,
            _ => Encounter.EncounterStatus.Unknown
        };
    }
}
