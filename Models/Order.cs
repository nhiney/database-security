using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_N6.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string CancelReason { get; set; }

        // Giá trị mở rộng khi load danh sách (không nằm trong DB)
        public decimal TotalAmount { get; set; }


    }
}
