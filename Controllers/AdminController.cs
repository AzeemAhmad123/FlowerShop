using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowerShop.Data;
using FlowerShop.Models;

namespace FlowerShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly FlowerShopContext _context;
        private readonly UserManager<Client> _userManager;

        public AdminController(FlowerShopContext context, UserManager<Client> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Clean up any old cancelled orders that weren't deleted (from before delete functionality)
            var cancelledOrders = await _context.Orders
                .Where(o => o.Status == "Cancelled")
                .ToListAsync();

            if (cancelledOrders.Any())
            {
                foreach (var order in cancelledOrders)
                {
                    var orderDetails = await _context.OrderDetails
                        .Where(od => od.OrderId == order.Id)
                        .ToListAsync();
                    _context.OrderDetails.RemoveRange(orderDetails);
                }
                _context.Orders.RemoveRange(cancelledOrders);
                await _context.SaveChangesAsync();
            }

            // Get statistics for admin dashboard (exclude cancelled orders)
            var totalOrders = await _context.Orders.CountAsync();
            var totalProducts = await _context.Products.CountAsync();
            var totalUsers = await _context.Clients.CountAsync();
            var pendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending");

            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.PendingOrders = pendingOrders;

            return View();
        }

        public async Task<IActionResult> Orders()
        {
            // Exclude cancelled orders from the list
            var orders = await _context.Orders
                .Where(o => o.Status != "Cancelled")
                .OrderByDescending(o => o.OrderDate)
                .Include(o => o.Client)
                .ToListAsync();

            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // If status is set to Cancelled, delete the order
            if (status == "Cancelled")
            {
                // Delete all order details first
                var orderDetails = await _context.OrderDetails
                    .Where(od => od.OrderId == order.Id)
                    .ToListAsync();

                _context.OrderDetails.RemoveRange(orderDetails);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Order has been cancelled and deleted successfully.";
            }
            else
            {
                order.Status = status;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Order status updated successfully.";
            }

            return RedirectToAction("Orders");
        }

        public async Task<IActionResult> Users()
        {
            var users = await _context.Clients
                .OrderBy(u => u.Email)
                .ToListAsync();

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Prevent deleting the current admin user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null && currentUser.Id == id)
            {
                TempData["ErrorMessage"] = "You cannot delete your own account.";
                return RedirectToAction("Users");
            }

            // Delete all orders and order details for this user
            var userOrders = await _context.Orders
                .Where(o => o.ClientId == id)
                .ToListAsync();

            foreach (var order in userOrders)
            {
                var orderDetails = await _context.OrderDetails
                    .Where(od => od.OrderId == order.Id)
                    .ToListAsync();

                _context.OrderDetails.RemoveRange(orderDetails);
            }

            _context.Orders.RemoveRange(userOrders);
            await _context.SaveChangesAsync();

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User has been deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the user.";
            }

            return RedirectToAction("Users");
        }
    }
}

