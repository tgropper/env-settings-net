FROM microsoft/aspnetcore-build:2.0 AS build-env
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

FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "EnvSettings.Example.WebApp.dll"]