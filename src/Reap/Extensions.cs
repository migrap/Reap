using System;

namespace Reap {
    public static partial class Extensions {
        public static TResult Extension<TResult>(this Message message, ExtensionSelector<Message, TResult> selector, Action<TResult> callback = null) where TResult : IExtension<Message> {
            return message.Extensions(selector, callback);
        }        
    }
}