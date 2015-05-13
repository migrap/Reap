using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Reap {
    public class ExtensionCollection : IExtensionCollection {
        private readonly IExtensionCollection _defaults;
        private readonly Dictionary<Type, object> _bytype = new Dictionary<Type, object>(TypeComparer.Default);
        private readonly Dictionary<string, Type> _byname = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        private readonly object _sync = new Object();        
        private bool _disposed = false;
        private int _revision;

        public ExtensionCollection() {
        }

        public ExtensionCollection(IExtensionCollection defaults) {
            _defaults = defaults;
        }

        public object GetInterface() {
            return GetInterface(null);
        }

        public object GetInterface(Type type) {
            var extension = (object)null;
            var actualType = (Type)null;

            if(_bytype.TryGetValue(type, out extension)) {
                return extension;
            }               

            if(_byname.TryGetValue(type.FullName, out actualType)) {
                if(_bytype.TryGetValue(actualType, out extension)) {
                    if(type.IsInstanceOfType(extension)) {
                        return extension;
                    }
                    return null;
                }
            }

            if(null != _defaults && _defaults.TryGetValue(type, out extension)) {
                return extension;
            }

            return null;
        }

        private void SetInterface(Type type, object extension) {
            if(type == null) {
                throw new ArgumentNullException("type");
            }

            if(extension == null) {
                throw new ArgumentNullException("extension");
            }

            lock (_sync) {
                var priorExtensionType = (Type)null;
                
                if(_byname.TryGetValue(type.FullName, out priorExtensionType)) {
                    if(priorExtensionType == type) {
                        _bytype[type] = extension;
                    } else {
                        _byname[type.FullName] = type;
                        _bytype.Remove(priorExtensionType);
                        _bytype.Add(type, extension);
                    }
                } else {
                    _byname.Add(type.FullName, type);
                    _bytype.Add(type, extension);
                }

                Interlocked.Increment(ref _revision);
            }
        }

        public virtual int Revision {
            get { return _revision; }
        }
        
        protected virtual void Dispose(bool disposing) {
            if(!_disposed) {
                if(disposing) {
                }
                _disposed = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }        

        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator() {
            return _bytype.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<Type, object> item) {
            SetInterface(item.Key, item.Value);
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<Type, object> item) {
            var value = (object)null;
            return TryGetValue(item.Key, out value) && Equals(item.Value, value);
        }

        public void CopyTo(KeyValuePair<Type, object>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<Type, object> item) {
            return Contains(item) && Remove(item.Key);
        }

        public int Count {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool ContainsKey(Type key) {
            return GetInterface(key) != null;
        }

        public void Add(Type key, object value) {
            if(ContainsKey(key)) {
                throw new ArgumentException();
            }

            SetInterface(key, value);
        }

        public bool Remove(Type key) {
            var priorExtensionType = (Type)null;

            if(_byname.TryGetValue(key.FullName, out priorExtensionType)) {
                _byname.Remove(key.FullName);
                _bytype.Remove(priorExtensionType);
                Interlocked.Increment(ref _revision);
                return true;
            }

            return false;
        }

        public bool TryGetValue(Type key, out object value) {
            value = GetInterface(key);
            return value != null;
        }

        public object this[Type key] {
            get { return GetInterface(key); }
            set { SetInterface(key, value); }
        }

        public ICollection<Type> Keys {
            get { return _bytype.Keys; }
        }

        public ICollection<object> Values {
            get { return _bytype.Values; }
        }

        private class TypeComparer : IEqualityComparer<Type> {
            public static TypeComparer Default { get; } = new TypeComparer();

            public bool Equals(Type x, Type y) {
                return (x.Equals(y)) || (x.IsAssignableFrom(y)) || (y.IsAssignableFrom(x));
            }

            public int GetHashCode(Type obj) {
                return 0;
            }
        }
    }
}
