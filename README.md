# Projeto .NET 8 com Arquitetura em Camadas

Este projeto é uma aplicação .NET 8 para gerir contratos de locações de ônibus utilizando a arquitetura em camadas, seguindo boas práticas e padrões de mercado como Clean Architecture e Clean Code. Atualmente, o sistema está em processo de migração, com planos para incorporar padrões adicionais como Specification Pattern e Generic Repository.

## Estrutura do Projeto

O projeto está organizado nas seguintes camadas:

### Controllers (API)

A camada de Controllers expõe os endpoints para interação com a aplicação via HTTP. Ela não contém nenhuma lógica de negócios, apenas chamadas para validações e operações de serviço. As validações de entidades são realizadas utilizando o `ValidateModel` da camada Commons, que auxilia na validação com FluentValidation.

### Commons

A camada Commons contém utilitários e componentes reutilizáveis por outras camadas. Inclui regex, middlewares, notificações (filters), mensagens de sucesso e erro, e validações independentes (cpf, telefone, placa) que podem ser usadas tanto na camada de Service quanto na camada de Business.

### Services

A camada de Services é responsável por chamar a camada de Repository e Business. As validações nesta camada controlam o fluxo e aplicam regras comuns a todas as aplicações, e não específicas do sistema. Também gerencia transações e rollbacks por meio do UnitOfWork antes e depois de realizar operações de criação, atualização ou remoção no banco de dados por meio da camada repositories. O que permite o controle sobre a integridade dos dados. 

### Business

A camada de Business gerencia as regras de negócio. Em cenários específicos, como ao buscar um registro, realiza validações de fluxo seguidas de validações de negócio antes de retornar o registro à camada de Service. Caso contrário, foca exclusivamente nas regras de negócio. Esta camada interage apenas com a camada de Repository para consultas, não realizando operações de criação, atualização ou remoção. Ela é acionada exclusivamente pela camada de Service e tem conhecimento apenas da camada de Repository para verificação de existência ou busca de dados para validações.

### Entities

A camada de Entities contém os validadores (FluentValidation), responses, requests, models, enums e DTOs. As respostas são usadas para devolver registros ao frontend, enquanto os corpos das requisições são recebidos por classes de Request. Os DTOs são usados para integração com outras aplicações que necessitam de um corpo específico. As models representam os dados do domínio e são utilizadas para mapear os dados da aplicação para o banco de dados.

### Repositories

A camada de Repositories gerencia o acesso aos dados. Embora não esteja utilizando o Specification Pattern e Generic Repository atualmente, a implementação dessas práticas está planejada para futuras atualizações.

## Tratamento de Exceções

Neste projeto, evitamos ao máximo o uso de exceções em pontos que não seja necessário. Não que o projeto seja contra, mas por perfomance e em facilidade no uso do pattern de notification nos métodos, a tendência é que evite o uso demasiado de "throw new exception()". Por padrão, não usamos try catch, apenas em pontos específicos do código ou tratativas não mapeadas, isto é, exceções podem ser lançadas somente em pontos específico. Por exemplo, jobs, métodos que devem continuar mesmo que haja exception em algo que não afeta o objetivo final da execução e assim por diante. Este conceito não torna exceções um ponto negativo da aplicação, já que possuímos um middleware que realiza o tratamento das mesma em cenários de erros não mapeados ou instabilidade no banco de dados.

## Notification Pattern

Utilizamos o Notification Pattern no lugar de exceções para retornar erros de acordo com a convenção de APIs RESTful e evitar o lançamento de exceções demasiadamente e todo o stack trace gerado ao lançar uma exception. As notificações incluem o título do erro (ex.: Requisição Inválida, Não Encontrado), o status code e detalhes customizados da mensagem. As respostas de sucesso e erro podem incluir status como 200 (OK), 204 (No Content), 400 (Bad Request), 401 (Unauthorized), 403 (Forbidden), 404 (Not Found), 409 (Conflict), 500 (Internal Server Error), e outros conforme o contexto específico. Além disso, um middleware protege a aplicação para não retornar informações sensíveis em caso de exceções, formatando a resposta como um erro 500 com uma mensagem genérica de falha no processamento.

## Tecnologias Utilizadas

O projeto utiliza diversas tecnologias, algumas das quais foram definidas e reutilizadas da versão anterior do projeto, incluindo:

- **Azure**: Utilizado para hospedagem do servidor e serviços adicionais como envio de emails.
- **Azure Key Vault**: Planejado para uso futuro para gerenciamento de segredos.
- **Zenvia**: Para envio de SMS.
- **Assas**: Para possíveis transações bancárias.
- **Entity Framework**: Para acesso e manipulação de dados.
- **iTextSharp**: Para geração de PDFs de contratos de locação de frotas.
- **ClosedXML**: Para geração de relatórios em Excel.
- **SQL Server com Docker**: Utilizado como banco de dados, facilitando o ambiente de desenvolvimento e deploy.
- **Docker**: Possivelmente utilizado também para a API em futuras atualizações.
- **UserManager**: Para gerenciamento de usuários e controle de autenticação e autorização.
- **AutoMapper**: Utilizado para mapeamento de objetos em aplicações .NET, facilitando a transferência de dados entre models, DTOs, requests e responses.
- **JWT**: Utilizado para geração e validação de tokens de autenticação, garantindo segurança na comunicação entre cliente e servidor.

Boa parte dessas tecnologias já foram integradas na versão anterior do projeto, que está pronta, mas com menos regras em relação às convenções padrões de mercado e boas práticas. Outras estão nessa ou serão implementadas.

## Instalação e Uso

### Pré-requisitos

- .NET 8 SDK
- Docker

### Passos para Configuração

1. Clone o repositório:
    ```sh
    git clone https://github.com/seu-usuario/seu-projeto.git
    ```

2. Navegue até o diretório do projeto:
    ```sh
    cd seu-projeto/src/MyProject
    ```

3. Configure o Docker para o banco de dados SQL Server:
    ```sh
    docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Seu@SenhaForte" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2019-latest
    ```

4. Restaure as dependências:
    ```sh
    dotnet restore
    ```

5. Atualize a string de conexão no `appsettings.json` com as informações do banco de dados:
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost,1433;Database=MyDatabase;User Id=sa;Password=Seu@SenhaForte;"
    }
    ```

6. Execute as migrações do banco de dados (caso esteja utilizando Entity Framework):
    ```sh
    dotnet ef database update -s BusesControl.Api -p BusesControl.Persistence
    ```

7. Execute o projeto:
    ```sh
    dotnet run
    ```

## Contribuição

Sinta-se à vontade para abrir issues ou pull requests. Toda contribuição é bem-vinda!

## Licença

Este projeto está licenciado sob a Licença MIT. Veja o arquivo `LICENSE` para mais detalhes.
