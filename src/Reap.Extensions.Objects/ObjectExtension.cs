namespace Reap.Extensions.Objects {
    public class ObjectExtension : IObjectExtension {
        public Class Class { get; set; } = new Class();

        public object Properties { get; set; } = new object();

        public ObjectCollection Objects { get; set; } = new ObjectCollection();

        public Links Links { get; set; } = new Links();

        public string Title { get; set; }
    }
}