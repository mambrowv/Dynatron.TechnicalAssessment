namespace Dynatron.Shared
{
    public abstract class DomainEventBase
    {
        public DateTime DateTimeOccured { get; } = DateTime.UtcNow;
    }
}
