using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr;
using ProductService.Model;
using ProductService.Service;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {

        private const string DaprPubSubName = "pubsub";

        private static readonly string[] FakeProducts = new[]
        {
            "SKU_*1", "SKU_*/////////2", "SKU_*///3", "SKU_*4", "SKU_*5", "SKU_*6", "SKU_*7", "SKU_*8", "SKU_*900000", "SKU_*10000"
        };

        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public IEnumerable<SKU> Get()
        {
            _logger.LogInformation("开始查询商品数据=====123456");

            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new SKU
                {
                    Date = DateTime.Now.AddDays(index),
                    Index = rng.Next(1, 100),
                    Summary = FakeProducts[rng.Next(FakeProducts.Length)]
                })
                .ToArray();

            _logger.LogInformation("结束查询商品数据-444444");

            return result;
        }
        
        [HttpPost]
        [Topic(DaprPubSubName, "neworder")]
        public Model.Product SubProductStock(OrderStockDto orderStockDto)
        {
            _logger.LogInformation($"[开始] 子产品库存，库存需求 : {orderStockDto.Count}.");

            var product = _productService.GetProductById(orderStockDto.ProductId);
            if (orderStockDto.Count < 0 || orderStockDto.Count > product.Stock)
            {
                throw new InvalidOperationException("没有这个库存");
            }
            product.Stock = product.Stock - orderStockDto.Count;
            _productService.SaveProduct(product);

            _logger.LogInformation($"[End] Sub Product Stock Finished, Stock Now : {product.Stock}.");

            return product;
        }
    }
}
