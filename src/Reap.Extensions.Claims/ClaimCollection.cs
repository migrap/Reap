using System.Collections;
using System.Collections.Generic;

namespace Reap.Extensions.Claims {
    public class ClaimCollection : IClaimCollection {
        private HashSet<Claim> _collection = new HashSet<Claim>();

        public void Add(Claim item) {
            _collection.Add(item);
        }

        public void Add(string issuer, string type, string value) {
            Add(new Claim(issuer, type, value));
        }

        public IEnumerator<Claim> GetEnumerator() {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}