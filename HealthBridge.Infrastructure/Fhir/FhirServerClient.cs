
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.Extensions.Configuration;

namespace HealthBridge.Infrastructure.Fhir;

public class FhirServerClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public FhirServerClient(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;

        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(
                "application/fhir+json"));
    }

    public async Task<string> ObterCapacidadesAsync(
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(
            "metadata",
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(
            cancellationToken);
    }


    public async Task<string?> ObterPacienteAsync(
        string idFhir,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(idFhir))
        {
            throw new ArgumentException(
                "O identificador FHIR é obrigatório.",
                nameof(idFhir));
        }

        // Validar o formato do identificador FHIR
        if (idFhir.Length > 64 ||
            !System.Text.RegularExpressions.Regex.IsMatch(
                idFhir,
                @"^[A-Za-z0-9\-\.]+$"))
        {
            throw new ArgumentException(
                "O identificador FHIR tem um formato inválido.",
                nameof(idFhir));
        }

        using var response = await _httpClient.GetAsync(
            $"Patient/{idFhir}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound ||
            response.StatusCode == HttpStatusCode.Gone)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(
            cancellationToken);
    }


    public async Task<string> PesquisarPacientePorProcessoAsync(
        string numeroProcesso,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroProcesso))
        {
            throw new ArgumentException(
                "O número de processo é obrigatório.",
                nameof(numeroProcesso));
        }

        const string sistemaIdentificacao =
            "https://healthbridge.example.org/identifiers/pacientes";

        var identificador =
            $"{sistemaIdentificacao}|{numeroProcesso.Trim()}";

        var url =
            $"Patient?identifier={Uri.EscapeDataString(identificador)}";

        using var response = await _httpClient.GetAsync(
            url,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(
            cancellationToken);
    }



    public async Task<string> PublicarPacienteAsync(
        Patient paciente,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(paciente);

        // Bloquear escrita quando não estiver autorizada
        var permitirEscrita = bool.TryParse(
     _configuration["FhirServer:AllowWriteOperations"],
     out var valor) && valor;

        if (!permitirEscrita)
        {
            throw new InvalidOperationException(
                "As operações de escrita FHIR estão desativadas.");
        }

        // Obter o identificador clínico
        var identificador = paciente.Identifier?
            .FirstOrDefault(i =>
                !string.IsNullOrWhiteSpace(i.System) &&
                !string.IsNullOrWhiteSpace(i.Value));

        if (identificador is null)
        {
            throw new ArgumentException(
                "O paciente deve possuir um identificador clínico.");
        }

        // Serializar o recurso FHIR
        var serializer = new FhirJsonSerializer();

        var json = serializer.SerializeToString(paciente);

        using var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/fhir+json");

        // Criação condicional para evitar duplicações
        var token = $"{identificador.System}|{identificador.Value}";

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "Patient");

        request.Content = content;

        request.Headers.TryAddWithoutValidation(
            "If-None-Exist",
            $"identifier={Uri.EscapeDataString(token)}");

        using var response = await _httpClient.SendAsync(
            request,
            cancellationToken);

        if (response.StatusCode != HttpStatusCode.Created &&
            response.StatusCode != HttpStatusCode.OK)
        {
            var detalhe = await response.Content.ReadAsStringAsync(
                cancellationToken);

            throw new HttpRequestException(
                $"Erro FHIR: {(int)response.StatusCode} - {detalhe}");
        }

        return await response.Content.ReadAsStringAsync(
            cancellationToken);
    }
}
