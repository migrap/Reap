using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Reap.Newtonsoft.Json {
    public class MessageContractResolver : CamelCasePropertyNamesContractResolver {
        private IExtensionProvider _extensionProvider = new DefaultExtensionProvider();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);

            if(typeof(Message).IsAssignableFrom(property.DeclaringType) && typeof(IExtensionCollection<Message>).IsAssignableFrom(property.PropertyType)) {
                property.ShouldSerialize = instance => false;
            }

            return property;
        }
    }
}