using System;
using System.Diagnostics;
using System.Globalization;

namespace Reap.Extensions.Mood {
    [DebuggerDisplay("{DebuggerDisplay()}")]
    public sealed partial class Mood : IEquatable<Mood> {
        private readonly string _value;

        public Mood(string value) {
            _value = value;
        }

        public Mood(Mood value) {
            _value = value._value;
        }

        public static implicit operator string (Mood value) {
            return value._value;
        }

        private string DebuggerDisplay() {
            return _value;
        }

        public override int GetHashCode() {
            return _value.GetHashCode();
        }

        public bool Equals(Mood other) {
            if(ReferenceEquals(null, other)) {
                return false;
            }
            if(ReferenceEquals(this, other)) {
                return true;
            }

            return string.Equals(this, other, StringComparison.OrdinalIgnoreCase);
        }
    }

    public sealed partial class Mood {
        public static Mood Afraid { get; } = new Mood("afraid");
        public static Mood Amazed { get; } = new Mood("amazed");
        public static Mood Angry { get; } = new Mood("angry");
        public static Mood Amorous { get; } = new Mood("amorous");
        public static Mood Annoyed { get; } = new Mood("annoyed");
        public static Mood Anxious { get; } = new Mood("anxious");
        public static Mood Aroused { get; } = new Mood("aroused");
        public static Mood Ashamed { get; } = new Mood("ashamed");
        public static Mood Bored { get; } = new Mood("bored");
        public static Mood Brave { get; } = new Mood("brave");
        public static Mood Calm { get; } = new Mood("calm");
        public static Mood Cautious { get; } = new Mood("cautious");
        public static Mood Cold { get; } = new Mood("cold");
        public static Mood Confident { get; } = new Mood("confident");
        public static Mood Confused { get; } = new Mood("confused");
        public static Mood Contemplative { get; } = new Mood("contemplative");
        public static Mood Contented { get; } = new Mood("contented");
        public static Mood Cranky { get; } = new Mood("cranky");
        public static Mood Crazy { get; } = new Mood("crazy");
        public static Mood Creative { get; } = new Mood("creative");
        public static Mood Curious { get; } = new Mood("curious");
        public static Mood Dejected { get; } = new Mood("dejected");
        public static Mood Depressed { get; } = new Mood("depressed");
        public static Mood Disappointed { get; } = new Mood("disappointed");
        public static Mood Disgusted { get; } = new Mood("disgusted");
        public static Mood Dismayed { get; } = new Mood("dismayed");
        public static Mood Distracted { get; } = new Mood("distracted");
        public static Mood Embarrassed { get; } = new Mood("embarrassed");
        public static Mood Envious { get; } = new Mood("envious");
        public static Mood Excited { get; } = new Mood("excited");
        public static Mood Flirtatious { get; } = new Mood("flirtatious");
        public static Mood Frustrated { get; } = new Mood("frustrated");
        public static Mood Grumpy { get; } = new Mood("grumpy");
        public static Mood Guilty { get; } = new Mood("guilty");
        public static Mood Happy { get; } = new Mood("happy");
        public static Mood Hopeful { get; } = new Mood("hopeful");
        public static Mood Hot { get; } = new Mood("hot");
        public static Mood Humbled { get; } = new Mood("humbled");
        public static Mood Humiliated { get; } = new Mood("humiliated");
        public static Mood Hungry { get; } = new Mood("hungry");
        public static Mood Hurt { get; } = new Mood("hurt");
        public static Mood Impressed { get; } = new Mood("impressed");
        public static Mood Indignant { get; } = new Mood("indignant");
        public static Mood Interested { get; } = new Mood("interested");
        public static Mood Intoxicated { get; } = new Mood("intoxicated");
        public static Mood Invincible { get; } = new Mood("invincible");
        public static Mood Jealous { get; } = new Mood("jealous");
        public static Mood Lonely { get; } = new Mood("lonely");
        public static Mood Lucky { get; } = new Mood("lucky");
        public static Mood Mean { get; } = new Mood("mean");
        public static Mood Moody { get; } = new Mood("moody");
        public static Mood Nervous { get; } = new Mood("nervous");
        public static Mood Neutral { get; } = new Mood("neutral");
        public static Mood Offended { get; } = new Mood("offended");
        public static Mood Outraged { get; } = new Mood("outraged");
        public static Mood Playful { get; } = new Mood("playful");
        public static Mood Proud { get; } = new Mood("proud");
        public static Mood Relaxed { get; } = new Mood("relaxed");
        public static Mood Relieved { get; } = new Mood("relieved");
        public static Mood Remorseful { get; } = new Mood("remorseful");
        public static Mood Restless { get; } = new Mood("restless");
        public static Mood Sad { get; } = new Mood("sad");
        public static Mood Sarcastic { get; } = new Mood("sarcastic");
        public static Mood Serious { get; } = new Mood("serious");
        public static Mood Shocked { get; } = new Mood("shocked");
        public static Mood Shy { get; } = new Mood("shy");
        public static Mood Sick { get; } = new Mood("sick");
        public static Mood Sleepy { get; } = new Mood("sleepy");
        public static Mood Spontaneous { get; } = new Mood("spontaneous");
        public static Mood Stressed { get; } = new Mood("stressed");
        public static Mood Strong { get; } = new Mood("strong");
        public static Mood Surprised { get; } = new Mood("surprised");
        public static Mood Thankful { get; } = new Mood("thankful");
        public static Mood Thirsty { get; } = new Mood("thirsty");
        public static Mood Tired { get; } = new Mood("tired");
        public static Mood Undefined { get; } = new Mood("undefined");
        public static Mood Weak { get; } = new Mood("weak");
        public static Mood Worried { get; } = new Mood("worried");
    }

#if MGP_LATER
    public class Mood : IEquatable<Mood> {
        public Mood() {
        }

        public Mood(string id, Language display) {
            Id = id;
            Display = display;
        }

        public Mood(Mood value) {
            Id = value.Id;
            Display = value.Display;
        }

        //public Mood(VerbExtensionDelegate verb)
        //    : this(verb()()) {
        //}

        public string Id { get; }

        public Language Display { get; }

        public override string ToString() {
            return string.Format("{0}@{1}", Id, Display);
        }

        public override int GetHashCode() {
            return ToString().GetHashCode();
        }

        public bool Equals(Mood other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }

            return Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != this.GetType()) {
                return false;
            }

            return Equals((Mood)obj);
        }
    }
#endif
}