using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Reap.Newtonsoft.Json {
    public class MessageConverter : JsonConverter {
        private IExtensionProvider _extensions = new ExtensionProvider();

        public override bool CanConvert(Type objectType) {
            return typeof(Message).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var dictionary = new Dictionary<string, object>();
            serializer.Populate(reader, dictionary);

            var message = new Message();

            foreach(var item in dictionary) {
                var descriptor = _extensions.Extensions.First(x => item.Key.Equals(x.ExtensionName, StringComparison.OrdinalIgnoreCase));
                var extension = (object)null;
                var converter = serializer.Converters.FirstOrDefault(x => x.CanConvert(descriptor.ExtensionType));

                if(converter!=null) {
                    extension = converter.ReadJson(reader, descriptor.ExtensionType, item.Value, serializer);
                } else {
                    var method = typeof(JObject).GetMethod("ToObject", new Type[] { typeof(JsonSerializer) }).MakeGenericMethod(descriptor.ExtensionType);
                    extension = method.Invoke(item.Value, new object[] { serializer });
                }

                message.Extension(descriptor?.ExtensionType ?? extension.GetType(), extension);
            }

            return message;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var dictionary = new Dictionary<string, object>();

            foreach(var extension in (value as Message).Extensions) {
                var descriptor = _extensions.Extensions.FirstOrDefault(x => extension.Key.IsAssignableFrom(x.ExtensionType));
                dictionary[descriptor?.ExtensionName ?? extension.Key.Name] = extension.Value;
            }

            serializer.Serialize(writer, dictionary);
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class NonExtensionAttribute : Attribute {
    }

    public class ExtensionDescriptor {
        public ExtensionDescriptor(string extensionName, Type extensionType) {
            ExtensionName = extensionName;
            ExtensionType = extensionType;
        }

        public string ExtensionName { get; }
        public Type ExtensionType { get; }
    }

    public interface IExtensionProvider {
        IReadOnlyList<ExtensionDescriptor> Extensions { get; }
    }

    public class ExtensionProvider : IExtensionProvider {
        private readonly string ExtensionName = "Extension";

        public ExtensionProvider() {
        }

        private IEnumerable<Assembly> Assemblies { get; } = GetAssemblies();

        public IReadOnlyList<ExtensionDescriptor> Extensions => GetExtensions();

        private static IEnumerable<Assembly> GetAssemblies() {
            return AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic && !StartsWith(x, "System", "Microsoft", "Newtonsoft"));
        }

        private IReadOnlyList<ExtensionDescriptor> GetExtensions() {
            var descriptors = new List<ExtensionDescriptor>();
            var assemblies = new HashSet<Assembly>(GetAssemblies());
            var extensions = GetExtensions(assemblies);            

            foreach(var extension in extensions) {
                var extensionType = extension;
                var extensionName = extensionType.Name.EndsWith(ExtensionName, StringComparison.OrdinalIgnoreCase) ? extensionType.Name.Substring(0, extensionType.Name.Length - ExtensionName.Length) : extensionType.Name;

                descriptors.Add(new ExtensionDescriptor(extensionName, extensionType));
            }

            return descriptors;
        }

        private static bool StartsWith(Assembly assembly, params string[] values) {
            return StartsWith(assembly.GetName().Name, values);
        }

        private static bool StartsWith(string name, params string[] values) {
            return values.Any(x => name.StartsWith(x));
        }

        private IReadOnlyCollection<Type> GetExtensions(HashSet<Assembly> assemblies) {
            return assemblies.SelectMany(x => x.DefinedTypes)
                .Where(x => IsExtension(x, assemblies))
                .ToList();
        }

        protected internal virtual bool IsExtension(Type type, ISet<Assembly> assemblies) {
            if(type.IsClass == false) {
                return false;
            }

            if(type.IsAbstract) {
                return false;
            }

            if(type.IsPublic == false) {
                return false;
            }

            if(type.ContainsGenericParameters) {
                return false;
            }

            if(type.IsDefined(typeof(NonExtensionAttribute))) {
                return false;
            }

            return type.Name.EndsWith(ExtensionName, StringComparison.Ordinal);
        }
    }

    public class MessageCamelCasePropertyNamesContractResolver : CamelCasePropertyNamesContractResolver {
        private IExtensionProvider _extensionProvider = new ExtensionProvider();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);

            if(typeof(Message).IsAssignableFrom(property.DeclaringType) && property.PropertyType == typeof(IExtensionCollection)) {
                property.ShouldSerialize = instance => false;
            }

            return property;
        }
    }

    public static partial class Extensions {
        public static bool Any<TSource>(this IEnumerable<TSource> source) {
            return null != source && Enumerable.Any(source);
        }
    }
}