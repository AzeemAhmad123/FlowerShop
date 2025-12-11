using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowerShop.Data;
using FlowerShop.Models;

namespace FlowerShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly FlowerShopContext _context;
        private readonly UserManager<Client> _userManager;

        public OrderController(FlowerShopContext context, UserManager<Client> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays the My Orders / Order History page.
        /// </summary>
        /// <returns>The order history view.</returns>
        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Fetch orders for the current user (exclude cancelled orders)
            var orders = await _context.Orders
                .Where(o => o.ClientId == user.Id && o.Status != "Cancelled")
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // Get order details for each order
            var ordersWithDetails = new List<OrderHistoryViewModel>();
            foreach (var order in orders)
            {
                var orderDetails = await _context.OrderDetails
                    .Where(od => od.OrderId == order.Id)
                    .ToListAsync();

                ordersWithDetails.Add(new OrderHistoryViewModel
                {
                    Order = order,
                    OrderDetails = orderDetails
                });
            }

            return View(ordersWithDetails);
        }

        /// <summary>
        /// Cancels and deletes an order.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.ClientId == user.Id);

            if (order == null)
            {
                return NotFound();
            }

            // Only allow cancellation if order is not already delivered
            if (order.Status != "Delivered")
            {
                // Delete all order details first (foreign key constraint)
                var orderDetails = await _context.OrderDetails
                    .Where(od => od.OrderId == order.Id)
                    .ToListAsync();

                _context.OrderDetails.RemoveRange(orderDetails);

                // Delete the order
                _context.Orders.Remove(order);

                // Save changes
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Order has been cancelled and removed successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Delivered orders cannot be cancelled.";
            }

            return RedirectToAction("History");
        }

        /// <summary>
        /// Displays the tracking information for a specific order.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> TrackDelivery(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Fetch the order and verify it belongs to the current user
            var order = await _context.Orders
                .Where(o => o.Id == id && o.ClientId == user.Id)
                .Include(o => o.Client)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            // Get order details
            var orderDetails = await _context.OrderDetails
                .Where(od => od.OrderId == order.Id)
                .ToListAsync();

            var orderViewModel = new OrderHistoryViewModel
            {
                Order = order,
                OrderDetails = orderDetails
            };

            return View(orderViewModel);
        }
    }
}

