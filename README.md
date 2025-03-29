# Análise e Documentação do Projeto Frenet.Logistic

## Visão Geral
O Frenet.Logistic é uma API para sistemas de logística que oferece funcionalidades de rastreamento, despacho e gerenciamento de pedidos. O projeto segue a arquitetura limpa (Clean Architecture) com separação clara de responsabilidades entre as diferentes camadas da aplicação.

## Estrutura do Projeto
O projeto está organizado em quatro camadas principais:

- **Frenet.Logistic.API**: Interface da aplicação (Controllers, Middlewares, Configurações)
- **Frenet.Logistic.Application**: Lógica de aplicação (Use Cases, DTOs, Validações)
- **Frenet.Logistic.Domain**: Regras de negócio e entidades de domínio
- **Frenet.Logistic.Infrastructure**: Implementações de infraestrutura (Persistência, Serviços Externos)

## Tecnologias Utilizadas

### Framework e Runtime
- **.NET 8.0**: Framework base do projeto
- **ASP.NET Core**: Framework web para construção da API

### Persistência de Dados
- **Dapper**: Micro-ORM para acesso a dados
- **Entity Framework Core**: ORM para mapeamento objeto-relacional
- **Entity Framework Core Tools**: Ferramentas para migrations

### API e Documentação
- **ASP.NET Core API Versioning**: Para versionamento da API
- **Swagger/OpenAPI**: Para documentação da API
- **Swashbuckle**: Integração do Swagger com ASP.NET Core

### Autenticação e Autorização
- **JWT Bearer Authentication**: Implementação de autenticação baseada em tokens JWT

### Logging e Monitoramento
- **NLog**: Framework para geração de logs
- **NLog.Extensions.Logging**: Integração com sistema de logging do .NET

### Validação
- **FluentValidation**: Biblioteca para validação de objetos
- **FluentValidation.DependencyInjectionExtensions**: Extensões para injeção de dependência

### Padrões de Design e Arquitetura
- **MediatR**: Implementação do padrão Mediator para comunicação entre componentes
- **Bogus**: Biblioteca para geração de dados falsos (usada para seeding)

### Padrões de Projeto Implementados

#### Padrões Arquiteturais
- **Clean Architecture**: Separação clara entre domínio, aplicação, infraestrutura e API
- **Domain-Driven Design (DDD)**: Foco no domínio do negócio com entidades ricas e eventos

#### Padrões de Design
- **Repository Pattern**: Abstração do acesso a dados
- **Mediator Pattern**: Implementado via MediatR para desacoplamento das requisições
- **CQRS (Command Query Responsibility Segregation)**: Separação entre operações de leitura e escrita
- **Smart Enum Pattern**: Implementado através da classe `Enumeration<TEnum>` para enumerações ricas
- **Middleware Pattern**: Para tratamento de exceções e logging

#### Outros Padrões
- **Unit of Work**: Para gerenciamento de transações
- **Dependency Injection**: Uso extensivo de injeção de dependência
- **Options Pattern**: Para configuração da aplicação

## Funcionalidades Principais
- **Gerenciamento de Despachos**: Controle de envio de pacotes
- **Integração com API Externa**: Integração com MelhorEnvio para cálculo de frete
- **Tratamento Global de Exceções**: Middleware para interceptar e tratar exceções
- **Logging Estruturado**: Sistema de log para monitoramento e depuração
- **Documentação da API**: Via Swagger com suporte a versionamento
- **Autenticação JWT**: Sistema de segurança baseado em tokens

## Recursos e Características

### Tratamento de Exceções
O projeto implementa um middleware global para tratamento de exceções (`ExceptionHandlingMiddleware`), que captura exceções não tratadas e retorna respostas HTTP apropriadas.

### Logging
Utiliza NLog para logging estruturado, com configurações para saída em arquivos e console.

### Versionamento de API
Implementa versionamento de API para garantir compatibilidade e evoluções futuras.

### Seeding de Dados
Inclui mecanismo para popular o banco de dados com dados iniciais usando a biblioteca Bogus.

### Documentação da API
Documentação completa via Swagger/OpenAPI, incluindo informações de autenticação JWT.

## Considerações de Segurança
- Implementação de autenticação JWT Bearer
- HTTPS ativado para comunicação segura
- Validação de input via FluentValidation
- Tratamento adequado de exceções para prevenir vazamento de informações sensíveis
