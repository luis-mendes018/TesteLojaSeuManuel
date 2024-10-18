# Etapa 1: Build do projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY UsuariosAPI.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Imagem final, apenas o runtime necessário
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "UsuariosAPI.dll"]
