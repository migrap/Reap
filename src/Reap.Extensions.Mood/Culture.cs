using System;
using System.Diagnostics;
using System.Globalization;

namespace Reap.Extensions.Mood {

    [DebuggerDisplay("{DebuggerDisplay()}")]
    public sealed partial class Culture : IEquatable<Culture> {
        private readonly CultureInfo _value;

        public Culture(string name) {
            _value = CultureInfo.GetCultureInfoByIetfLanguageTag(name);
        }

        public Culture(CultureInfo value) {
            _value = value;
        }

        public Culture(Culture value) {
            _value = value._value;
        }

        public static implicit operator CultureInfo(Culture value) {
            return value._value;
        }

        private string DebuggerDisplay() {
            return _value.ToString();
        }

        public override int GetHashCode() {
            return _value.GetHashCode();
        }

        public bool Equals(Culture other) {
            if(ReferenceEquals(null, other)) {
                return false;
            }
            if(ReferenceEquals(this, other)) {
                return true;
            }

            return _value.Equals(other._value);
        }
    }

    public sealed partial class Culture {
        public static Culture Current => new Culture(CultureInfo.CurrentCulture);
    }
}