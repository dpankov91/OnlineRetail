namespace OrderApi.Infrastructure.ServiceGateaway
{
    public interface IServiceGateway<T>
    {
        T Get(int id);
    }
}
