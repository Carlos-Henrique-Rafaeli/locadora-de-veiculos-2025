# Locadora de Veículos

[![Stack](https://skillicons.dev/icons?i=dotnet,cs,angular,nodejs,bootstrap,html,scss,typescript,azure&perline=9)](https://skillicons.dev)

## Projeto

Desenvolvido durante o curso Full-Stack da [Academia do Programador](https://www.academiadoprogramador.net) 2025

## Descrição

Projeto Full-Stack sobre uma locadora de veículos. Permite cadastrar veículos, clientes e condutores, controlar aluguéis, planos, taxas e valores, além de acompanhar tudo que está em andamento de forma prática e organizada.

## Funcionalidades

### Cliente
- Tipo de Cliente (Pessoa Física ou Jurídica)
- Nome
- Telefone
- CPF ou CNPJ dependendo do tipo
- Estado
- Cidade
- Bairro
- Rua
- Numero

### Condutor
- Cliente
- Cliente condutor
- Nome
- CPF
- CNH
- Validade CNH
- Telefone

### Grupo de Veículo
- Nome

### Veículo
- Grupo de Veículo
- Placa (Antiga ou Mercosul)
- Marca
- Modelo
- Cor
- Tipo de Combustível (Gasolina, Etanol, Diesel)
- Capacidade do Tanque
- Imagem (Opicional)

### Plano de Cobrança
- Grupo de Veículo
- Tipo do Plano:
  - Plano Diário:
    -  Valor Diário
    -  Valor por Km
  - Plano Controlado:
    - Valor Diário
    - Km Incluso
    - Valor por Km excedente
  - Plano Livre:
    - Valor Fixo

### Taxa/Serviço
- Nome
- Valor
- Tipo de Cobrança (Valor Fixo ou Cobrança Diária)

### Aluguel
- Condutor
- Grupo de Veículo
- Veículo
- Data de Entrada
- Data de Retorno
- Plano de Cobrança
- Taxas e Serviços (Opicional)

### Preço Combustível
- Gasolina
- Diesel
- Etanol

### Funcionários (Apenas Empresa)
- Nome Completo
- CPF
- Email
- Salário
- Data de Admissão

## Requisitos para Execução do Projeto Completo

- .NET SDK (recomendado .NET 8.0 ou superior) para compilação e execução do projeto back-end.
- Node.js v20+

## Configuração de Variáveis de Ambiente (Desenvolvimento)

O funcionamento da aplicação depende que variáveis de ambiente sejam configuradas.

Utilize o sistema de gerenciamento de segredos de usuário do dotnet (dotnet user secrets) no projeto **LocadoraDeVeiculos.WebApi**. [Saiba mais sobre configuração de ambiente na documentação da Microsoft](https://learn.microsoft.com/pt-br/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=windows).

```json
{
  "SQL_CONNECTION_STRING": "{substitua-pelo-segredo}",
  "NEWRELIC_LICENSE_KEY": "{substitua-pelo-segredo}",
  "LUCKYPENNY_LICENSE_KEY": "{substitua-pelo-segredo}"
  "JWT_GENERATION_KEY": "{substitua-pelo-segredo}",
  "JWT_AUDIENCE_DOMAIN": "http://localhost:4200",
  "CORS_ALLOWED_ORIGINS": "http://localhost:4200"
}
```

## Executando o Back-End

Vá para a pasta do projeto da WebAPI:

```bash
cd server/LocadoraDeVeiculos.WebApi
```

Execute o projeto:

```bash
dotnet run
```

A API poderá ser acessada no endereço `https://localhost:7050/api`.

A documentação **OpenAPI** também estará disponível em: `https://localhost:7050/swagger`.

## Executando o Front-End

Vá para a pasta do projeto Angular:

```bash
cd client
```

Instale as dependências:

```bash
npm install
```

Execute o projeto:

```bash
npm start
```

A aplicação Angular estárá disponível no endereço `http://localhost:4200`.
