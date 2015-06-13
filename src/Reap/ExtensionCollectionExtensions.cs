using System.Linq.Expressions;

namespace Reap {
    public static partial class ExtensionCollectionExtensions {
        public static bool Contains<TSource>(this TSource extensions, params Expression<ExtensionSelector<TSource, IExtension<TSource>>>[] expressions) where TSource : IExtensible<TSource> {
            return true;
        }
    }
}