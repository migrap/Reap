using System.Collections.Generic;

namespace Reap.Extensions.Claims {
    public interface IClaimCollection : IEnumerable<Claim> {
        void Add(Claim claim);
    }
}