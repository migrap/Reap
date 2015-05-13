using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Reap {
    internal class ExtensionFactory : IExtensionFactory {
        public object Create(Type type) {
            if(type.IsInterface) {
                type = Assemblies.SelectMany(x => x.DefinedTypes)
                    .Where(x => false == x.IsInterface)
                    .First(x => ImplementedInterface(x, type));
            }
            return Activator.CreateInstance(type);
        }

        public IEnumerable<Assembly> Assemblies { get; } = GetAssemblies();

        private static IEnumerable<Assembly> GetAssemblies() {
            return AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic && !StartsWith(x, "System", "Microsoft"));
        }

        private static bool ImplementedInterface(TypeInfo typeinfo, Type type) {
            return typeinfo.ImplementedInterfaces.Any(x => x.Equals(type));
        }

        private static bool StartsWith(Assembly assembly, params string[] values) {
            return StartsWith(assembly.GetName().Name, values);            
        }

        private static bool StartsWith(string name, params string[] values) {
            return values.Any(x => name.StartsWith(x));
        }
    }
}