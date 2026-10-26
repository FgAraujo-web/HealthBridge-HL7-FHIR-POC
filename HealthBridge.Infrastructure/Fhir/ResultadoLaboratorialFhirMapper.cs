
using HealthBridge.Domain.Entities;
using Hl7.Fhir.Model;

namespace HealthBridge.Infrastructure.Fhir;

public class ResultadoLaboratorialFhirMapper
{
    public Observation Mapear(
        ResultadoLaboratorial resultado)
    {
        ArgumentNullException.ThrowIfNull(resultado);

        if (resultado.Paciente is null)
        {
            throw new ArgumentException(
                "O resultado laboratorial deve incluir o paciente associado.");
        }

        return new Observation
        {
            Status = MapearEstado(resultado.Estado),

            Category =
            [
                new CodeableConcept
                {
                    Coding =
                    [
                        new Coding
                        {
                            System =
                                "http://terminology.hl7.org/CodeSystem/observation-category",
                            Code = "laboratory",
                            Display = "Laboratory"
                        }
                    ]
                }
            ],

            Code = new CodeableConcept
            {
                Text = resultado.NomeAnalise,
                Coding =
                [
                    new Coding
                    {
                        System =
                            "https://healthbridge.example.org/codigos/analises",
                        Code = resultado.CodigoAnalise,
                        Display = resultado.NomeAnalise
                    }
                ]
            },

            Subject = new ResourceReference
            {
                Identifier = new Identifier
                {
                    System =
                        "https://healthbridge.example.org/identifiers/pacientes",
                    Value = resultado.Paciente.NumeroProcesso
                }
            },

            Effective = new FhirDateTime(
                resultado.DataColheita.ToString("o")),

            Value = new Quantity
            {
                Value = resultado.Valor,
                Unit = resultado.Unidade
            }
        };
    }

    private static ObservationStatus MapearEstado(
        string estado)
    {
        return estado.Trim().ToLowerInvariant() switch
        {
            "preliminar" => ObservationStatus.Preliminary,
            "final" => ObservationStatus.Final,
            "corrigido" => ObservationStatus.Corrected,
            "cancelado" => ObservationStatus.Cancelled,
            _ => ObservationStatus.Unknown
        };
    }
}
