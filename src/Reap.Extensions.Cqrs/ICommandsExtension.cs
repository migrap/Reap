namespace Reap.Extensions.Cqrs {
    public interface ICommandsExtension {
        ICommandCollection Commands { get; }
    }
}