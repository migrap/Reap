using System;
using Newtonsoft.Json;
using Reap;
using Reap.Extensions.Authentication;
using Reap.Extensions.Authorization;
using Reap.Extensions.Claims;
using Reap.Extensions.Headers;
using Reap.Extensions.Mood;
using Reap.Extensions.Resource;
using Reap.Extensions.Objects;
using Reap.Newtonsoft.Json;

namespace Sandbox {
    public class Program {
        public void Main(string[] args) {
            var settings = new MessageSerializerSettings();
            settings.Converters.Add(new MessageConverter());
            settings.Converters.Add(new VersionConverter());

            var message = new Message();

            var authorization = message.Extension(x => x.Authorization, x => {
            });

            var headers = message.Extension(x => x.Headers, x => {
                x.Headers.Add("", "");
            });

            var claims = message.Extension(x => x.Claims, x => {
                x.Claims.Add("issuer", "type", "value");
            });

            var authentication = message.Extension(x => x.Authentication, x => {
                x.Token = "Bearer 0xABCDEF0123456789";
            });

            var version = message.Extension(x => x.Version, x => {
                x.Version = "1.0.0";
            });

            var mood = message.Extension(x => x.Mood, x => {
                x.Mood = Mood.Happy;
            });            

            var uri = message.Extension(x => x.Resource, x => {
                x.Resource = "http://www.google.com";
            });

            var obj = message.Extension(x => x.Object, x => {
                x.Class = "Person";
                x.Properties = new {
                    name = "Michael",
                    age = 10,
                };
                x.Links.Add(Link.About("http://www.about.com"));
                x.Title = "Person Description";
            });

            var urn = message.Extension(x => x.Resource, "urn:here@home.com");

            var urh = message.Extension(x => x.Resource, x => x.Home);

            var json = (string)null;
            json = JsonConvert.SerializeObject(message, settings);

            var egassem = JsonConvert.DeserializeObject<Message>(json, settings);
            authorization = egassem.Extension(x => x.Authorization);
            version = egassem.Extension(x => x.Version);

            //var contains = egassem.Extensions.Contains(x => x.Authentication, x => x.Jello);
        }
    }

    public class VersionConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return typeof(IVersionExtension).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if(reader.TokenType == JsonToken.String) {
                return new VersionExtension { Version = reader.Value.ToString() };
            }
            throw new InvalidOperationException("VersionConverter.ReadJson");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            WriteJson(writer, value as IVersionExtension, serializer);
        }

        private void WriteJson(JsonWriter writer, IVersionExtension value, JsonSerializer serializer) {
            serializer.Serialize(writer, value.Version);
        }
    }

    public interface IVersionExtension {
        string Version { get; set; }
    }

    public class VersionExtension : IVersionExtension {
        public string Version { get; set; }
    }

    public static partial class VersionExtensions {
        public static IVersionExtension Extension(this Message message, ExtensionSelector<IVersionExtension> extension, Action<IVersionExtension> callback = null) {
            return message.Extension<IVersionExtension>(extension, callback);
        }

        public static IVersionExtension Version(this Message message) {
            return message.Extension<IVersionExtension>();
        }
    }
}