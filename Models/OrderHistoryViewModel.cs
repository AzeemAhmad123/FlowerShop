namespace FlowerShop.Models
{
    public class OrderHistoryViewModel
    {
        public Order Order { get; set; } = null!;
        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
