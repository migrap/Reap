using System;

namespace Reap.Extensions.Authorization {
    public static partial class AuthorizationExtensions {
        public static IAuthorizationExtension Extension(this Message message, ExtensionSelector<IAuthorizationExtension> extension, Action<IAuthorizationExtension> callback = null) {
            return message.Extension<IAuthorizationExtension>(extension, callback);
        }

        public static IAuthorizationExtension Authorization(this Message message) {
            return message.Extension<IAuthorizationExtension>(new AuthorizationExtension());
        }
    }
}