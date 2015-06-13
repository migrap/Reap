using System;

namespace Reap {
    public static partial class Extensions {
        public static TResult Extension<TSource, TResult>(this TSource source, TResult extension, Action<TResult> callback) where TSource : IExtensible<TSource> where TResult : IExtension<TSource> {
            if(callback != null) {
                callback(extension);
            }
            return extension;
        }
    }
}
