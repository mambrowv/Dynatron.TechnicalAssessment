namespace Dynatron.Shared
{
    public abstract class EntityBase
    {
        public EntityBase()
        {
            CreatedDateTime = DateTime.UtcNow;
        }

        public DateTimeOffset UpdatedDateTime { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }

        public List<DomainEventBase> DomainEvents = new List<DomainEventBase>();
    }
}
