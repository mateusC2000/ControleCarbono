# Usa a imagem oficial do SDK do .NET 8.0 como base para o build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Define o diretório de trabalho dentro do contêiner como /src
WORKDIR /src

# Copia os arquivos .csproj primeiro (melhor para cache de camadas)
COPY Fiap.Api.ControleCarbono/Fiap.Api.ControleCarbono.csproj Fiap.Api.ControleCarbono/
COPY Fiap.Api.ControleCarbono.Tests/Fiap.Api.ControleCarbono.Tests.csproj Fiap.Api.ControleCarbono.Tests/

# Limpa o cache local do NuGet para garantir uma restauração limpa.
RUN dotnet nuget locals all --clear

# Cria um arquivo NuGet.Config explícito no diretório de trabalho, com apenas o feed oficial.
# Isso garante que não há outros feeds ou configurações interferindo.
RUN echo '<?xml version="1.0" encoding="utf-8"?>' > NuGet.Config && \
    echo '<configuration>' >> NuGet.Config && \
    echo '  <packageSources>' >> NuGet.Config && \
    echo '    <clear />' >> NuGet.Config && \
    echo '    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />' >> NuGet.Config && \
    echo '  </packageSources>' >> NuGet.Config && \
    echo '</configuration>' >> NuGet.Config

# PASSO DE DIAGNÓSTICO: Lista as fontes NuGet que o dotnet está usando.
RUN dotnet nuget list source --configfile NuGet.Config

# Restaura as dependências de AMBOS os projetos, usando o NuGet.Config recém-criado.
# Isso garante que todas as dependências sejam resolvidas, incluindo as inter-projetos.
RUN dotnet restore Fiap.Api.ControleCarbono/Fiap.Api.ControleCarbono.csproj --configfile NuGet.Config && \
    dotnet restore Fiap.Api.ControleCarbono.Tests/Fiap.Api.ControleCarbono.Tests.csproj --configfile NuGet.Config

# Copia todo o restante do código-fonte
# Isso é feito APÓS a restauração para aproveitar o cache de camadas do Docker
COPY Fiap.Api.ControleCarbono/ Fiap.Api.ControleCarbono/
COPY Fiap.Api.ControleCarbono.Tests/ Fiap.Api.ControleCarbono.Tests/

# Define o diretório de trabalho para o projeto de testes para os comandos subsequentes (build e test)
WORKDIR "/src/Fiap.Api.ControleCarbono.Tests"

# Constrói o projeto de testes.
RUN dotnet build

# Define o comando de entrada que será executado quando o contêiner iniciar.
ENTRYPOINT ["dotnet", "test", "--logger:trx"]
