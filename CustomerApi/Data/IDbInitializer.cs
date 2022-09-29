using System.Security.Cryptography;

namespace CustomerApi.Data
{
    public interface IDbInitializer
    {
        void Initialize(CustomerApiContext context);
    }
}
