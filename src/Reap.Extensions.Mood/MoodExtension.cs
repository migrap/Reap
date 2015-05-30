namespace Reap.Extensions.Mood {
    public class MoodExtension : IMoodExtension {
        public MoodExtension() {
        }

        public MoodExtension(string name, string value) {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public Culture Culture { get; set; } = Culture.Current;
    }
}