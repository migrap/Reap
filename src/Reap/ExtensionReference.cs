using System.Reflection;
using System.Threading;

namespace Reap {
    public struct ExtensionReference<T> {
        private T _extension;
        private int _revision;

        private ExtensionReference(T extension, int revision) {
            _extension = extension;
            _revision = revision;
        }

        public static readonly ExtensionReference<T> Default = new ExtensionReference<T>(default(T), -1);

        public T Fetch(IExtensionCollection extensions) {
            var value = (object)null;

            if(_revision == extensions.Revision) {
                return _extension;
            }            

            if(extensions.TryGetValue(typeof(T).GetTypeInfo(), out value)) {
                _extension = (T)value;
            } else {
                _extension = default(T);
            }

            Interlocked.Exchange(ref _revision, extensions.Revision);

            return _extension;
        }

        public T Update(IExtensionCollection extensions, T extension) {
            extensions[typeof(T).GetTypeInfo()] = _extension = extension;
            Interlocked.Exchange(ref _revision, extensions.Revision);
            return extension;
        }
    }
}
