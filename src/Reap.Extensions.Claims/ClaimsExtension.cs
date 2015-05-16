namespace Reap.Extensions.Claims {
    public class ClaimsExtension : IClaimsExtension {
        public IClaimCollection Claims { get; } = new ClaimCollection();
    }
}