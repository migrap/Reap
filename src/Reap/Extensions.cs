using System;

namespace Reap {
    public static partial class Extensions {
        public static TResult Extension<TSource, TResult>(this TSource source, TResult extension, Action<TResult> callback) where TSource : IExtensible<TSource> where TResult : IExtension<TSource> {
            if(callback != null) {
                callback(extension);
            }
            return extension;
        }

        public static TResult Extension<TSource, TResult>(this TSource source, ExtensionSelector<TSource, TResult> selector, Action<TResult> callback = null) where TSource : IExtensible<TSource> where TResult : IExtension<TSource> {
            throw new NotImplementedException();
        }

        public static TResult Extension<TResult>(this Message message, ExtensionSelector<Message, TResult> extension, Action<TResult> callback = null) where TResult : IExtension<Message> {
            return message.Extension(extension(message)(), callback);
        }
    }
}