using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Extensions.Configuration
{
    public static class IConfigurationExtensions
    {
        public static IConfiguration ReplacePlaceholders(this IConfiguration @this, params string[] placeholderPrefixes)
        {
            if (@this == null) throw new System.ArgumentNullException(nameof(@this));
            if (placeholderPrefixes == null || placeholderPrefixes.Any() == false) throw new System.ArgumentNullException(nameof(placeholderPrefixes));

            var enumerable = @this.AsEnumerable();
            var keyValuesToMerge = enumerable
                .Where(keyValue => !string.IsNullOrEmpty(keyValue.Value) && placeholderPrefixes.Any(prefix => keyValue.Value.Contains(prefix)))
                .ToList();
            var domainKeyValues = enumerable
                .Where(keyValue => placeholderPrefixes.Any(prefix => keyValue.Key.StartsWith($"{prefix}_")))
                .ToDictionary(x => x.Key, x => x.Value);

            foreach (var keyValue in keyValuesToMerge)
            {
                var value = keyValue.Value;
                if (string.IsNullOrEmpty(value))
                    continue;

                foreach (var domainKeyValue in domainKeyValues)
                {
                    var regex = new Regex(domainKeyValue.Key);
                    if (regex.IsMatch(value))
                        value = regex.Replace(value, domainKeyValue.Value);
                }

                @this[keyValue.Key] = value;
            }

            return @this;
        }
    }
}