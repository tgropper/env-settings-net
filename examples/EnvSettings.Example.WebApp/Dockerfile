FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-alpine3.9 AS build
WORKDIR /app

COPY src/EnvSettings/*.csproj ./src/EnvSettings/
COPY examples/EnvSettings.Example.WebApp/*.csproj ./examples/EnvSettings.Example.WebApp/

WORKDIR /app/src/EnvSettings
RUN dotnet restore EnvSettings.csproj
COPY src/EnvSettings/. ./
RUN dotnet publish EnvSettings.csproj -o /app/out

WORKDIR /app/examples/EnvSettings.Example.WebApp
RUN dotnet restore EnvSettings.Example.WebApp.csproj
COPY examples/EnvSettings.Example.WebApp/. ./
RUN dotnet publish EnvSettings.Example.WebApp.csproj -o /app/out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-alpine3.9 AS runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "EnvSettings.Example.WebApp.dll"]