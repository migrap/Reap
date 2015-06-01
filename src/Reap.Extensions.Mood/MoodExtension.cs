namespace Reap.Extensions.Mood {
    public class MoodExtension : IMoodExtension {
        public MoodExtension() {
        }

        public MoodExtension(string mood) {
            Mood = new Mood(mood);
        }

        public MoodExtension(Mood mood) {
            Mood = mood;
        }

        public Mood Mood { get; set; }
    }
}