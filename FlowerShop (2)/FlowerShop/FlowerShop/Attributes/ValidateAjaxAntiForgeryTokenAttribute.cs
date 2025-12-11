using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace FlowerShop.Attributes
{
    /// <summary>
    /// Custom attribute to validate CSRF tokens for AJAX requests.
    /// For AJAX JSON requests, validates the token from the RequestVerificationToken header.
    /// </summary>
    public class ValidateAjaxAntiForgeryTokenAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;
            var antiforgery = context.HttpContext.RequestServices.GetService<IAntiforgery>();

            if (antiforgery == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            try
            {
                // Check if token exists in header (for AJAX) or form (for regular forms)
                var token = request.Headers["RequestVerificationToken"].ToString();
                if (string.IsNullOrEmpty(token) && request.HasFormContentType)
                {
                    token = request.Form["__RequestVerificationToken"].ToString();
                }

                // If no token found, reject the request
                if (string.IsNullOrEmpty(token))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                // Validate the anti-forgery token
                // This ensures the request token matches the cookie token
                antiforgery.ValidateRequestAsync(context.HttpContext).Wait();
            }
            catch
            {
                // Token validation failed
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}


