using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Reap;
//using Reap.Extensions.Resource;

namespace Sandbox {
    public class Program {
        public void Main(string[] args) {
            //var settings = new MessageSerializerSettings();
            //settings.Converters.Add(new MessageConverter());
            //settings.Converters.Add(new VersionConverter());

            (new AccountService()).SendAsync(x => x.Create("Michael", "Checking"));

            var message = new Message();

            //var mood = message.Extension(x => x.Mood, x => {
            //    x.Mood = Mood.Happy;
            //});

            var owners = Enumerable.Range(0, 3).Select(x => Guid.NewGuid().ToString()).ToArray();
            var uuid = Guid.NewGuid().ToString();

            message.Extension(x => x.Command, x => {
                x.Uuid = "10001";
                x.Uri = "/account";
                x.Name = "create";
                x.Type = "application/vnd.bacnking+json";
                x.Data = new { owners = owners, name = "Family Checking" };
            });

            var json = (string)null;
            //json = JsonConvert.SerializeObject(message, settings);



            //var egassem = JsonConvert.DeserializeObject<Message>(json, settings);
            //authorization = egassem.Extension(x => x.Authorization);
            //version = egassem.Extension(x => x.Version);

            //var contains = egassem.Extensions.Contains(x => x.Authentication, x => x.Jello);
        }
    }

    public interface IAccountService {
        Task SendAsync(Message message);
    }

    public class AccountService : IAccountService {
        public Task SendAsync(Message message) {
            return Task.FromResult(1);
        }
    }

    public static class AccountServiceExtensions {
        public static void SendAsync(this IAccountService account, Expression<Action<IAccountService>> expression) {
            var mce = (MethodCallExpression)expression.Body;
            var parameters = mce.Method.GetParameters();
            var arguments = mce.Arguments.ToArray();

            var data = ToDictionary(parameters.Select(x => x.Name).Skip(1), arguments.Skip(1));

            var message = new Message();
            message.Extension(x => x.Command, x => {
                x.Uuid = Guid.NewGuid().ToString();
                x.Uri = "/account";
                x.Name = mce.Method.Name;
                x.Type = "application/vnd.bacnking+json";
                x.Data = data;
            });
            account.SendAsync(message);
        }

        public static void Create(this IAccountService account, string name, string type) {
        }

        private static Dictionary<string, object> ToDictionary(IEnumerable<string> keys, IEnumerable<object> values) {
            var k = keys.GetEnumerator();
            var v = values.GetEnumerator();
            var d = new Dictionary<string, object>();

            while(k.MoveNext()) {
                d[k.Current] = (v.MoveNext()) ? v.Current : null;
            }

            return d;
        }
    }

    public interface ICommandExtension {
        string Uuid { get; set; }
        string Uri { get; set; }
        string Name { get; set; }
        string Type { get; set; }
        object Data { get; set; }
    }

    public class CommandExtension : ICommandExtension {
        public object Data { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Uri { get; set; }

        public string Uuid { get; set; }
    }

    public static class CommnadExtensions {
        public static ICommandExtension Extension(this Message message, ExtensionSelector<ICommandExtension> extension, Action<ICommandExtension> callback = null) {
            return message.Extension<ICommandExtension>(extension, callback);
        }

        public static ICommandExtension Command(this Message message) {
            return message.Extension<ICommandExtension>(new CommandExtension());
        }
    }
}