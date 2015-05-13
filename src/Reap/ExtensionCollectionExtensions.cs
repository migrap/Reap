using System;
using System.Linq;

namespace Reap {
    public interface IExtensionCollectionExtension { }
    public static class ExtensionCollectionExtensions {
        public static bool Contains(this IExtensionCollection source, params Func<IExtensionCollectionExtension, Func<Type>>[] types) {
            return !types.Any(type => !source.Keys.Contains(type(null)()));
        }
    }
}
