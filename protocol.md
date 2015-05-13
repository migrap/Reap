# Reactive Extensible Application Protocol

Reactive Extensible Application Protocol (Reap) is designed to allow applications to communicate using a collection of extensions. Protocols often limit what and how you wish to define messages. HTTP is a great example of this. It's amazing how much we've been able to do with POST, PUT, GET, and DELETE. Developers shouldn't have to work around protocol symantics when they want to add exisiting functionality to there applications. If there's a new feature you want to add to your application and an existing extension doesn't exist, then define a new extension and off you go.

Reap is a tag value based protocol with defined symantics.

### Extensions

Reap implementation should support the following extensions

|Name                |Description         |
|--------------------|--------------------|
|headers             |dictionary of an array of strings|
|version             |version or protocol |
|authorization       |existing authorization mechanism such as bearer token|
|content             |content of message |
|status              |status of message |
|encrypted           |list of extensions who's contents are encrypted. If extension exists but empty then every extension is encrypted|


### Json Example

```json
{
  "version":"semver",
  "headers":{
    "host": "computer.domain.com"
  }
  "authorization": {
      "token":"Bearer abcdef0123456789"
  },
  "encrypted":["secret"],
  "secret":"0xABCDEF0123456789"
}
```

```csharp
class Program {
  static void Main(string[] args){
    var message = new Message();
    var authorization = message.Extension(x=>x.Authorization);
    authorization.Token = "Bearer abcdef0123456789";
  }
}

// syntactic sugar
public static partial class MessageExtensions {
  public static IAuthorizationExtension Extension(this Message message, Func<IMessageExtension, Func<ExtensionReference<IAuthorizationExtension>>> callback) {
      return message.Extension<IAuthorizationExtension>();
  }

  public static ExtensionReference<IAuthorizationExtension> Authorization(this IMessageExtension extension) {
      return ExtensionReference<IAuthorizationExtension>.Default;
  }
}
```
