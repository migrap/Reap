#if MGP_NEXT
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Reap {
    public class Message : IExtensible<Message> {
        private IExtensionCollection<Message> _extensions;

        public Message() {
            _extensions = new ExtensionCollection<Message>(this);
        }

        public IExtensionCollection<Message> Extensions {
            get { return _extensions; }
        }
    }

    public interface IExtensionCollectionExtension { } 
    public static class ExtensionCollectionExtensions {
        public static bool Contains(this IExtensionCollection<Message> source, params Func<IExtensionCollectionExtension, Func<Type>>[] types) {
            return !types.Any(type => !source.Any(x => type(null)().IsAssignableFrom(x.GetType())));
        }
    }

    public delegate Func<T> ExtensionSelector<T>(Message message);

    public static class MessageExtensions {
        private static T Extension<T>(this Message message, T extension, Action<T> callback) {
            if(callback != null) {
                callback(extension);
            }
            return extension;
        }

        public static IMoodExtension Extension(this Message message, ExtensionSelector<IMoodExtension> extension, Action<IMoodExtension> callback = null) {
            return message.Extension<IMoodExtension>(extension, callback);
        }

        public static IMoodExtension Mood(this Message message) {
            return message.Extensions.GetOrAdd<IMoodExtension>(() => new MoodExtension());
        }

        public static ITimeExtension Extension(this Message message, ExtensionSelector<ITimeExtension> extension, Action<ITimeExtension> callback = null) {
            return message.Extension<ITimeExtension>(extension, callback);
        }

        public static ITimeExtension Time(this Message message) {
            return message.Extensions.GetOrAdd<ITimeExtension>(() => new TimeExtension());
        }

        public static Type Time(this IExtensionCollectionExtension extension) {
            return typeof(ITimeExtension);
        }

        public static T Extension<T>(this Message message, ExtensionSelector<T> extension, Action<T> callback) {
            return message.Extension(extension(message)(), callback);
        }
    }

    public interface ITimeExtension : IExtension<Message> {
        DateTimeOffset Time { get; set; }
    }

    public class TimeExtension : ITimeExtension {
        private Message _message;

        public DateTimeOffset Time { get; set; }

        void IExtension<Message>.Attach(Message owner) {
            _message = owner;
        }

        void IExtension<Message>.Detach(Message owner) {
            if(_message == owner) {
                _message = null;
            }
        }
    }

    public interface IMoodExtension : IExtension<Message> {
        string Mood { get; set; }
    }

    public class MoodExtension : IMoodExtension {
        private Message _message;

        public string Mood { get; set; }

        void IExtension<Message>.Attach(Message owner) {
            _message = owner;
        }

        void IExtension<Message>.Detach(Message owner) {
            if(_message == owner) {
                _message = null;
            }
        }
    }

    public interface IExtensionCollection<T> : ICollection<IExtension<T>> where T : IExtensible<T> {
        int Revision { get; }
        TExtension GetOrAdd<TExtension>(Func<TExtension> creator) where TExtension : IExtension<T>;
    }

    public interface IExtension<T> where T : IExtensible<T> {
        void Attach(T owner);
        void Detach(T owner);
    }

    public interface IExtensible<T> where T : IExtensible<T> {
        IExtensionCollection<T> Extensions { get; }
    }

    public sealed class ExtensionCollection<T> : IExtensionCollection<T> where T : IExtensible<T> {
        private readonly List<IExtension<T>> _collection = new List<IExtension<T>>();
        private readonly object _sync = new Object();
        private readonly T _owner;
        private int _revision;

        public ExtensionCollection(T owner) {
            _owner = owner;
        }

        public int Count => _collection.Count;

        public bool IsReadOnly => false;

        public int Revision => _revision;

        public void Add(IExtension<T> item) => Add(_sync, _owner, _collection, item, ref _revision);

        public void Clear() => _collection.Clear();

        public bool Contains(IExtension<T> item) => _collection.Contains(item);

        public void CopyTo(IExtension<T>[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

        public IEnumerator<IExtension<T>> GetEnumerator() => _collection.GetEnumerator();

        public bool Remove(IExtension<T> item) => Remove(_sync, _owner, _collection, item, ref _revision);

        public TExtension GetOrAdd<TExtension>(Func<TExtension> creator) where TExtension : IExtension<T> { 
            var item = _collection.Find(x => typeof(TExtension).IsAssignableFrom(x.GetType()));

            if(item == null) {
                item = creator();
                Add(item);
            }

            return (TExtension)item;
        }      

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static void Add(object sync, T owner, ICollection<IExtension<T>> collection, IExtension<T> item, ref int revision) {
            lock (sync) {
                item.Attach(owner);
                collection.Add(item);
                Interlocked.Increment(ref revision);
            }
        }

        private static bool Remove(object sync, T owner, ICollection<IExtension<T>> collection, IExtension<T> item, ref int revision) {
            lock (sync) {
                item.Detach(owner);
                return collection.Remove(item);
                Interlocked.Increment(ref revision);
            }
        }
    }

    public struct ExtensionReference<T> where T : IExtension<Message> {
        private T _extension;
        private int _revision;

        private ExtensionReference(T extension, int revision) {
            _extension = extension;
            _revision = revision;
        }

        public static ExtensionReference<T> Default = new ExtensionReference<T>(default(T), -1);

        public T Fetch(IExtensionCollection<Message> extensions) {
            if(_revision == extensions.Revision) {
                return _extension;
            }

            _extension = extensions.GetOrAdd(() => default(T));

            Interlocked.Exchange(ref _revision, extensions.Revision);

            return _extension;
        }
    }
}
#endif