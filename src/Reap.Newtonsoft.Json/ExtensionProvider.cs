using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reap.Newtonsoft.Json {
    public class DefaultExtensionProvider : IExtensionProvider {
        private readonly string ExtensionName = "Extension";

        public DefaultExtensionProvider() {
        }

        private IEnumerable<Assembly> Assemblies { get; } = GetAssemblies();

        public IReadOnlyList<Extension> Extensions => GetExtensions();

        private static IEnumerable<Assembly> GetAssemblies() {
            return AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic && !StartsWith(x, "System", "Microsoft", "Newtonsoft"));
        }

        private IReadOnlyList<Extension> GetExtensions() {
            var descriptors = new List<Extension>();
            var assemblies = new HashSet<Assembly>(GetAssemblies());
            var extensions = GetExtensions(assemblies);

            foreach(var extension in extensions) {
                var extensionType = extension;
                var extensionName = extensionType.Name.EndsWith(ExtensionName, StringComparison.OrdinalIgnoreCase) ? extensionType.Name.Substring(0, extensionType.Name.Length - ExtensionName.Length) : extensionType.Name;

                descriptors.Add(new Extension(extensionName, extensionType));
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
}