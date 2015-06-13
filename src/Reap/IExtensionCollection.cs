using System;
using System.Collections.Generic;

namespace Migrap.Framework.Extensions {
    public interface IExtensionCollection<T> : IDisposable, IDictionary<Type, IExtension<T>> where T : IExtensible<T> {
        int Revision { get; }
    }
}