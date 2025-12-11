using Microsoft.AspNetCore.Identity; // Required for inheriting IdentityUser

namespace FlowerShop.Models
{
    // Inheriting from IdentityUser adds essential fields like Username, Email, and Password hash.
    public class Client : IdentityUser
    {
        // Note: FirstName, Email, and Password properties are now provided by IdentityUser,
        // but we can add back the FirstName property manually if needed for user display.
        public string FirstName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public int Age { get; set; }
    }
}