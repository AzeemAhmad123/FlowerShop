using Microsoft.AspNetCore.Mvc;

namespace FlowerShop.Controllers
{
    public class FeedbackController : Controller
    {
        /// <summary>
        /// Displays the feedback page.
        /// </summary>
        /// <returns>The feedback view.</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
