using System;

namespace Reap {
    public delegate Func<T> ExtensionSelector<T>(Message message);
}