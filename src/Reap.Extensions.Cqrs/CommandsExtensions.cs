using System;

namespace Reap.Extensions.Cqrs {
    public static class CommandsExtensions {
        public static ICommandsExtension Extension(this Message message, ExtensionSelector<ICommandsExtension> extension, Action<ICommandsExtension> callback = null) {
            return message.Extension<ICommandsExtension>(extension, callback);
        }

        public static ICommandsExtension Headers(this Message message) {
            return message.Extension<ICommandsExtension>();
        }
    }
}