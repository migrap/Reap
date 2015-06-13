using System;
using Reap;
using Reap.Newtonsoft.Json;

namespace Sandbox {
    class Program {
        static void Main(string[] args) {
            var message = new Message();            
            var mood = message.Extension(x => x.Mood, x => {
                x.Mood = "Happy";
                x.Degree = 0.923;
                x.Reason = "Because";
            });

            mood.Mood = "Melaonchol";

            mood = message.Extension(x => x.Mood, x => {
                x.Mood = "Sad";
            });

            var settings = new MessageSerializerSettings();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(message, settings);
        }
    }

    public static partial class Extensions {
        public static HeadersExtension Extension(this Message message, ExtensionSelector<Message, HeadersExtension> extension, Action<HeadersExtension> callback = null) {
            return message.Extensions(extension, callback);            
        }

        public static HeadersExtension Headers(this Message message) {
            return new HeadersExtension();
        }

        public static MoodExtension Extension(this Message message, ExtensionSelector<Message, MoodExtension> extension, Action<MoodExtension> callback = null) {
            return message.Extensions(extension, callback);            
        }

        public static MoodExtension Mood(this Message message) {
            return new MoodExtension(message);
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
        public string Reason { get; set; }
        public double Degree { get; set; }
    }
}