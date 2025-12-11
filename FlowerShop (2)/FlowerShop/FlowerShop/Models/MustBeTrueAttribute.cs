using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Models
{
    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is bool boolValue)
            {
                return boolValue;
            }
            return false;
        }
    }
}

