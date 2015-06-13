using System;
using Migrap.Framework.Extensions;

namespace Reap {
    public class Message : IExtensible<Message> {
        public IExtensionCollection<Message> Extensions => new ExtensionCollection<Message>();

        public virtual IExtension<Message> Extension(Type type, IExtension<Message> extension) {
            Extensions[type] = extension;
            return extension;
        }

        public virtual T Extension<T>(T extension) where T : IExtension<Message> {
            return (T)Extension(typeof(T), extension);
        }
    }
}