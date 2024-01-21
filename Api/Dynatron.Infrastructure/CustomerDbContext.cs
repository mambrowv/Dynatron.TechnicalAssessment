using Dynatron.Domain;
using Dynatron.Shared;
using Microsoft.EntityFrameworkCore;

namespace Dynatron.Infrastructure
{
    public sealed class CustomerDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public CustomerDbContext(DbContextOptions<CustomerDbContext> options, IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerId);

            modelBuilder.Entity<Customer>()
                .Property(c => c.FirstName)
                .IsRequired(false)
                .HasMaxLength(50);

            modelBuilder.Entity<Customer>()
                .Property(c => c.LastName)
                .IsRequired(false)
                .HasMaxLength(50);

            modelBuilder.Entity<Customer>()
                .Property(c => c.EmailAddress)
                .IsRequired(true)
                .HasMaxLength(128);

            modelBuilder.Entity<Customer>()
                .Property(c => c.CreatedDateTime)
                .IsRequired(true);

            modelBuilder.Entity<Customer>()
                .Property(c => c.UpdatedDateTime)
                .IsRequired(true);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var upsertedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach(var entity in upsertedEntities) 
            {
                if(entity.Entity is EntityBase entityBase)
                { 
                    entityBase.UpdatedDateTime = DateTime.UtcNow;

                    if(entity.State == EntityState.Added)
                        entityBase.CreatedDateTime = DateTime.UtcNow;
                }
            }

            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            if (_dispatcher == null) return result;

            var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToArray();
                entity.DomainEvents.Clear();
                foreach (var domainEvent in events)
                {
                    await _dispatcher.Dispatch(domainEvent).ConfigureAwait(false);
                }
            }

            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
