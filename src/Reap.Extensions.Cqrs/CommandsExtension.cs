namespace Reap.Extensions.Cqrs {
    public class CommandsExtension : ICommandsExtension {
        public ICommandCollection Commands { get; } = new CommandCollection();
    }
}