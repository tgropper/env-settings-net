# env-settings-net
Replace placeholders from appsettings files with values loaded from environment variables.

## Install [![NuGet](https://img.shields.io/nuget/v/EnvSettings.svg)](https://www.nuget.org/packages/EnvSettings)
From the package manager console:
```
PM> Install-Package EnvSettings
```

## Usage

With `EnvSettings` you can define a unique template for your configurations in your `appsettings.json` (generic for any environment) and then load the dynamic values from environment variables.

Following with the previous example, you can have an *appsettings.json* file with placeholders to replace like this:

```json
{
  "MongoConfig": {
    "ConnectionString": "mongodb://MONGO_USER:MONGO_PASS@MONGO_HOST:MONGO_PORT",
    "Database": "MONGO_DATABASE"
  }
}
```

And then load from the environment variables the values to replace the placeholders, for example:

**launchSettings.json**
```json
{
  "profiles": {
    "Development": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "MONGO_HOST": "mongo-host",
        "MONGO_PORT": "27017",
        "MONGO_USER": "mongodb",
        "MONGO_PASS": "mongodb",
        "MONGO_DATABASE": "env-settings-net"
      },
      "applicationUrl": "http://localhost:61658"
    }
  }
}
```

Or:

**docker-compose.yml**
```yml
version: '3'
services:
  app:
    image: env-settings-net
    build: .
    ports:
      - 8000:80
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - MONGO_HOST=mongo-host
      - MONGO_PORT=27017
      - MONGO_USER=mongodb
      - MONGO_PASS=mongodb
      - MONGO_DATABASE=env-settings-net
```

To make this happen, the *merge* process is triggered using the `IConfigurationBuilder.Build(params string[] placeholderPrefixesToReplace)` extension:

**Startup**
```cs
public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IHostingEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        Configuration = builder.Build("MONGO");
    }
    ...
}
```

**FAQ**: Why the *placeholderPrefixes*? Just to be sure that the values being replaced in your configuration are the one that you are expecting to be replaced.

## Example

Rename the `launchSettings-example.json` to `launchSettings.json` and then run the example within `Visual Studio` or using the `dotnet` CLI:

```
cd examples/EnvSettings.Example.WebApp
dotnet build
dotnet run --launch-profile {Environment} // try using different values (Development|Staging) to see different outputs
```

Or with `Docker` from root directory::

```
docker build -t env-settings-net -f examples/EnvSettings.Example.WebApp/Dockerfile .
docker run -it -p 8000:80 -e ASPNETCORE_ENVIRONMENT=Development -e MONGO_HOST=localhost -e MONGO_PORT=27017 -e MONGO_DATABASE=env-settings-net-dev env-settings-net
```

## Motivation

A good application configuration *must* be through environment variables. It gives the opportunity to reuse already existing configs or secrets in the environment where your application is being deployed (in fact, the motivation of this library is to take advantage of `ConfigMaps` and `Secrets` in `Kubernetes`).

Sometimes using environment variables to configure the entire application can be challenging and repeatable. For example, if your application uses a `mongo` connection string, then in `.net core` you would have two options (there are more options but this two are the nicest):
1. Define the connection string in each `appsettings.{environment}.json` like this:
* `appsettings.Development.json`: `mongodb://dev-user:dev-pass@dev-host:dev-port`.
* `appsettings.Production.json`: `mongodb://prod-user:prod-pass@prod-host:prod-port`.
* And so on for all your environments.

2. Pass the connection string as environment variable. So if you use different infrastructure in each environment you must configure your run files to have this tedious conn string (and we are using mongo as example and not a RDB conn string). This could be a `launchSettings.json` in dev, `docker-compose.yml` in qa or `Kubernetes` yamls in prod.


This means that in every environment you must hardcode the entire conn string. There is no midpoint, even when it's clear that the conn string is a static template that has dynamic variables depending on the environment where your application is running.

### TODOs
- [ ] Add unit tests.
- [ ] Add CI.
- [ ] Improve and add examples.
- [ ] Support not having the *placeholderPrefixes* parameter.

