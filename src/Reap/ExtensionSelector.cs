using System;

namespace Migrap.Framework.Extensions {
    public delegate Func<TResult> ExtensionSelector<TSource, TResult>(TSource value) where TSource : IExtensible<TSource> where TResult : IExtension<TSource>;
}
