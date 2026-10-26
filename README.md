
# HealthBridge — POC de Interoperabilidade HL7 FHIR R4

## 1. Visão geral

O HealthBridge é uma prova de conceito (POC) de interoperabilidade
em sistemas de informação de saúde.

O projeto demonstra a transformação de dados clínicos armazenados
numa base de dados relacional em recursos HL7 FHIR R4, utilizando
uma API REST desenvolvida em ASP.NET Core.

A solução utiliza exclusivamente dados fictícios para demonstração.

## 2. Objetivos

- Explorar o standard HL7 FHIR R4.
- Modelar dados clínicos numa base de dados relacional.
- Implementar uma arquitetura modular em .NET 10.
- Converter entidades de domínio em recursos FHIR.
- Integrar uma API local com um servidor FHIR externo.
- Validar operações de consulta e publicação controlada.
- Aplicar mecanismos básicos de proteção contra escritas acidentais.

## 3. Tecnologias utilizadas

- C# / .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server LocalDB
- HL7 FHIR R4
- Firely .NET SDK
- HAPI FHIR — servidor público de testes
- Visual Studio 2026

## 4. Arquitetura

A solução está organizada em quatro projetos:

HealthBridge.Domain
  Entidades de domínio:
  - Paciente
  - Consulta
  - ResultadoLaboratorial

HealthBridge.Application
  Serviços de aplicação e contratos de repositórios.

HealthBridge.Infrastructure
  Persistência com Entity Framework Core.
  Implementação dos repositórios.
  Mapeadores HL7 FHIR.
  Cliente HTTP para integração com o servidor FHIR.

HealthBridge.Api
  Controladores REST.
  Configuração e injeção de dependências.
  Endpoints de consulta, diagnóstico, publicação e testes.

## 5. Recursos FHIR implementados

### Patient

Representa os dados de identificação de um paciente.

Mapeamento:
- NumeroProcesso → identifier
- NomeCompleto → name
- DataNascimento → birthDate
- Sexo → gender

### Encounter

Representa um episódio de prestação de cuidados.

Mapeamento:
- NumeroConsulta → identifier
- Estado → status
- DataInicio → period.start
- DataFim → period.end
- Paciente.NumeroProcesso → subject.identifier

### Observation

Representa um resultado laboratorial.

Mapeamento:
- CodigoAnalise → code.coding.code
- NomeAnalise → code.text
- Estado → status
- DataColheita → effectiveDateTime
- Valor → valueQuantity.value
- Unidade → valueQuantity.unit
- Paciente.NumeroProcesso → subject.identifier

Os códigos de análises utilizados nesta POC pertencem
a um sistema fictício próprio do HealthBridge.

## 6. Integração com HAPI FHIR

Servidor público de testes:

https://hapi.fhir.org/baseR4/

A integração utiliza pedidos HTTP e recursos FHIR em JSON.

Funcionalidades implementadas:
- Consulta das capacidades do servidor.
- Consulta de pacientes por identificador FHIR.
- Pesquisa de pacientes por número de processo.
- Publicação controlada de pacientes fictícios.

A publicação utiliza criação condicional através do cabeçalho
If-None-Exist, para reduzir a criação de duplicados.

## 7. Endpoints principais

GET /api/pacientes

GET /api/fhir/pacientes/{numeroProcesso}

GET /api/fhir/diagnostico/capacidades

GET /api/fhir/consulta/pacientes/{idFhir}

GET /api/fhir/consulta/pacientes/processo/{numeroProcesso}

POST /api/fhir/publicacao/pacientes/{numeroProcesso}

Endpoints locais de demonstração:

GET /api/fhir/testes/encounter

GET /api/fhir/testes/observation

## 8. Segurança

As operações de escrita no servidor FHIR externo
estão desativadas por predefinição:

"AllowWriteOperations": false

A publicação implementada está limitada ao identificador
de demonstração HBTEST001.

A solução utiliza dados fictícios e não deve ser utilizada
com dados clínicos reais.

Os mecanismos implementados não substituem autenticação,
autorização, auditoria e restantes controlos exigidos
num ambiente de produção.

## 9. Testes realizados

Foram executados testes funcionais aos seguintes cenários:

- Consulta de pacientes na API local.
- Conversão de pacientes para recursos FHIR Patient.
- Consulta das capacidades do servidor HAPI FHIR.
- Publicação controlada de um paciente fictício.
- Consulta do recurso Patient publicado.
- Pesquisa FHIR por identificador de paciente.
- Conversão de consultas para Encounter.
- Conversão de resultados laboratoriais para Observation.
- Bloqueio de publicação com HTTP 403.

Os testes foram realizados com dados sintéticos.

## 10. Limitações e evolução futura

Esta POC tem caráter experimental e não constitui
um sistema de informação clínica de produção.

Possíveis evoluções:
- Autenticação e autorização com OAuth 2.0.
- Validação de recursos contra perfis FHIR.
- Integração de terminologias LOINC, SNOMED CT e UCUM.
- Testes automatizados de integração.
- Auditoria e rastreabilidade das operações.
- Gestão de erros, resiliência e observabilidade.
- Integração com sistemas clínicos reais, em ambiente autorizado.

## 11. Conclusão

O HealthBridge demonstra a aplicação de conceitos
de interoperabilidade em sistemas de informação de saúde,
integrando modelação relacional, arquitetura de software,
APIs REST e transformação de dados para HL7 FHIR R4.

O projeto foi desenvolvido como exercício prático
de análise técnico-funcional, integração de sistemas
e engenharia de software.

---

POC desenvolvida por Flórida Araújo.
