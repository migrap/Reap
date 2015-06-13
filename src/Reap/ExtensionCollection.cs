using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Migrap.Framework.Extensions {
    public class ExtensionCollection<T> : IExtensionCollection<T> where T : IExtensible<T> {
        private readonly IExtensionCollection<T> _defaults;
        private readonly Dictionary<Type, IExtension<T>> _bytype = new Dictionary<Type, IExtension<T>>(TypeComparer.Default);
        private readonly Dictionary<string, Type> _byname = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        private readonly object _sync = new Object();
        private bool _disposed = false;
        private int _revision;

        public ExtensionCollection() {
        }

        public ExtensionCollection(IExtensionCollection<T> defaults) {
            _defaults = defaults;
        }

        public object GetInterface() {
            return GetInterface(null);
        }

        public IExtension<T> GetInterface(Type type) {
            var extension = (IExtension<T>)null;
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

        private void SetInterface(Type type, IExtension<T> extension) {
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

        public IEnumerator<KeyValuePair<Type, IExtension<T>>> GetEnumerator() {
            return _bytype.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<Type, IExtension<T>> item) {
            SetInterface(item.Key, item.Value);
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<Type, IExtension<T>> item) {
            var value = (IExtension<T>)null;
            return TryGetValue(item.Key, out value) && Equals(item.Value, value);
        }

        public void CopyTo(KeyValuePair<Type, IExtension<T>>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<Type, IExtension<T>> item) {
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

        public void Add(Type key, IExtension<T> value) {
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

        public bool TryGetValue(Type key, out IExtension<T> value) {
            value = GetInterface(key);
            return value != null;
        }

        public IExtension<T> this[Type key] {
            get { return GetInterface(key); }
            set { SetInterface(key, value); }
        }

        public ICollection<Type> Keys {
            get { return _bytype.Keys; }
        }

        public ICollection<IExtension<T>> Values {
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