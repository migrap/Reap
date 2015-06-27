using System;
using System.Linq;
using Newtonsoft.Json;

namespace Reap.Newtonsoft.Json {
    public class MessageSerializerSettings : JsonSerializerSettings {
        public MessageSerializerSettings() {
            ContractResolver = new MessageContractResolver();
            Formatting = Formatting.Indented;
            NullValueHandling = NullValueHandling.Ignore;
            Converters.Add(new MessageConverter());
        }

        public MessageSerializerSettings(params JsonConverter[] converters):this() {
            foreach(var converter in converters) {
                Converters.Add(converter);
            }
        }

        public MessageSerializerSettings(params Func<MessageSerializerSettings, Func<JsonConverter>>[] converters)
            : this(converters.Select(x => x(null)()).ToArray()) {
        }
    }
}
