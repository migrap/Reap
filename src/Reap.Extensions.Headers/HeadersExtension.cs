namespace Reap.Extensions.Headers {
    public class HeadersExtension : IHeadersExtension {
        public IHeaderCollection Headers { get; } = new HeaderCollection();
    }
}