namespace Reap.Extensions.Mood {
    public class MoodExtension : IMoodExtension {
        public string Name { get; set; }
        public string Value { get; set; }
        public Culture Culture { get; set; } = Culture.Current;
    }
}