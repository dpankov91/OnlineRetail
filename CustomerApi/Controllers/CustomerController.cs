using CustomerApi.Data;
using CustomerApi.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace CustomerApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<Customer> repository;

        public CustomerController(IRepository<Customer> repos)
        {
            repository = repos;
        }

        // GET: /<CustomerController>
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return repository.GetAll();
        }

        // GET /<CustomerController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);

            if (item == null)
            {
                var nullCustomer = new Customer
                {
                    Id = 0
                };

                return new ObjectResult(nullCustomer);
                //return NotFound();
            }

            return new ObjectResult(item);
        }

        // POST /<CustomerController>
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            var newCustomer = repository.Add(customer);

            return CreatedAtRoute("GetCustomer", new { id = newCustomer.Id }, newCustomer);
        }

        // PUT /<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE /<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
