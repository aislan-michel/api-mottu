# Etapa 1: Construção (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Defina o diretório de trabalho
WORKDIR /app

# Copie o arquivo csproj e restaure as dependências
COPY src/Mottu.Api/*.csproj ./src/Mottu.Api/
RUN dotnet restore src/Mottu.Api/Mottu.Api.csproj

# Copie o restante do código da aplicação
COPY . ./

# Publique a aplicação
RUN dotnet publish src/Mottu.Api/Mottu.Api.csproj -c Release -o out

# Etapa 2: Imagem de execução (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Defina o diretório de trabalho
WORKDIR /app

# Copie os arquivos publicados da etapa anterior
COPY --from=build /app/out .

# Defina a variável de ambiente para garantir que o ASP.NET Core ouça na porta 80
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

# Exponha a porta em que sua API vai rodar
EXPOSE 80

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "Mottu.Api.dll"]