# Etapa 1: Compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos de proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código y compilar en Release
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Etapa 2: Ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los binarios publicados
COPY --from=build /app/out .

# Configurar para que escuche el puerto asignado por Render
ENV ASPNETCORE_URLS=http://+:${PORT}

ENTRYPOINT ["dotnet", "RestfulPrueba.dll"]