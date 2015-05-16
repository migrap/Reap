using System.Collections.Generic;

namespace Reap.Extensions.Claims {
    public class ClaimCollection : IClaimCollection {
        private HashSet<Claim> _collection = new HashSet<Claim>();

        public void Add(Claim item) {
            _collection.Add(item);
        }
    }
}