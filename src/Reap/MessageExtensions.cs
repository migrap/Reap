using System;

namespace Reap {
    public static class MessageExtensions {
        private static T Extension<T>(this Message message, T extension, Action<T> callback) {
            if(callback != null) {
                callback(extension);
            }
            return extension;
        }

        public static T Extension<T>(this Message message, ExtensionSelector<T> extension, Action<T> callback) {
            return message.Extension(extension(message)(), callback);
        }
    }
}