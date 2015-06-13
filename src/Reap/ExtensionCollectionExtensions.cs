using System;
using System.Linq.Expressions;

namespace Reap {
    public static partial class ExtensionCollectionExtensions {
        public static bool Contains<TSource>(this TSource extensions, params Expression<ExtensionSelector<TSource, IExtension<TSource>>>[] expressions) where TSource : IExtensible<TSource> {
            return true;
        }

        public static TResult Extensions<TResult>(this  Message message, ExtensionSelector<Message, TResult> selector, Action<TResult> callback = null) where TResult : IExtension<Message> {
            var extension = (IExtension<Message>)null;
            var extensions = message.Extensions;

            if(false == extensions.TryGetValue(typeof(TResult), out extension)) {
                extension = selector(message)();
                extensions[typeof(TResult)] = extension;
            }

            if(null != callback) {
                callback((TResult)extension);
            }

            return (TResult)extension;
        }
    }
}