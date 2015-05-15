using Newtonsoft.Json;

namespace Reap.Newtonsoft.Json {
    public class MessageSerializerSettings : JsonSerializerSettings {
        public MessageSerializerSettings() {
            ContractResolver = new MessageContractResolver();
            Formatting = Formatting.None;
            NullValueHandling = NullValueHandling.Ignore;

            Converters.Add(new MessageConverter());
        }
    }
}
