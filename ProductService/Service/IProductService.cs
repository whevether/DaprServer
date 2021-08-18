namespace ProductService.Service
{
    public interface IProductService
    {
        Model.Product GetProductById(int productId);
        bool SaveProduct(Model.Product product);
    }
}