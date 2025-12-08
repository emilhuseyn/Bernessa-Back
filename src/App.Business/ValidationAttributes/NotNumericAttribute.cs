using System.ComponentModel.DataAnnotations;

namespace App.Business.ValidationAttributes
{
    public class NotNumericAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var stringValue = value.ToString();
            
            if (int.TryParse(stringValue, out _) || long.TryParse(stringValue, out _))
            {
                return new ValidationResult(ErrorMessage ?? "Slug sad?c? r?q?m ola bilm?z");
            }

            return ValidationResult.Success;
        }
    }
}
