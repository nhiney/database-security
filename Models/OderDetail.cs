using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_N6.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }   // Join PRODUCTS
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Giá trị mở rộng (QUANTITY * PRICE)
        public decimal Total => Quantity * Price;
    }
}
