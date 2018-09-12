using EnvSettings.Example.WebApp.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;

namespace EnvSettings.Example.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.BuildAndReplacePlaceholders();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions()
                .Configure<MongoConfig>(Configuration.GetSection(nameof(MongoConfig)));

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<MongoConfig> options)
            => app.Run(async (context) =>
                await context.Response.WriteAsync($"This is my Mongo configuration in {env.EnvironmentName}: {JsonConvert.SerializeObject(options.Value)}"));
    }
}