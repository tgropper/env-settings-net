using System.Linq;

namespace Microsoft.Extensions.Configuration
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfiguration Build(this IConfigurationBuilder @this, params string[] placeholderPrefixesToReplace)
        {
            var configuration = @this.Build();
            if (placeholderPrefixesToReplace != null & placeholderPrefixesToReplace.Any())
                configuration.ReplacePlaceholders(placeholderPrefixesToReplace);

            return configuration;
        }
    }
}