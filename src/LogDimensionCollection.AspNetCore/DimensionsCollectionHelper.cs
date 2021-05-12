using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    public static class DimensionsCollectionHelper
    {
        private static JsonSerializerOptions SerializerOptions { get; } = new()
        {
            // Minimize the bytes sent to a log sink
            IgnoreNullValues = true
        };

        /// <summary>
        ///     Serialize then collect <paramref name="items" /> into <paramref name="source" />,
        ///     adding an optional <paramref name="dimensionPrefix" /> to each dimension key
        /// </summary>
        /// <param name="source">Existing dimensions to aggregate with</param>
        /// <param name="items">The dimensions to collect</param>
        /// <param name="dimensionPrefix">Optional prefix</param>
        /// <param name="serializer">
        ///     Delegate that will be used to serialize dimension values (defaults to
        ///     <see cref="SerializeValue" />
        /// </param>
        public static void SetDimensions(this IDictionary<string, string> source,
            IEnumerable<KeyValuePair<string, object>> items,
            string dimensionPrefix = null,
            Func<object, string> serializer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (items == null) return;

            serializer ??= SerializeValue;

            var serializedDimensions = items
                .HasKey()
                .PrefixKey(dimensionPrefix)
                .Select(x => new KeyValuePair<string, string>(x.Key, serializer(x.Value)));

            // materialize ALL dimensions so that any exception leaves the original list unaltered
            var materializedDimensions = serializedDimensions.ToList();

            materializedDimensions
                .Aggregate(source, (result, d) =>
                {
                    result[d.Key] = d.Value;
                    return result;
                });
        }

        /// <summary>
        ///     Return only those dimension entries that have a value other than whitespace
        /// </summary>
        public static IEnumerable<KeyValuePair<string, object>> HasKey(
            this IEnumerable<KeyValuePair<string, object>> source)
        {
            return source.Where(d => !string.IsNullOrWhiteSpace(d.Key));
        }

        /// <summary>
        ///     Prefix the key of each dimension with the <paramref name="dimensionPrefix" /> supplied
        /// </summary>
        public static IEnumerable<KeyValuePair<string, object>> PrefixKey(
            this IEnumerable<KeyValuePair<string, object>> source, string dimensionPrefix)
        {
            dimensionPrefix = StringUtils.PascalCase(dimensionPrefix);
            return source.Select(d =>
            {
                var (rawKey, value) = d;
                var dimensionKey = StringUtils.PascalCase(rawKey);
                var key = string.IsNullOrEmpty(dimensionPrefix) || rawKey.StartsWith(dimensionPrefix)
                    ? dimensionKey
                    : $"{dimensionPrefix}{dimensionKey}";
                return new KeyValuePair<string, object>(key, value);
            });
        }

        /// <summary>
        ///     The default implementation used to serialize value to be sent as custom dimensions
        /// </summary>
        /// <remarks>
        ///     Will serialize primitive and primitive-like values (eg DateTime)
        ///     method and for all other types to try and serialize
        ///     the object as a JSON string
        /// </remarks>
        public static string SerializeValue(object value)
        {
            return value switch
            {
                null => null,
                string s => s,
                int _ => value.ToString(),
                DateTime dtm => dtm.ToString("O"),
                DateTimeOffset dtm2 => dtm2.ToString("O"),
                _ => TrySerializeAsJson(value)
            };
        }

        private static string TrySerializeAsJson(object value)
        {
            try
            {
                return JsonSerializer.Serialize(value, SerializerOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
