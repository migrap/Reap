using System;

namespace Reap.Extensions.Objects {
    public class Href {
        private Uri _value;

        public Href(string uriString) {
            _value = new Uri(uriString, UriKind.RelativeOrAbsolute);
        }

        public Href(string uriString, UriKind uriKind) { 
            _value = new Uri(uriString, uriKind);
        }

        public Href(Uri baseUri, string relativeUri) {
            _value = new Uri(baseUri, relativeUri);
        }

        public Href(Uri baseUri, Uri relativeUri) {
            _value = new Uri(baseUri, relativeUri);
        }

        public static implicit operator string(Href value) {
            return value.ToString();
        }

        public static implicit operator Href(string value) {
            return new Href(value);
        }

        public static implicit operator Uri(Href value) {
            return value._value;
        }

        public Href(Uri uri) {
            _value = uri;
        }
    }
}
