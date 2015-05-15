using System;

namespace Reap.Extensions.Authentication {
    public static class AuthenticationExtensions {
        public static IAuthenticationExtension Extension(this Message message, ExtensionSelector<IAuthenticationExtension> extension, Action<IAuthenticationExtension> callback = null) {
            return message.Extension<IAuthenticationExtension>(extension, callback);
        }

        public static IAuthenticationExtension Authentication(this Message message) {
            return message.Extension<IAuthenticationExtension>();
        }
    }
}