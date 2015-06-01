using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reap.Extensions.Objects {
    public class ObjectCollection : IEnumerable<IObjectExtension> {
        private List<IObjectExtension> _collection;

        public ObjectCollection(IEnumerable<IObjectExtension> collection = null) {
            _collection = new List<IObjectExtension>(collection ?? Enumerable.Empty<IObjectExtension>());
        }

        public ObjectCollection Add(params IObjectExtension[] collection) {
            _collection.AddRange(collection);
            return this;
        }

        public IEnumerator<IObjectExtension> GetEnumerator() {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}