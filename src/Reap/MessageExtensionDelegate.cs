using System;

namespace Reap {
    public delegate Func<ExtensionReference<T>> MessageExtensionDelegate<T>(IMessageExtension extension = null);
}
