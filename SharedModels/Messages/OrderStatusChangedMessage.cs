using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Messages
{
    public class OrderStatusChangedMessage
    {
        public int? CustomerId { get; set; }
        public IList<OrderLine> OrderLines { get; set; }
    }
}
