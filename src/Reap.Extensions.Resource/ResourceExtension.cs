namespace Reap.Extensions.Resource {
    public class ResourceExtension : IResourceExtension {
        public ResourceExtension() {
        }

        public ResourceExtension(string resource) {
            Resource = resource;
        }

        public string Resource { get; set; }
    }
}