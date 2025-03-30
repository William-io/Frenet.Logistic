# Estrutura da Camada
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

