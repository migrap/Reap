using System.Collections.Generic;

namespace Reap.Newtonsoft.Json {
    public interface IExtensionProvider {
        IReadOnlyList<Extension> Extensions { get; }
    }
}
