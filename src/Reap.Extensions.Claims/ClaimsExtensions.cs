using System;

namespace Reap.Extensions.Claims {
    public static class ClaimsExtensions {
        //public static IClaimsExtension Extension(this Message message, ExtensionSelector<IClaimsExtension> extension, Action<IClaimsExtension> callback = null) {
        //    return message.Extension<IClaimsExtension>(extension, callback);
        //}

        //public static IClaimsExtension Claims(this Message message) {
        //    return message.Extension<IClaimsExtension>();
        //}

        public static void Add(this IClaimCollection collection, string issuer, string type, string value) {
            collection.Add(new Claim(issuer, type, value));
        }

        public static void Add(this IClaimCollection collection, string type, string value) {
            collection.Add(new Claim(type, value));
        }
    }
}