using System;
using Reap;

namespace Sandbox {
    class Program {
        static void Main(string[] args) {
            var message = new Message();
            var headers = message.Extension(x => x.Headers);
            var mood = message.Extension(x => x.Mood, x => {
                x.Mood = "Happy";
                x.Degree = 0.923;
            });

            var contains = message.Contains(x => x.Mood, x => x.Headers);
        }
    }

    public static partial class Extensions {
        public static HeadersExtension Extension(this Message message, ExtensionSelector<Message, HeadersExtension> extension, Action<HeadersExtension> callback = null) {
            return message.Extension(extension(message)(), callback);
        }

        public static HeadersExtension Headers(this Message message) {
            return new HeadersExtension();
        }

        public static MoodExtension Extension(this Message message, ExtensionSelector<Message, MoodExtension> extension, Action<MoodExtension> callback = null) {
            return message.Extension(extension(message)(), callback);
        }

        public static MoodExtension Mood(this Message message) {
            return new MoodExtension(message);
        }
    }

    public class Message : IExtensible<Message> {
        public IExtensionCollection<Message> Extensions => new ExtensionCollection<Message>();

        public virtual IExtension<Message> Extension(Type type, IExtension<Message> extension) {
            Extensions[type] = extension;
            return extension;
        }

        public virtual T Extension<T>(T extension) where T : IExtension<Message> {
            return (T)Extension(typeof(T), extension);
        }
    }

    public class HeadersExtension : IExtension<Message> {
    }

    public class MoodExtension : IExtension<Message> {
        private readonly Message _message;

        public MoodExtension(Message message) {
            _message = message;
        }

        public string Mood { get; set; }
        public double Degree { get; set; }
    }
}