using System;

namespace Reap.Extensions.Claims {
    public class Claim : IEquatable<Claim> {
        public Claim(string issuer, string type, string value) {
            Issuer = issuer;
            Type = type;
            Value = value;
        }

        public string Issuer { get; }

        public string Type { get; }

        public string Value { get; }

        public bool Equals(Claim other) {
            return Issuer.Equals(other.Issuer, StringComparison.OrdinalIgnoreCase)
                && Type.Equals(other.Type, StringComparison.OrdinalIgnoreCase)
                && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode() {
            return (Issuer + Type + Value).ToLower().GetHashCode() ^ 27;
        }
    }
}