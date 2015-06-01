using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reap.Extensions.Objects {
    public class Links : IEnumerable<Link> {
        private List<Link> _collection;

        public Links(IEnumerable<Link> collection = null) {
            _collection = new List<Link>(collection ?? Enumerable.Empty<Link>());
        }

        public Links Add(params Link[] collection) {
            _collection.AddRange(collection);
            return this;
        }

        public Links Add(Rel rel, Href href) {
            return Add(new Link(rel, href));
        }

        public IEnumerator<Link> GetEnumerator() {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}