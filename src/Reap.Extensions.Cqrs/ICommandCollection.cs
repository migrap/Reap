namespace Reap.Extensions.Cqrs {
    public interface ICommandCollection {
        void Add(ICommand command);
    }
}
