using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowerShop.Data;
using FlowerShop.Models;
using FlowerShop.Attributes;
using System.Linq;
using System.Text.Json;


namespace FlowerShop.Controllers
{
    public class CartController : Controller
    {
        private readonly FlowerShopContext _context;
        private readonly UserManager<Client> _userManager;

        public CartController(FlowerShopContext context, UserManager<Client> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            // Get cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

            // Calculate grand total
            var grandTotal = cart.Sum(c => c.Subtotal);
            ViewBag.GrandTotal = grandTotal;

            return View(cart);
        }

        [HttpPost]
        [ValidateAjaxAntiForgeryToken]
        public IActionResult UpdateCartAjax([FromBody] Dictionary<int, int> quantities)
        {
            // Get cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

            // Update quantities
            foreach (var item in cart.ToList())
            {
                if (quantities.ContainsKey(item.ProductId))
                {
                    var newQuantity = quantities[item.ProductId];
                    if (newQuantity > 0)
                    {
                        item.Quantity = newQuantity;
                    }
                    else
                    {
                        // Remove item if quantity is 0 or less
                        cart.Remove(item);
                    }
                }
                else
                {
                    // If product not in quantities dictionary, remove it
                    cart.Remove(item);
                }
            }

            // Save cart back to session
            cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);

            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(IFormCollection form)
        {
            // Get cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

            // Update quantities from form data
            foreach (var item in cart.ToList())
            {
                var quantityKey = $"quantities[{item.ProductId}]";
                if (form.ContainsKey(quantityKey))
                {
                    if (int.TryParse(form[quantityKey].ToString(), out int newQuantity))
                    {
                        if (newQuantity > 0)
                        {
                            item.Quantity = newQuantity;
                        }
                        else
                        {
                            // Remove item if quantity is 0 or less
                            cart.Remove(item);
                        }
                    }
                }
            }

            // Save cart back to session
            cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);

            TempData["SuccessMessage"] = "Cart updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int productId)
        {
            // Get cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

            // Remove item
            cart = cart.Where(c => c.ProductId != productId).ToList();

            // Save cart back to session
            cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);

            TempData["SuccessMessage"] = "Item removed from cart.";
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Checkout()
        {
            // Get cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

            if (!cart.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty. Please add items before checkout.";
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return Json(0);
            }

            var cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
            var totalItems = cart.Sum(c => c.Quantity);
            return Json(totalItems);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return View("Checkout", model);
            }

            try
            {
                // Get the current logged-in user (Client)
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                // Step 1: Create new Order object and link it to the Client
                var order = new Order
                {
                    ClientId = user.Id,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0m, // Will be calculated from OrderDetails
                    Status = "Pending"
                };

                // Step 2: Add order to context (Master Record)
                _context.Orders.Add(order);

                // Step 3: Save to get the Order ID (needed for OrderDetails)
                await _context.SaveChangesAsync();

                // Step 4: Create OrderDetail items from cart (Detail Records)
                var cartJson = HttpContext.Session.GetString("Cart");
                var cart = string.IsNullOrEmpty(cartJson)
                    ? new List<CartItem>()
                    : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

                if (!cart.Any())
                {
                    ModelState.AddModelError(string.Empty, "Your cart is empty.");
                    return View("Checkout", model);
                }

                decimal totalAmount = 0m;

                foreach (var cartItem in cart)
                {
                    var product = await _context.Products.FindAsync(cartItem.ProductId);
                    if (product != null)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            ProductId = cartItem.ProductId,
                            Quantity = cartItem.Quantity,
                            UnitPrice = cartItem.Price
                        };

                        _context.OrderDetails.Add(orderDetail);
                        totalAmount += orderDetail.Quantity * orderDetail.UnitPrice;
                    }
                }

                // Step 5: Update total amount based on OrderDetails
                order.TotalAmount = totalAmount;
                _context.Orders.Update(order);

                // Clear cart after successful order
                HttpContext.Session.Remove("Cart");

                // Step 6: Commit the transaction to the database (Final Save)
                await _context.SaveChangesAsync();

                // Step 7: Redirect to order history with success message
                TempData["SuccessMessage"] = "Order placed successfully!";
                return RedirectToAction("History", "Order");
            }
            catch (Exception ex)
            {
                // Log the error (in production, use proper logging)
                ModelState.AddModelError(string.Empty, "An error occurred while placing your order. Please try again.");
                return View("Checkout", model);
            }
        }
    }
}

