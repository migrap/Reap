using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
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

            var headers = message.Extension(x => x.Headers, x => {
                x.Name = "Michael";
                x.Count = 10;
            });

            (headers as dynamic).Age = 37;

            mood.Mood = "Melaonchol";

            mood = message.Extension(x => x.Mood, x => {
                x.Mood = "Sad";
            });

            message.Extension(x => x.Message, x => {
                x.Extension(xx => xx.Mood, xx => {
                    xx.Mood = "Happy";
                });
            });

            var json = message.Serialize(x => x.Json);
        }
    }

    public static partial class Extensions {
        public static HeadersExtension Extension(this Message message, ExtensionSelector<Message, HeadersExtension> extension, Action<dynamic> callback = null) {
            return message.Extensions(extension, callback);
        }

        public static HeadersExtension Headers(this Message message) {
            return new HeadersExtension();
        }

        public static MoodExtension Extension(this Message message, ExtensionSelector<Message, MoodExtension> extension, Action<MoodExtension> callback = null) {
            return message.Extensions(extension, callback);
        }

        public static MessageExtension Extension(this Message message, ExtensionSelector<Message, MessageExtension> extension, Action<MessageExtension> callback = null) {
            return message.Extensions(extension, callback);
        }

        public static MoodExtension Mood(this Message message) {
            return new MoodExtension(message);
        }

        public static MessageExtension Message(this Message message) {
            return new MessageExtension();
        }

        public static string Serialize(this Message message, Func<Message, Func<string>> serializer) {
            return serializer(message)();
        }

        public static byte[] Serialize(this Message message, Func<Message, Func<byte[]>> serializer) {
            return serializer(message)();
        }

        public static string Json(this Message message) {
            var settings = new MessageSerializerSettings();
            return Newtonsoft.Json.JsonConvert.SerializeObject(message, settings);
        }

        public static byte[] Buff(this Message message) {
            var settings = new MessageSerializerSettings();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(message, settings);
            return Encoding.UTF8.GetBytes(json);
        }
    }

    public class HeadersExtension : DynamicObject, IExtension<Message> {
        private ConcurrentDictionary<string, object> _headers = new ConcurrentDictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            return _headers.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value) {
            _headers.AddOrUpdate(binder.Name, value, (x, y) => value);
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames() {
            return _headers.Keys;
        }
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