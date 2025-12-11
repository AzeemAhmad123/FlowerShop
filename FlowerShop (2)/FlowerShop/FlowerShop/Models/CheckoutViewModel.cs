using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Models
{
    public class CheckoutViewModel
    {
        [Required]
        public string StreetAddress { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        public string ExpiryDate { get; set; } = string.Empty;

        [Required]
        public string Cvc { get; set; } = string.Empty;

    }
}
