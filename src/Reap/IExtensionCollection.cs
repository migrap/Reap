using System;
using System.Collections.Generic;

namespace Reap {
    public interface IExtensionCollection : IDictionary<Type, object>, IDisposable {
        int Revision { get; }
    }
}
