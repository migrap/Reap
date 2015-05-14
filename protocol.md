# Reactive Extensible Application Protocol
__Current version: developmental__

Reactive Extensible Application Protocol (Reap) is designed to allow applications to communicate using a collection of extensions. Basically Reap is a tag value protocol with syntactic sugar.

Reap has been carefully designed to sit nicely in between the wire and the application logic. Packets can be taken off the wire and serialized into objects used by the application and then put back on the wire.

Applications create multiple endpoints to handle Reap messages being serialized in different formats. Json for debugging and a compatct binary format for production or contrained networks.

### Inspiration
I've pulled ideas from lots of different protocols I've looked at.
HTTP
REST
Siren
Lime
Collection+Json
HAL
Protocol Buffers
CoAP
ASN.1
Netflow
TLV (Tag-Length-Value)

A big inspiration that's not a protocol perse is CQRS/ES and DDD.

CQRS and Hypermedia are examples of where Reap comes in.

In all my googleing I couldn't find a CQRS protocol based on hypermedia ideas (or maybe it's a hypermeida based CQRS protocol). Hypermedia seems to be geared more towards reads then writes but I might be wrong. Regardless... hypermedia seems an ideal fit for the Query and Read portion of CQRS. Where does that leave us with the Command portion of CQRS? Are we to use a different protocol. If we go over HTTP, the four major methods (GET, POST, PUT, and DELETE) seem limiting leaving us with having to hack our way around HTTP. It's all a bit message.

Using Reap you can define the different extensions to support CQRS. Want hypermedia bits... great... define the extension and add it in. Want to supply headers to both commands and queries then share that extension.

Applications just validate that they have all the extensions needed to process a message do.

### Siren
Siren was a big inspiration. I really liked the actions section. But not for what it was designed for. Actions are meant to suppoly actions for a user to take. They define how a user/application can query for additionaly resources. I had the idea of actions going the other way. Here's a request, but here are some actions you'll need to take in order to complete that action.

If a request was made to change a user's profile picture, generally the user would have to upload that image along with the request. If the user's on a contrained network that could be taxing, especially if the image is large. But if you sent an action along telling the server where and how to download that image then the server can do the heavy lifting. Uploading an image might be a trite example but I hope it get's you thinking about the possibilities of what you can do with Reap.

Heck... you can define all of Siren, and most Json based hypermeida protocols using Reap.

### Why?

I couldn't find a protocol that met my needs... or maybe I'm being to needy. I've found merit in many of the protocols I've looked at and then there are things I don't like about them. This protocol has this feature but is missing that feature.

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

Most protocols I've reviewed I find a lot I like about them.
I like Message based protocols for there asynchronicity

Protocols can often limit developers experience resulting in work arounds in an effort just to get there jobs done. HTTP is a great example of this. Developers are limited to four major methods: POST, PUT, GET, and DELETE. And look at all we've accomplished using just those four methods!

Developers shouldn't have to work around protocol symantics to add new functionality to there applications.

We crave a rich experience in our programming languages, and our IDE/text editors.

Yet we don't look for such experiences with the application protocol we build our application(s) on.

If there's a new feature you want to add to your application and an existing extension doesn't suit your needs, then define a new extension and off you go.

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
