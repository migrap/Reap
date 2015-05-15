using System;

namespace Reap.Newtonsoft.Json {
    public class Extension {
        public Extension(string name, Type type) {
            Name = name;
            Type = type;
        }

        public string Name { get; }

        public Type Type { get; }
    }
}