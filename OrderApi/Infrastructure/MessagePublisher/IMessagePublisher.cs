using SharedModels;

namespace OrderApi.Infrastructure.MessagePublisher
{
    public interface IMessagePublisher
    {
        void PublishOrderStatusChangedMessage(int? customerId, IList<OrderLine> orderLines, string topic);
    }
}
