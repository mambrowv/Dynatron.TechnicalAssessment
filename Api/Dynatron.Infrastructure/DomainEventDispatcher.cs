using Dynatron.Shared;

namespace Dynatron.Infrastructure
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        public Task Dispatch(DomainEventBase domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
