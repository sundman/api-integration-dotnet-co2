using PaysonIntegration.Models.Enums;

namespace PaysonIntegration.Models
{
    public class Customer
    {
        public string City { get; set; }

        public string CountryCode { get; set; }

        public string IdentityNumber { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string PostalCode { get; set; }

        public string Street { get; set; }

        public string Reference { get; set; }

        public CustomerType Type { get; set; }

        public Customer()
        {
            Type = CustomerType.Person;
        }
    }
}