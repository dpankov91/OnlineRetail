using Microsoft.AspNetCore.Mvc;
using Microsoft.Toolkit.Uwp.Notifications;
using OrderApi.Data;
using OrderApi.Infrastructure.MessagePublisher;
using OrderApi.Infrastructure.ServiceGateaway;
using SharedModels;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        IOrderRepository repository;
        IServiceGateway<ProductDto> productServiceGateway;
        IMessagePublisher messagePublisher;
        IServiceGateway<CustomerDto> customerServiceGateway;

        public OrdersController(IRepository<Order> repos,
            IServiceGateway<ProductDto> productGateway,
            IServiceGateway<CustomerDto> customerGateway,
            IMessagePublisher publisher)
        {
            repository = repos as IOrderRepository;
            productServiceGateway = productGateway;
            customerServiceGateway = customerGateway;
            messagePublisher = publisher;
        }

        // GET: orders
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return repository.GetAll();
        }

        // GET orders/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST orders
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }


            if (ProductItemsAvailable(order))
            {
                try
                {
                    //Sent via http order.customerID to customerAPI, customerAPI checks if user exists
                    var customerId = order.customerId;
                    var customer = customerServiceGateway.Get(customerId);
                    if (customer.Id == 0)
                    {
                        new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText("User doesnt exist")
                        .AddText("Please register to make an order");

                        return NotFound("Please register to make an order");
                    }

                    if (customer.isCreditStanding == false)
                    {
                        new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText("")
                        .AddText("Please pay your bills first");

                        return NotFound("Please pay your bills first");
                    }

                    // Publish OrderStatusChangedMessage. If this operation
                    // fails, the order will not be created
                    messagePublisher.PublishOrderStatusChangedMessage(
                        order.customerId, order.OrderLines, "completed");

                    // Create order.
                    order.Status = Order.OrderStatus.completed;
                    var newOrder = repository.Add(order);
                    return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);
                }
                catch(Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
            {
                // If there are not enough product items available.
                new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText("Not enough items in stock")
                .AddText("Choose something else");

                return StatusCode(500, "Not enough items in stock.");
            }
        }

        private bool ProductItemsAvailable(Order order)
        {
            foreach (var orderLine in order.OrderLines)
            {
                // Call product service to get the product ordered.
                var orderedProduct = productServiceGateway.Get(orderLine.ProductId);
                if (orderLine.Quantity > orderedProduct.ItemsInStock - orderedProduct.ItemsReserved)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
