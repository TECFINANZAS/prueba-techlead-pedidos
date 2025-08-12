# Etapa 1: Compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar y restaurar dependencias (busca el .csproj en cualquier subcarpeta)
COPY . .
RUN dotnet restore $(find . -name "*.csproj")

# Publicar en Release
RUN dotnet publish $(find . -name "*.csproj") -c Release -o /app/out

# Etapa 2: Ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar binarios publicados
COPY --from=build /app/out .

# Configurar para que escuche el puerto asignado por Render
ENV ASPNETCORE_URLS=http://+:${PORT}

# Ejecutar la app (detecta el .dll automáticamente)
CMD ["sh", "-c", "dotnet $(find . -name '*.dll' | head -n 1)"]
