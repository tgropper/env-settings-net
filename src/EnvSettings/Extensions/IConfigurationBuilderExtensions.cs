namespace Microsoft.Extensions.Configuration
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfiguration BuildAndReplacePlaceholders(this IConfigurationBuilder @this)
        {
            var configuration = @this.Build();
            configuration.ReplacePlaceholders();

            return configuration;
        }
    }
}