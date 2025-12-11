using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowerShop.Data;
using FlowerShop.Models;
using System;
using System.Linq;
using System.Text.Json;
using FlowerShop.Controllers;

namespace FlowerShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly FlowerShopContext _context;

        public ShopController(FlowerShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the product catalog.
        /// </summary>
        /// <returns>The product catalog view.</returns>
        public IActionResult Index()
        {
            try
            {
                // Load all products from database
                var allProducts = _context.Products.ToList();

                // Map to ProductViewModel
                var result = allProducts
                    .Select(p => new ProductViewModel
                    {
                        ProductId = p.Id,
                        Name = p.Name ?? string.Empty,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl ?? string.Empty
                    })
                    .ToList();

                return View(result);
            }
            catch (Exception ex)
            {
                // Log the error and return empty list to prevent crash
                // In production, you should log this properly
                return View(new List<ProductViewModel>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            // Get cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

            // Check if product already in cart
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                });
            }

            // Save cart back to session
            cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);

            // Calculate total items for badge
            var totalItems = cart.Sum(c => c.Quantity);

            return Json(new { success = true, totalItems = totalItems, message = "Product added to cart" });
        }
    }
}

