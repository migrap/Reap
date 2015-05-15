using System;

namespace Reap.Newtonsoft.Json {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class NonExtensionAttribute : Attribute {
    }
}
