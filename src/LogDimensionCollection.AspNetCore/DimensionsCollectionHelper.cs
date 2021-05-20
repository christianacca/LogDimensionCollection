using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    public static class DimensionsCollectionHelper
    {
        private static JsonSerializerOptions SerializerOptions { get; } = new()
        {
            // Minimize the bytes sent to a log sink
            IgnoreNullValues = true
        };

        private static JsonSerializerSettings NewtonsoftSerializerOptions { get; } = new()
        {
            // Minimize the bytes sent to a log sink
            NullValueHandling = NullValueHandling.Ignore
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

            var serializedDimensions = items
                .HasKey()
                .PrefixKey(dimensionPrefix)
                .SerializeDimension(serializer ?? SerializeValue);

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
        ///     Serialize dimension values
        /// </summary>
        public static IEnumerable<KeyValuePair<string, string>> SerializeDimension(
            this IEnumerable<KeyValuePair<string, object>> source, Func<object, string> serializer)
        {
            return source.Select(x => new KeyValuePair<string, string>(x.Key, serializer(x.Value)));
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
                _ => IsNewtonsoftType(value) ? TryNewtonsoftSerializeAsJson(value) : TrySerializeAsJson(value)
            };
        }

        private static bool IsNewtonsoftType(object value)
        {
            return value != null && value.GetType().Namespace?.StartsWith("Newtonsoft.Json") == true;
        }

        private static string TryNewtonsoftSerializeAsJson(object value)
        {
            try
            {
                return JsonConvert.SerializeObject(value, NewtonsoftSerializerOptions);
            }
            catch (JsonSerializationException)
            {
                return null;
            }
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
