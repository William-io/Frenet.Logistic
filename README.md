# Guia de Execução do Projeto

## 1. Configuração Inicial do Banco de Dados

Para atualizar o banco de dados utilizando as migrations existentes, execute os seguintes comandos:

```bash
# Navegar até o diretório do projeto
cd seu-projeto

# Executar as migrations existentes
dotnet ef database update
```

## 2. Autenticação e Obtenção de Token

### 2.1. Registrar uma Nova Conta

Para criar uma nova conta, faça uma requisição POST para o endpoint:

```http
POST /api/v1/Customers/register
```

Exemplo de payload:
```json
{
  "email": "string",
  "firstName": "string",
  "lastName": "string",
  "phone": "string",
  "password": "string",
  "address": {
    "country": "string",
    "state": "string",
    "zipCode": "string",
    "city": "string",
    "street": "string"
  }
}
```

### 2.2. Efetuar Login

Após registrar sua conta, faça login para obter o token de autenticação. Use o endpoint:

```http
POST /api/v1/Customers/login
```

Exemplo de payload:
```json
{
    "email": "seu.email@exemplo.com"
}
```

Resposta esperada:
```json
{
    "token": "seu_token_jwt_aqui"
}
```

## 3. Utilizando o Token

Para fazer requisições autenticadas, inclua o token no header da requisição:

```http
Bearer (apiKey) PARA SWAGGER!!
Digite 'Bearer' [espaço] e seu token. Exemplo: 'Bearer abcdef123456'
//Dentro do campo Value:
```

## Observações Importantes

1. O token é gerado automaticamente após um login bem-sucedido
2. Mantenha seu token seguro e não o compartilhe
3. Utilize sempre HTTPS para suas requisições
4. Certifique-se de que todas as migrations foram executadas antes de iniciar o uso do sistema

## Visualizando Token:
há inseridas as permissões em seu Token: [ReadMember] / [UpdateMember]
> https://jwt.io/

<details>
  <summary>Especificações das Camadas da Aplicação</summary>

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

# Frenet.Logistic.Domain

A camada de domínio do projeto Frenet.Logistic implementa os conceitos centrais do negócio e contém as regras mais importantes da aplicação, seguindo os princípios da Arquitetura Limpa (Clean Architecture) e do Domain-Driven Design (DDD), Dominio Rico!

```bash
Frenet.Logistic.Domain/
├── Abstractions/           # Classes e interfaces base
│   ├── Entity.cs           # Classe base para entidades
│   ├── Error.cs            # Representação de erros de domínio
│   ├── IDomainEvent.cs     # Interface para eventos de domínio
│   ├── IUnitOfWork.cs      # Abstração para transações atômicas
│   └── Result.cs           # Padrão para encapsulamento de resultados
├── Customers/              # Agregado de Cliente
│   ├── Address.cs          # Objeto de valor para endereço
│   ├── Customer.cs         # Entidade raiz do agregado
│   ├── CustomerErrors.cs   # Erros específicos de cliente
│   ├── Email.cs            # Objeto de valor para email
│   ├── FirstName.cs        # Objeto de valor para nome
│   ├── ICustomerRepository.cs  # Interface de repositório
│   ├── LastName.cs         # Objeto de valor para sobrenome
│   ├── Permission.cs       # Objeto de valor para permissões
│   ├── Phone.cs            # Objeto de valor para telefone
│   ├── Role.cs             # Entidade para função/papel
│   └── RolePermission.cs   # Relação entre função e permissão
├── Dispatchs/              # Agregado de Despacho
│   ├── Dispatch.cs         # Entidade raiz do agregado
│   ├── IDispatchRepository.cs  # Interface de repositório
│   └── PackageParams.cs    # Objeto de valor para parâmetros do pacote
├── Enums/                  # Enumerações do sistema
│   └── Permission.cs       # Enumeração de permissões
├── Orders/                 # Agregado de Pedido
│   ├── IOrderRepository.cs # Interface de repositório
│   ├── Order.cs            # Entidade raiz do agregado
│   ├── OrderErrors.cs      # Erros específicos de pedido
│   └── ShippingPriceService.cs  # Serviço de cálculo de frete
└── Shared/                 # Componentes compartilhados
    └── Enumeration.cs      # Implementação de Smart Enum
```

<details>
  <summary>Componentes Principais</summary>

## Abstrações Fundamentais

### Entity
A classe abstrata `Entity` serve como base para todas as entidades do domínio, fornecendo:
- Identificação única através de um GUID
- Sistema de eventos de domínio integrado
- Métodos para adicionar, listar e limpar eventos

### Error
A classe `Error` representa erros de domínio de forma estruturada e consistente:
- Código de erro para identificação única
- Mensagem de erro descritiva
- Métodos para comparação e erros comuns pré-definidos

### Result Pattern
O tipo `Result<T>` implementa o padrão de resultado para encapsular o resultado de operações de domínio:
- Representa claramente sucesso ou falha
- Carrega um valor em caso de sucesso
- Carrega um erro estruturado em caso de falha
- Evita o uso excessivo de exceções para controle de fluxo

### Eventos de Domínio
A interface `IDomainEvent` define a estrutura para eventos de domínio, que permitem:
- Comunicação assíncrona entre agregados
- Desacoplamento entre componentes do sistema
- Extensibilidade para aspectos como auditoria e notificações

## Agregados e Entidades

### Customer (Cliente)
O agregado `Customer` representa um cliente no sistema, contendo:
- Informações pessoais (nome, sobrenome, email, telefone)
- Endereço completo
- Hash de senha para autenticação
- Métodos para atualização de dados

Objetos de valor associados:
- **FirstName**: Validação e formatação do nome
- **LastName**: Validação e formatação do sobrenome
- **Email**: Validação de formato e unicidade
- **Phone**: Validação e formatação de número telefônico
- **Address**: Estrutura completa de endereço

### Order (Pedido)
O agregado `Order` representa um pedido de envio no sistema:
- Referência ao cliente e ao despacho associados
- Valor monetário do pedido
- Status atual (processando, enviado, entregue, cancelado)
- Datas de criação, envio e entrega
- Métodos de transição de estado com validações de negócio

O ciclo de vida de um pedido segue regras estritas:
- Um pedido começa no status "Processando"
- Pode ser confirmado, passando para "Enviado"
- Pode ser completado, passando para "Entregue"
- Pode ser cancelado, se ainda não estiver entregue
- Transições inválidas resultam em erros de domínio

### Dispatch (Despacho)
O agregado `Dispatch` representa um despacho de pacote:
- Parâmetros físicos do pacote (altura, largura, comprimento, peso)
- Data do último despacho (opcional)
- Métodos para atualização de parâmetros ou data

## Objetos de Valor

### PackageParams
Encapsula os parâmetros físicos de um pacote:
- Altura (em centímetros)
- Largura (em centímetros)
- Comprimento (em centímetros)
- Peso (em quilogramas)
- Validações para garantir valores positivos

### Address
Representa um endereço completo e válido:
- Rua/logradouro
- Cidade
- Estado
- CEP/Código postal
- País
- Validações para garantir campos não vazios

## Serviços de Domínio

### ShippingPriceService
Serviço responsável pelo cálculo de preços de frete:
- Integra-se com serviço externo para obter cotações
- Recebe parâmetros do pacote e endereços de origem/destino
- Retorna valor monetário do frete, com tratamento de erros
- Uso da api externa: https://melhorenvio.com.br/

## Interfaces de Repositório
Cada agregado define sua própria interface de repositório:

### ICustomerRepository
- Busca por ID ou email
- Adiciona novos clientes
- Atualiza clientes existentes

### IOrderRepository
- Busca por ID
- Lista pedidos por cliente
- Adiciona e atualiza pedidos

### IDispatchRepository
- Busca por ID
- Lista todos os despachos disponíveis
- Adiciona e atualiza despachos

## Smart Enum
O padrão Smart Enum é implementado para enumerações ricas:
- Permite valores enumerados com comportamento e propriedades adicionais
- Suporta busca por ID ou nome
- Facilita a listagem de todos os valores possíveis

## Classes de Erro
O domínio define erros específicos para cada contexto:

### OrderErrors
- **NotFound**: Pedido não encontrado
- **NotProcessing**: Pedido não está em processamento
- **NotShipped**: Pedido não foi enviado
- **AlreadyDelivered**: Pedido já foi entregue
- **AlreadyCancelled**: Pedido já foi cancelado
- **Cancelled**: Pedido foi cancelado

### CustomerErrors
- **NotFound**: Cliente não encontrado
- **DuplicateEmail**: Email já cadastrado
- **InvalidCredentials**: Credenciais inválidas

## Princípios e Padrões Aplicados
A camada de domínio implementa diversos princípios e padrões:
- **Entidades Ricas**: Encapsulam comportamento e regras, não apenas dados
- **Objetos de Valor Imutáveis**: Representam conceitos sem identidade própria
- **Agregados**: Definem limites de consistência transacional
- **Factory Methods**: Encapsulam a criação de entidades complexas
- **Especificações**: Encapsulam regras de validação ou seleção
- **Result Pattern**: Tratamento explícito de erros sem exceções
- **Eventos de Domínio**: Comunicação desacoplada entre agregados

# Fluxos de Negócio Principais

## Processamento de Pedido
1. Cliente solicita um envio
2. Sistema calcula preço de frete usando o serviço de preços
3. Cria um novo pedido no estado "Processing"
4. Emite evento de domínio para notificação

## Confirmação de Pedido
1. Pedido é confirmado e passa para o estado "Shipped"
2. A data de envio é registrada
3. Evento de domínio sinaliza a mudança de status

## Entrega de Pedido
1. Pedido é marcado como entregue ("Delivered")
2. A data de entrega é registrada
3. Evento de domínio registra a conclusão do pedido

## Cancelamento de Pedido
1. Pedido é cancelado (se ainda não entregue)
2. Status muda para "Cancelled"
3. Evento de domínio notifica sobre o cancelamento
</details>

# Estrutura do Projeto Frenet.Logistic.Infrastructure

## Diretórios e Arquivos

```
Frenet.Logistic.Infrastructure/
├── Authentication/              # Implementação de autenticação e autorização
│   ├── CustomClaims.cs          # Claims personalizadas para JWT
│   ├── HasPermission.cs         # Atributo para verificação de permissões
│   ├── IPermissionService.cs    # Interface do serviço de permissões
│   ├── JwtBearerOptionsSetup.cs # Configuração do JWT Bearer
│   ├── JwtOptions.cs            # Opções de configuração do JWT
│   ├── JwtOptionsSetup.cs       # Setup das opções do JWT
│   ├── JwtProvider.cs           # Provedor de tokens JWT
│   ├── Permission.cs            # Enumeração de permissões
│   ├── PermissionAuthorizationHandler.cs # Handler para autorização baseada em permissões
│   ├── PermissionAuthorizationPolicyProvider.cs # Provedor de políticas de autorização
│   ├── PermissionRequirement.cs # Requisito de permissão para autorização
│   └── PermissionService.cs     # Serviço de gerenciamento de permissões
├── Clock/                      # Implementações relacionadas a tempo/data
│   └── DateTimeProvider.cs     # Provedor de data/hora do sistema
├── Constants/                  # Constantes da aplicação
│   └── Tables.cs               # Nomes de tabelas do banco de dados
├── Data/                       # Componentes de acesso a dados
│   ├── DateHandler.cs          # Manipulador de datas para Dapper
│   └── SqlConnectionFactory.cs # Fábrica de conexões SQL
├── Email/                      # Implementações de serviços de email
├── Migrations/                 # Migrações do Entity Framework Core
│   ├── 20250330002153_Initial.cs # Migração inicial do banco de dados
│   └── ContextModelSnapshot.cs   # Snapshot do modelo de banco de dados
├── Repositories/               # Implementações dos repositórios
│   ├── CustomerRepository.cs   # Repositório de clientes
│   ├── DispatchRepository.cs   # Repositório de despachos
│   ├── OrderRepository.cs      # Repositório de pedidos
│   └── Repository.cs           # Classe base para repositórios
├── Settings/                   # Configurações do Entity Framework Core
│   └── OrderSetting.cs         # Configuração da entidade Order
├── Context.cs                  # Contexto do Entity Framework Core
├── DependencyInjection.cs      # Configuração da injeção de dependências
└── Frenet.Logistic.Infrastructure.csproj # Arquivo de projeto
```

<details>
  <summary>Descrição das Funcionalidades</summary>
  
### Authentication

Implementa toda a lógica de autenticação e autorização usando JWT (JSON Web Tokens):

- **JwtProvider**: Gera tokens JWT para usuários autenticados
- **Permission**: Define níveis de permissão como ReadMember, UpdateMember
- **PermissionService**: Gerencia a verificação de permissões de usuários
- **Authorization Handlers**: Configuram políticas de autorização baseadas em permissões

### Clock

Contém abstrações relacionadas a tempo e data:

- **DateTimeProvider**: Implementação para fornecer data e hora do sistema

### Constants

Armazena constantes utilizadas pela aplicação:

- **Tables**: Nomes das tabelas utilizadas no banco de dados

### Data

Contém componentes para acesso a dados:

- **SqlConnectionFactory**: Cria conexões com o banco de dados SQL Server
- **DateHandler**: Manipula conversões de datas para o Dapper

### Migrations

Migrações do Entity Framework Core para criar e atualizar o banco de dados:

- **Initial**: Migração inicial que cria todas as tabelas do sistema
  - Inclui setup para tabelas de Customer, Dispatch, Order, Role, Permission

### Repositories

Implementa os repositórios definidos na camada de domínio:

- **Repository**: Base genérica para todos os repositórios
- **CustomerRepository**: Operações específicas para clientes
- **DispatchRepository**: Operações específicas para despachos
- **OrderRepository**: Operações específicas para pedidos

### Settings

Contém configurações de mapeamento do Entity Framework Core:

- **OrderSetting**: Configuração do mapeamento entidade-tabela para Order

### Context.cs

Contexto principal do Entity Framework Core que:

- Implementa IUnitOfWork para garantir transações atômicas
- Configura o mapeamento entre entidades e tabelas
- Gerencia a publicação de eventos de domínio

### DependencyInjection.cs

Configura a injeção de dependências para todos os serviços da camada de infraestrutura:

- Registra repositórios
- Configura autenticação JWT
- Configura acesso a dados
- Registra serviços de sistema
</details>

</details>
