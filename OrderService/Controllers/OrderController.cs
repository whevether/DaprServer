using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;

namespace OrderService.Controllers
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }
    }
    public class OrderStockDto
    {
        public int ProductId { get; set; }

        public int Count { get; set; }
    }
	
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private const string DaprPubSubName = "pubsub";

        private readonly ILogger<OrderController> _logger;
        private readonly DaprClient _daprClient;

        public OrderController(ILogger<OrderController> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        [HttpPost]
        public async Task<OrderDto> Post(OrderDto orderDto)
        {
            _logger.LogInformation("开始创建订单");

            var order = new OrderDto()
            {
                // some mapping
                Id = orderDto.Id,
                ProductId = orderDto.ProductId,
                Count = orderDto.Count
            };
            // some other logic for order

            var orderStockDto = new OrderStockDto()
            {
                ProductId = orderDto.ProductId,
                Count = orderDto.Count
            };
            await _daprClient.PublishEventAsync(DaprPubSubName, "neworder", orderStockDto);

            _logger.LogInformation($"[End] 创建订单id : {orderStockDto.ProductId}, 数量 : {orderStockDto.Count}");

            return order;
        }
    }
}
