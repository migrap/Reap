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

            var mood = message.Extension(x => x.Mood, x => {
                x.Mood = Mood.Happy;
            });

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

            //var cmd = message.Extension(x => x.Command, x => {
            //    x = x.CreateAccount(name: "Michael");
            //});

            //message.xExtension(x => x.Command, x => x.Command = Account.Create(create => {
            //    create.Name = Guid.Empty.ToString();
            //}));

            var owner = Guid.NewGuid().ToString();
            var uuid = Guid.NewGuid().ToString();

            message.Extension(x => x.Command, x => {
                x.Uuid = "10001";
                x.Uri = "/account";
                x.Name = "create";
                x.Type = "application/vnd.bacnking+json";
                x.Data = new { owner = owner, name = "Family Checking" };
            });

            //message.Extension(x => x.Event, x => {
            //    x.Path = "/account";
            //    x.Name = "created";
            //    x.Type = "application/vnd.bacnking+json";
            //    x.Body = new {
            //        owner = owner,
            //        uuid = uuid,
            //        name = "Family Checking",
            //        date = DateTimeOffset.UtcNow,
            //        balance = 0.0
            //    };
            //});

            var json = (string)null;
            json = JsonConvert.SerializeObject(message, settings);

            //var egassem = JsonConvert.DeserializeObject<Message>(json, settings);
            //authorization = egassem.Extension(x => x.Authorization);
            //version = egassem.Extension(x => x.Version);

            //var contains = egassem.Extensions.Contains(x => x.Authentication, x => x.Jello);
        }
    }

    public interface ICommandExtension {
        string Uuid { get; set; }
        string Uri { get; set; }
        string Name { get; set; }
        string Type { get; set; }
        object Data { get; set; }
    }

    public static class CommnadExtensions {
        public static ICommandExtension Extension(this Message message, ExtensionSelector<ICommandExtension> extension, Action<ICommandExtension> callback = null) {
            return message.Extension<ICommandExtension>(extension, callback);
        }

        public static ICommandExtension Command(this Message message) {
            return message.Extension<ICommandExtension>();
        }
    }

    //public interface IEventExtension {
    //    string Path { get; set; }
    //    string Name { get; set; }
    //    string Type { get; set; }
    //    object Body { get; set; }
    //}

    //public static class EventExtensions {
    //    public static IEventExtension Extension(this Message message, ExtensionSelector<IEventExtension> extension, Action<IEventExtension> callback = null) {
    //        return message.Extension<IEventExtension>(extension, callback);
    //    }

    //    public static IEventExtension Event(this Message message) {
    //        return message.Extension<IEventExtension>();
    //    }
    //}
}