using System;

namespace Reap.Extensions.Headers {
    public static class HeadersExtensions {
        public static IHeadersExtension Extension(this Message message, ExtensionSelector<IHeadersExtension> extension, Action<IHeadersExtension> callback = null) {
            return message.Extension<IHeadersExtension>(extension, callback);
        }

        public static IHeadersExtension Headers(this Message message) {
            return message.Extension<IHeadersExtension>(new HeadersExtension());
        }
    }
}