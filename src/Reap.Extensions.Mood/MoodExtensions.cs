using System;

namespace Reap.Extensions.Mood {
    public static class MoodExtensions {
        public static IMoodExtension Extension(this Message message, ExtensionSelector<IMoodExtension> extension, Action<IMoodExtension> callback = null) {
            return message.Extension<IMoodExtension>(extension, callback);
        }

        public static IMoodExtension Mood(this Message message) {
            return message.Extension<IMoodExtension>();
        }
    }
}