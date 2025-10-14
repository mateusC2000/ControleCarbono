# Projeto - ControleCarbono

Aplicação para mapeamento de emissões de carbono em empresas, desenvolvida com .NET. Esta API permite o cadastro de empresas, usuários e registros de emissão de carbono, facilitando o controle e a análise de dados ambientais para cidades mais inteligentes e sustentáveis.

##  Como executar localmente com Docker

Para executar a aplicação e os serviços relacionados localmente, você precisará ter o Docker e o Docker Compose instalados.

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/mateusC2000/ControleCarbono.git
    cd ControleCarbono
    ```

2.  **Suba a aplicação:**
    O comando a seguir irá construir a imagem da API e iniciar o container.
    ```bash
    docker compose up
    ```
    A API estará disponível em `http://localhost:8080`. A documentação do Swagger pode ser acessada em `http://localhost:8080/swagger`.

3.  **Para executar os testes:**
    Utilize o seguinte comando para rodar os testes de integração em um ambiente containerizado:
    ```bash
    docker compose up tests
    ```

## 🔄 Pipeline CI/CD

<img width="1651" height="580" alt="Screenshot from 2025-10-12 22-00-09" src="https://github.com/user-attachments/assets/94719c08-d524-472f-8f37-85387b0da948" />


O projeto utiliza **GitHub Actions** para automatizar os processos de Integração Contínua (CI) e Entrega Contínua (CD).

### Etapas do Pipeline

O pipeline é acionado a cada `push` na branch `main` e consiste nos seguintes jobs:

1.  **`build`**:
    -   **Ambiente**: `ubuntu-latest`.
    -   **Passos**:
        -   Configura o ambiente .NET 8.
        -   Constrói a aplicação (`dotnet build`).
        -   Executa os testes automatizados (`dotnet test`).
        -   Publica os artefatos da aplicação (`dotnet publish`).
        -   Faz o upload dos artefatos para serem utilizados nos jobs de deploy.

2.  **`deploy-staging`**:
    -   **Dependência**: Executado após o sucesso do job `build`.
    -   **Ambiente**: `ubuntu-latest`.
    -   **Passos**:
        -   Baixa os artefatos da aplicação.
        -   Realiza o login no Azure utilizando OpenID Connect (OIDC).
        -   Faz o deploy da aplicação no slot de **Staging** do Azure App Service.

3.  **`deploy-production`**:
    -   **Dependência**: Executado após o sucesso do job `deploy-staging`.
    -   **Ambiente**: `ubuntu-latest`.
    -   **Passos**:
        -   Baixa os artefatos da aplicação.
        -   Realiza o login no Azure.
        -   Faz o deploy da aplicação no slot de **Production** do Azure App Service.

Este fluxo garante que o código seja testado antes de ser implantado em um ambiente de homologação (Staging) e, finalmente, promovido para Produção.

## 📦 Containerização

A aplicação é containerizada usando Docker para garantir consistência entre os ambientes de desenvolvimento, teste e produção.

### Dockerfile

O `Dockerfile` utiliza uma abordagem *multi-stage build* para criar uma imagem otimizada e segura:

```dockerfile
# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos .csproj e restaura as dependências
COPY ["Fiap.Api.ControleCarbono/Fiap.Api.ControleCarbono.csproj", "Fiap.Api.ControleCarbono/"]
RUN dotnet restore "Fiap.Api.ControleCarbono/Fiap.Api.ControleCarbono.csproj"

# Copia o restante do código fonte e publica a aplicação
COPY . .
WORKDIR "/src/Fiap.Api.ControleCarbono"
RUN dotnet build "Fiap.Api.ControleCarbono.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fiap.Api.ControleCarbono.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio Final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expõe as portas HTTP e HTTPS
EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "Fiap.Api.ControleCarbono.dll"]
```

### Estratégias Adotadas

-   **Multi-stage Build**: O primeiro estágio (`build`) compila a aplicação e publica os artefatos. O estágio final (`final`) copia apenas os artefatos publicados para uma imagem base do ASP.NET runtime, que é muito menor que a imagem do SDK. Isso resulta em uma imagem final mais leve e com menor superfície de ataque.
-   **Cache de Camadas**: A restauração de pacotes NuGet é feita em uma camada separada, antes de copiar todo o código-fonte. Isso permite que o Docker armazene em cache a camada de dependências, acelerando builds futuros caso as dependências não mudem.

## 🖼️ Prints do funcionamento

### Execução Local (Swagger)

*<ins>Aqui você pode inserir um print da interface do Swagger rodando localmente.</ins>*

### Pipeline no GitHub Actions

*<ins>Aqui você pode inserir um print do pipeline sendo executado com sucesso no GitHub Actions.</ins>*

### Ambiente de Staging

-   *<ins>Aqui você pode inserir um print da aplicação funcionando no ambiente de Staging.</ins>*

### Ambiente de Produção

A aplicação em produção está disponível para os usuários finais.

-   *<ins>Aqui você pode inserir um print da aplicação funcionando no ambiente de Produção.</ins>*

## 🛠️ Tecnologias utilizadas

### Backend

-   **.NET 8**: Framework para construção da API.
-   **ASP.NET Core**: Para criar a aplicação web e os endpoints da API.
-   **Entity Framework Core**: ORM para interação com o banco de dados.
-   **AutoMapper**: Mapeamento de objetos (DTOs para Entidades).

### Segurança

-   **JWT (JSON Web Tokens)**: Para autenticação e autorização baseada em tokens.
-   **ASP.NET Core Identity**: Para gerenciamento de usuários e senhas.

### Banco de Dados

-   **Oracle**: Banco de dados relacional utilizado pela aplicação.

### Infraestrutura e DevOps

-   **Docker**: Para containerização da aplicação.
-   **Docker Compose**: Para orquestração de containers no ambiente local.
-   **GitHub Actions**: Para automação do pipeline de CI/CD.
-   **Azure App Service**: Plataforma de nuvem para hospedagem da aplicação (Staging e Produção).

### Documentação

-   **Swagger (Swashbuckle)**: Para documentação interativa da API.


### Ambientes


- Produção:
  * Disponível na url: https://controlecarbono-bqajafhzhkbhhfcq.canadacentral-01.azurewebsites.net

<img width="1317" height="704" alt="Screenshot from 2025-10-13 23-51-35" src="https://github.com/user-attachments/assets/9fe2a757-a57f-41d1-a0b4-574235208f31" />

- Stage:
  * Disponível na url: https://controlecarbono-bqajafhzhkbhhfcq.canadacentral-01.azurewebsites.net
    
<img width="1317" height="704" alt="Screenshot from 2025-10-13 23-52-02" src="https://github.com/user-attachments/assets/35c913fe-57bc-4576-9649-c7ce9b6fe776" />
