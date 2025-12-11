using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShop.Controllers
{
    public class ClientController : Controller
    {
        /// <summary>
        /// Gets or creates a persistent CartId cookie.
        /// </summary>
        /// <returns>The CartId string from cookie or a newly generated one.</returns>
        private string GetCartId()
        {
            var cartId = Request.Cookies["CartId"];

            if (string.IsNullOrEmpty(cartId))
            {
                cartId = Guid.NewGuid().ToString();
                var options = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                };
                Response.Cookies.Append("CartId", cartId, options);
            }

            return cartId;
        }
    }
}

