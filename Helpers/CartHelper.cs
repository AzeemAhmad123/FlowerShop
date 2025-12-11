using System.Text.Json;
using FlowerShop.Models;


namespace FlowerShop.Helpers
{
    public static class CartHelper
    {
        public static int GetCartItemCount(ISession session)
        {
            var cartJson = session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return 0;
            }

            var cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson);
            return cart?.Sum(c => c.Quantity) ?? 0;
        }
    }
}

