using System.Collections;
using System.Collections.Generic;

namespace Reap.Extensions.Objects {
    public class Class : List<string> {
        public static implicit operator Class(string value) {
            return new Class { value };
        }
    }
}
