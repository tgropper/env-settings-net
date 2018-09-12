using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Extensions.Configuration
{
    public static class IConfigurationExtensions
    {
        public static IConfiguration ReplacePlaceholders(this IConfiguration @this)
        {
            if (@this == null) throw new System.ArgumentNullException(nameof(@this));

            var valueToReplaceRegex = new Regex(@"(?<=\$\{)([a-zA-Z0-9_]+)(?=\})");

            @this.AsEnumerable()
                .Where(kv => !string.IsNullOrEmpty(kv.Value) && valueToReplaceRegex.IsMatch(kv.Value))
                .ToList()
                .ForEach(kv => valueToReplaceRegex.Matches(kv.Value).Cast<Match>()
                    .Select(x => x.Value).ToList()
                    .ForEach(match => @this[kv.Key] = @this[match] != null ? new Regex($"\\${{{match}}}").Replace(@this[kv.Key], @this[match]) : null));

            return @this;
        }
    }
}