using Dynatron.Domain;

namespace Dynatron.Api.Models
{
    public class CustomerModel
    {
        private CustomerModel() { }

        public int CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public DateTimeOffset UpdateDateTime { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }

        public static CustomerModel FromEntity(Customer entity)
        {
            return new CustomerModel
            {
                CustomerId = entity.CustomerId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                EmailAddress = entity.EmailAddress,
                CreatedDateTime = entity.CreatedDateTime,
                UpdateDateTime = entity.UpdatedDateTime
            };
        }
    }
}
