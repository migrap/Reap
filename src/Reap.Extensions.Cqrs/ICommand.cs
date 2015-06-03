namespace Reap.Extensions.Cqrs {
    public interface ICommand {
        string Uri { get; set; }
        string Name { get; set; }
        object Properties { get; set; }
    }
}