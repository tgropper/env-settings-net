using EnvSettings.Example.WebApp.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.Hosting;

namespace EnvSettings.Example.WebApp
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.BuildAndReplacePlaceholders();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions()
                .Configure<MongoConfig>(Configuration.GetSection(nameof(MongoConfig)));
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, IOptions<MongoConfig> options)
            => app.Run(async (context) =>
                await context.Response.WriteAsync($"This is my Mongo configuration in {env.EnvironmentName}: {System.Text.Json.JsonSerializer.Serialize(options.Value)}"));
    }
}