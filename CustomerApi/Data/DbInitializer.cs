using CustomerApi.Models;

namespace CustomerApi.Data
{
    public class DbInitializer : IDbInitializer
    {
    
        public void Initialize(CustomerApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Customers
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            List<Customer> customers = new List<Customer>
            {
                new Customer {  Name = "Flex inc",
                                Email = "flex@email.com",
                                Phone = "+37256481122",
                                BillingAddress = "Flexible st. 14",
                                ShippingAddress = "Flinders st. 15, flat 24",
                                isCreditStanding = true},

                new Customer {  Name = "Jubelee company",
                                Email = "jubelee@email.com",
                                Phone = "+37256434212",
                                BillingAddress = "Jubstone st. 14",
                                ShippingAddress = "Green st. 15, flat 24",
                                isCreditStanding = false}
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
