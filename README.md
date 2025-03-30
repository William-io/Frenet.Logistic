# Frenet.Logistic.Api
A camada de API está organizada da seguinte forma:

```bash
Frenet.Logistic.API/
├── Controllers/                # Controladores REST organizados por domínio
│   ├── Customers/              # Endpoints relacionados a clientes
│   ├── Dispatches/             # Endpoints relacionados a despachos
│   └── Orders/                 # Endpoints relacionados a pedidos
├── Extensions/                 # Classes de extensão para configuração da aplicação
│   ├── ApplicationBuilder.cs   # Configurações do pipeline da aplicação
│   └── Seeding.cs              # Dados de inicialização para o ambiente de desenvolvimento
├── Middleware/                 # Middleware personalizado para processamento de requisições
│   ├── ExceptionHandlingMiddleware.cs    # Tratamento global de exceções
│   └── RequestContextLoggingMiddleware.cs  # Logging de contexto de requisições
├── OpenApi/                    # Configurações para documentação da API
│   └── SettingsSwagger.cs      # Configuração do Swagger/OpenAPI
├── Properties/                 # Configurações específicas da aplicação
├── Program.cs                  # Ponto de entrada da aplicação
└── appsettings.json            # Arquivo de configuração
```

<details>
  <summary>Funcionalidades Principais | Frenet.Logistic.API</summary>
  
  ## Middleware de Tratamento de Exceções
  O `ExceptionHandlingMiddleware` intercepta exceções lançadas durante o processamento de requisições e as transforma em respostas HTTP estruturadas:
  
  - Exceções de validação são transformadas em respostas **400 Bad Request**
  - Exceções não tratadas são transformadas em respostas **500 Internal Server Error**
  - Todas as exceções são registradas para fins de diagnóstico
  - As mensagens de erro são formatadas como um objeto `ProblemDetails` padrão
  
  ## Documentação OpenAPI
  A API é documentada usando o padrão OpenAPI através da configuração em `SettingsSwagger`:
  
  - Suporte a versionamento de API
  - Informações detalhadas sobre endpoints, parâmetros e respostas
  - Configuração de autenticação JWT para endpoints protegidos
  - Inclusão de comentários XML dos arquivos de código
  
  ## Segurança e Autenticação
  A API utiliza autenticação JWT (JSON Web Tokens) para proteger os endpoints:
  
  - Configuração de Bearer Token através do middleware de autenticação
  - Esquema de segurança documentado no Swagger para testes interativos
  - Suporte a autorização baseada em permissões
  
  ## Inicialização de Dados
  O `Seeding` fornece dados de exemplo para o ambiente de desenvolvimento:
  
  - Geração de dados realistas usando a biblioteca Bogus
  - Populamento inicial das tabelas de despacho com dimensões e pesos aleatórios
  - Inserção direta no banco de dados usando Dapper para melhor performance
  
  ## Configuração da Aplicação
  O `ApplicationBuilder` oferece métodos de extensão para configurar o pipeline de requisições HTTP:
  
  - `ApplyMigrations()`: Aplica migrações pendentes ao banco de dados
  - `UseCustomExceptionHandler()`: Adiciona o middleware de tratamento de exceções
  - Comentado: `UseRequestContextLogging()`: Para logging de contexto das requisições
  
  ## Versionamento da API
  A API suporta versionamento através de segmentos de URL (`v1`, `v2`, etc.):
  
  - Configuração centralizada do versionamento
  - Documentação diferenciada para cada versão
  - Indicação de versões obsoletas na documentação
</details>

# Frenet.Logistic.Application

A camada de Aplicação do projeto Frenet.Logistic atua como intermediária entre a camada de API e o Domínio, implementando os casos de uso da aplicação. Esta camada segue os princípios de Clean Architecture e utiliza o padrão CQRS (Command Query Responsibility Segregation) através do MediatR para separar operações de leitura e escrita.

```bash
Frenet.Logistic.Application/
├── Abstractions/              # Interfaces e abstrações
│   ├── Behaviors/             # Comportamentos do pipeline do MediatR
│   ├── Clock/                 # Abstrações relacionadas a tempo
│   ├── DataFactory/           # Factory para acesso a dados
│   ├── Email/                 # Serviços de e-mail
│   ├── Messaging/             # Interfaces para CQRS
├── Authentication/            # Serviços de autenticação
│   ├── IJwtProvider.cs        # Interface para geração de tokens JWT
├── Customers/                 # Casos de uso relacionados a clientes
│   ├── GetCustomerById/       # Consulta para obter cliente por ID
│   ├── LoginCustomer/         # Comando para autenticação de cliente
│   ├── RegisterCustomer/      # Comando para registro de cliente
├── Dispatchs/                 # Casos de uso relacionados a despachos
│   ├── GetAllDispatchs/       # Consulta para obter todos os despachos
│   ├── SearchDispatchs/       # Consulta para buscar despachos por critérios
├── Exceptions/                # Exceções específicas da aplicação
├── Extensions/                # Classes de extensão
├── Orders/                    # Casos de uso relacionados a pedidos
│   ├── CancelOrder/           # Comando para cancelar pedido
│   └── ...                    # Outros comandos e consultas
└── DependencyInjection.cs     # Configuração de injeção de dependência
```

<details>
    <summary>Componentes Principais</summary>

## Padrão CQRS
O projeto implementa o padrão CQRS através dos seguintes componentes:

- **Comandos**: Operações que modificam o estado do sistema, retornando `Result<T>` ou `Result`
- **Consultas**: Operações somente leitura que retornam dados, retornando `Result<T>`
- **Manipuladores**: Classes responsáveis por processar comandos e consultas

Exemplo de um comando:
```csharp
public sealed record RegisterCustomerCommand(
    string Email, 
    string FirstName,
    string LastName,
    string Phone,
    string Password,
    Address Address) : ICommand<Guid>;
```

Exemplo de uma consulta:
```csharp
public sealed record GetAllDispatchsQuery() : IQuery<IReadOnlyList<GetAllDispatchsResponse>>;
```

## Pipeline de Comportamentos

O projeto utiliza o conceito de Behaviors do MediatR para implementar funcionalidades transversais:

- **ValidationBehavior**: Valida comandos e consultas usando FluentValidation antes da execução
- **LoggingBehavior**: Registra logs de entrada e saída para cada comando e consulta

Estes comportamentos são registrados no pipeline do MediatR no arquivo `DependencyInjection.cs`:

```csharp
services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
    configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
    configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
```

## Validação de Dados

A validação é implementada usando FluentValidation e integrada através do `ValidationBehavior`:

- O `ValidationBehavior` intercepta as requisições.
- Executa todas as validações aplicáveis ao tipo de requisição.
- Se encontrar erros, lança uma `ValidationException` com a lista de erros.
- A exceção é capturada na camada de API e transformada em uma resposta HTTP 400.

## Autenticação

A camada de aplicação define contratos para autenticação:

- **IJwtProvider**: Interface para geração de tokens JWT.
- **LoginCustomerCommand/Handler**: Implementa a lógica de autenticação.

## DTOs e Mapeamentos

A camada define DTOs (Data Transfer Objects) para comunicação entre as camadas:

- **Command/Query**: Dados de entrada para os casos de uso.
- **Response**: Dados de saída retornados para a camada de API.

### Exemplo:
```csharp
public sealed class DispatchResponse
{
    public Guid Id { get; init; }
    public PackageParamsResponse Package { get; set; }
}
```

## Tratamento de Erros e Resultados

A camada utiliza o tipo `Result<T>` do domínio para encapsular resultados e erros:

- **Sucesso**: `Result.Success(value)`
- **Falha**: `Result.Failure<T>(error)`

Este padrão permite o tratamento de erros de forma elegante sem uso excessivo de exceções.

## Casos de Uso Principais

### Clientes
- **RegisterCustomer**: Registra um novo cliente no sistema
- **LoginCustomer**: Autentica um cliente e retorna um token JWT
- **GetCustomerById**: Obtém detalhes de um cliente por ID

### Despachos
- **GetAllDispatchs**: Lista todos os despachos disponíveis
- **SearchDispatchs**: Busca despachos com base em critérios específicos

### Pedidos
- **CancelOrder**: Cancela um pedido existente
- **CompleteOrder**: Marca um pedido como concluído

### Tecnologias Utilizadas
- **MediatR**: Para implementação do padrão CQRS e Mediator
- **FluentValidation**: Para validação de dados de entrada
- **Dapper**: Para consultas SQL otimizadas em algumas operações
- **JWT**: Para autenticação baseada em tokens

### Integração com Outras Camadas
- **API**: Recebe requisições HTTP e as traduz em comandos/consultas
- **Domínio**: Contém as regras de negócio e entidades
- **Infraestrutura**: Implementa interfaces definidas na aplicação

Esta arquitetura garante uma separação clara de responsabilidades e facilita a manutenção e testabilidade do sistema.
</details>
