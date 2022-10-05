namespace ProductApi.Models.Converter
{
    public interface IConverter<T, U>
    {
        T Convert(U model);
        U Convert(T model);
    }
}
