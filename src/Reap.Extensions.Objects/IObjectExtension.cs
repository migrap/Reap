namespace Reap.Extensions.Objects {
    public interface IObjectExtension {
        Class Class { get; set; }

        object Properties { get; set; }

        ObjectCollection Objects { get; set; }

        Links Links { get; set; }

        string Title { get; set; }
    }
}