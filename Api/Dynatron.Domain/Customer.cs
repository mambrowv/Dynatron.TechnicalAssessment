using Dynatron.Shared;

namespace Dynatron.Domain
{
    public sealed class Customer : EntityBase
    {
        public Customer(string? firstName, string? lastName, string emailAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress));
        }

        public int CustomerId { get; private set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string EmailAddress { get; set; }
    }
}
