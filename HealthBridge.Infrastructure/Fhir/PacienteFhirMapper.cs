
using HealthBridge.Domain.Entities;
using Hl7.Fhir.Model;

namespace HealthBridge.Infrastructure.Fhir;

public class PacienteFhirMapper
{
    public Patient Mapear(Paciente paciente)
    {
        ArgumentNullException.ThrowIfNull(paciente);

        var nomes = paciente.NomeCompleto
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var patient = new Patient
        {
            Identifier =
            [
                new Identifier
                {
                    System = "https://healthbridge.example.org/identifiers/pacientes",
                    Value = paciente.NumeroProcesso
                }
            ],

            Name =
            [
                new HumanName
                {
                    Use = HumanName.NameUse.Official,
                    Text = paciente.NomeCompleto,
                    Given = nomes.Length > 0
                        ? [nomes[0]]
                        : [],
                    Family = nomes.Length > 1
                        ? nomes[^1]
                        : null
                }
            ],

            BirthDate = paciente.DataNascimento
                .ToString("yyyy-MM-dd"),

            Gender = MapearSexo(paciente.Sexo),

            Active = true
        };

        return patient;
    }

    private static AdministrativeGender MapearSexo(string sexo)
    {
        return sexo.Trim().ToLowerInvariant() switch
        {
            "masculino" => AdministrativeGender.Male,
            "feminino" => AdministrativeGender.Female,
            "outro" => AdministrativeGender.Other,
            _ => AdministrativeGender.Unknown
        };
    }
}
