namespace Microsoft.Extensions.Configuration
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfiguration BuildAndReplacePlaceholders(this IConfigurationBuilder @this, bool replaceOnEmpty = true)
        {
            var configuration = @this.Build();
            configuration.ReplacePlaceholders(replaceOnEmpty);

            return configuration;
        }
    }
}