using System;

namespace Reap.Extensions.Resource {
    public static class ResourceExtensions {
        //public static IResourceExtension Extension(this Message message, ExtensionSelector<IResourceExtension> extension, Action<IResourceExtension> callback = null) {
        //    return message.Extension<IResourceExtension>(extension, callback);
        //}

        //public static IResourceExtension Extension(this Message message, ExtensionSelector<IResourceExtension> extension, string value) {
        //    return message.Extension<IResourceExtension>(extension, x => {
        //        x.Resource = value;
        //    });
        //}

        //public static IResourceExtension Extension(this Message message, ExtensionSelector<IResourceExtension> extension, Func<IResourceExtension, Func<IResourceExtension>> callback) {
        //    return message.Extension(callback(message.Extension<IResourceExtension>())());
        //}

        //public static IResourceExtension Resource(this Message message) {
        //    return message.Extension<IResourceExtension>();
        //}

        public static IResourceExtension Home(this IResourceExtension extension) {
            return new ResourceExtension("home");
        }
    }
}