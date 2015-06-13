using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Reap.Newtonsoft.Json {
    public class MessageConverter : JsonConverter {
        private readonly IReadOnlyList<Extension> _extensions = (new DefaultExtensionProvider()).Extensions;

        public override bool CanConvert(Type objectType) {
            return typeof(Message).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var name = (string)null;
            var message = new Message();

            while(reader.Read()) {
                if(reader.TokenType == JsonToken.PropertyName) {
                    name = reader.Value.ToString();
                } else if(reader.TokenType != JsonToken.EndObject) {
                    var extension = _extensions.First(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase));
                    var value = serializer.Deserialize(reader, extension.Type) as IExtension<Message>;
                    message.Extension(extension.Type ?? value.GetType(), value);
                }
            }

            return message;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var dictionary = new Dictionary<string, object>();

            foreach(var item in (value as Message).Extensions) {
                var extension = _extensions.FirstOrDefault(x => item.Key.IsAssignableFrom(x.Type));
                dictionary[extension?.Name ?? item.Key.Name] = item.Value;
            }

            serializer.Serialize(writer, dictionary);
        }
    }

    public static partial class Extensions {
        public static bool Any<TSource>(this IEnumerable<TSource> source) {
            return null != source && Enumerable.Any(source);
        }
    }
}