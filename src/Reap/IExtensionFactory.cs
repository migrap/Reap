using System;

namespace Reap {
    public interface IExtensionFactory {
        object Create(Type type);
    }
}
