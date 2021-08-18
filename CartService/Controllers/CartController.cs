using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dapr.Client;

namespace CartService.Controllers
{
    public class SKU
    {
        public int Index { get; set; }

        public DateTime Date { get; set; }

        public string Summary { get; set; }
    }
    
    [ApiController]
    [Route("[controller]/[action]")]
    public class CartController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<CartController> _logger;
        public CartController(DaprClient daprClient, ILogger<CartController> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IEnumerable<SKU>> Get()
        {
            _logger.LogInformation("开始查询商品数据服务=====");

            var products = await _daprClient.InvokeMethodAsync<IEnumerable<SKU>>
                (HttpMethod.Get, "productapp", "Product/Get");

            _logger.LogInformation($"[End] 结束查询商品数据====== : {products.ToArray().ToString()}");

            return products;
        }
    }
}
