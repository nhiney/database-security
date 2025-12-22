namespace DA_N6.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal Total
        {
            get { return Quantity * Price; }
        }
    }

    public static class ShoppingCart
    {
        public static System.Collections.Generic.List<CartItem> Items { get; set; } = new System.Collections.Generic.List<CartItem>();

        public static void AddToCart(int id, string name, decimal price, int qty)
        {
            var existingItem = System.Linq.Enumerable.FirstOrDefault(Items, x => x.ProductId == id);
            if (existingItem != null)
            {
                existingItem.Quantity += qty;
            }
            else
            {
                Items.Add(new CartItem { ProductId = id, ProductName = name, Price = price, Quantity = qty });
            }
        }

        public static void Clear() => Items.Clear();
        public static decimal GetGrandTotal() => System.Linq.Enumerable.Sum(Items, x => x.Total);
    }
}