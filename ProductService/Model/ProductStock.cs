using System;

namespace ProductService.Model
{
    public class ProductStock
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Stock { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}