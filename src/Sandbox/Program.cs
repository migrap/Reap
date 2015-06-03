using System;
using Newtonsoft.Json;
using Reap;
using Reap.Extensions.Authentication;
using Reap.Extensions.Authorization;
using Reap.Extensions.Claims;
using Reap.Extensions.Headers;
using Reap.Extensions.Mood;
//using Reap.Extensions.Resource;
using Reap.Extensions.Objects;
using Reap.Newtonsoft.Json;

namespace Sandbox {
    public class Program {
        public void Main(string[] args) {
            var settings = new MessageSerializerSettings();
            settings.Converters.Add(new MessageConverter());
            settings.Converters.Add(new VersionConverter());

            var message = new Message();

            //var authorization = message.Extension(x => x.Authorization, x => {
            //});

            //var headers = message.Extension(x => x.Headers, x => {
            //    x.Headers.Add("", "");
            //});

            //var claims = message.Extension(x => x.Claims, x => {
            //    x.Claims.Add("issuer", "type", "value");
            //});

            //var authentication = message.Extension(x => x.Authentication, x => {
            //    x.Token = "Bearer 0xABCDEF0123456789";
            //});

            //var version = message.Extension(x => x.Version, x => {
            //    x.Version = "1.0.0";
            //});

            //var mood = message.Extension(x => x.Mood, x => {
            //    x.Mood = Mood.Happy;
            //});            

            //var uri = message.Extension(x => x.Resource, x => {
            //    x.Resource = "http://www.google.com";
            //});

            //var obj = message.Extension(x => x.Object, x => {
            //    x.Class = "Person";
            //    x.Properties = new {
            //        name = "Michael",
            //        age = 10,
            //    };
            //    x.Links.Add(Link.About("http://www.about.com"));
            //    x.Title = "Person Description";
            //});

            //var urn = message.Extension(x => x.Resource, "urn:here@home.com");

            //var urh = message.Extension(x => x.Resource, x => x.Home);

            var cmd = message.Extension(x => x.Command, x => {
                x = x.CreateAccount(name: "Michael");
            });

            message.xExtension(x => x.Command, x => x.Account, x => x.Create(""));

            var json = (string)null;
            json = JsonConvert.SerializeObject(message, settings);

            //var egassem = JsonConvert.DeserializeObject<Message>(json, settings);
            //authorization = egassem.Extension(x => x.Authorization);
            //version = egassem.Extension(x => x.Version);

            //var contains = egassem.Extensions.Contains(x => x.Authentication, x => x.Jello);
        }
    }

    public interface ICommandExtension {

    }

    public abstract class CommandExtension : ICommandExtension {
        public CommandExtension(string uri) {
            Uri = uri;  
        }

        public string Uri { get; }
    }

    public abstract class AccountCommandExtension : CommandExtension {
        public AccountCommandExtension() : base("/account") {
        }
    }

    public class CreateAccountCommandExtension : AccountCommandExtension {
        public CreateAccountCommandExtension(string name) {
            Name = name;
        }

        public string Name { get; }
    }

    public delegate Func<T> CommandSelector<T>(ICommandExtension extension = null) where T : ICommandExtension;

    public static class CommnadExtensions {
        public static ICommandExtension Extension(this Message message, ExtensionSelector<ICommandExtension> extension, Action<ICommandExtension> callback = null) {
            return message.Extension<ICommandExtension>(extension, callback);
        }    
        
        public static ICommandExtension Command(this Message message) {
            return default(ICommandExtension);
        }

        public static ICommandExtension CreateAccount(this ICommandExtension extension, string name) {
            return new CreateAccountCommand(name);
        }

        //public static ICommandExtension xExtension(this Message message, ExtensionSelector<ICommandExtension> extension, CommandSelector<AccountCommandExtension> account, Func<AccountCommandExtension,Func<ICommandExtension>> command) {
        //    return command(account(null)());
        //}

        public static ICommandExtension xExtension(this Message message, ExtensionSelector<ICommandExtension> extension, Func<ICommandExtension, Func<AccountCommandExtension>> account, Func<AccountCommandExtension, ICommandExtension> command) {
            return message.Extension<ICommandExtension>(command(null));
        }

        public static AccountCommandExtension Account(this ICommandExtension extension) {
            return default(AccountCommandExtension);
        }

        public static CreateAccountCommandExtension Create(this AccountCommandExtension extension, string name) {
            return new CreateAccountCommandExtension(name);
        }
    }

    public class CreateAccountCommand : CommandExtension {
        public CreateAccountCommand(string name) : base("/account") {
            Name = name;
        }

        public string Name { get; }
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