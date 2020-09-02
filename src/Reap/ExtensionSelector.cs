using System;

namespace Reap {
    public delegate Func<TResult> ExtensionSelector<TSource, TResult>(TSource value) where TSource : IExtensible<TSource> where TResult : IExtension<TSource>;
}