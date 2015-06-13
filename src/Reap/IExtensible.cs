namespace Migrap.Framework.Extensions {
    public interface IExtensible<T> where T : IExtensible<T> {
        IExtensionCollection<T> Extensions { get; }
    }
}
