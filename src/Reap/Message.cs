using System;

namespace Reap {
    public partial class Message {
        private readonly IExtensionCollection _extensions;
        private readonly IExtensionFactory _factory;

        public Message(IExtensionCollection extensions = null, IExtensionFactory factory = null) {
            _extensions = extensions ?? new ExtensionCollection();
            _factory = factory ?? new ExtensionFactory();
        }

        public Message() : this(null, null) {
        }

        public IExtensionCollection Extensions {
            get { return _extensions; }
        }

        public virtual object Extension(Type type) {
            var value = (object)null;
            if(false == _extensions.TryGetValue(type, out value)) {
                value = _factory.Create(type);
                Extension(type, value);
            }
            return value;
        }

        public virtual object Extension(Type type, object instance) {
            _extensions[type] = instance;
            return instance;
        }

        public virtual T Extension<T>() {
            return (T)Extension(typeof(T));
        }

        public virtual T Extension<T>(T instance) {
            return (T)Extension(typeof(T), instance);
        }

        public void Dispose() {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing) {
            if(disposing) {
                _extensions.Dispose();
            }
        }
    }
}