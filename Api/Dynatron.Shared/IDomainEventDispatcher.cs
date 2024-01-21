namespace Dynatron.Shared
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(DomainEventBase domainEvent);
    }
}
