using System.Globalization;

namespace Reap.Extensions.Mood {
    public interface IMoodExtension {
        string Name { get; set; }
        string Value { get; set; }
        Culture Culture { get; set; }
    }
}