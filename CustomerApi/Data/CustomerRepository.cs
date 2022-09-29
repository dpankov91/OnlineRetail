using CustomerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Data
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly CustomerApiContext db;

        public CustomerRepository(CustomerApiContext context)
        {
            db = context;
        }

        public Customer Add(Customer entity)
        {
            var newCustomer = db.Customers.Add(entity).Entity;
            db.SaveChanges();
            return newCustomer;
        }

        public void Edit(Customer entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Customer Get(int id)
        {
            var customer = db.Customers.FirstOrDefault(x => x.Id == id);
            return customer;
        }

        public IEnumerable<Customer> GetAll()
        {
            var allCustomer = db.Customers.ToList();
            return allCustomer;
        }

        public void Remove(int id)
        {
            var customer = db.Customers.FirstOrDefault(p => p.Id == id);
            if(customer != null)
            {
                db.Customers.Remove(customer);
                db.SaveChanges();
            }
        }
    }
}
