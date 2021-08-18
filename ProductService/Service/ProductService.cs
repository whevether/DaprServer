namespace ProductService.Service
{
    public class ProductService: IProductService
    {
        private const int ProductStockSum = 100;

        public Model.Product GetProductById(int productId)
        {
            return new Model.Product()
            {
                Id = 1000,
                Name = "HuaWei Mate30 Pro",
                Stock = ProductStockSum
            };
        }

        public bool SaveProduct(Model.Product product)
        {
            throw new System.NotImplementedException();
        }
    }
}