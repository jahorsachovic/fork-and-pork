using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace fork_and_pork.Classes;

public static class PropertyValidator
{
    public static void Validate<T>(object instance, T value, [CallerMemberName] string propertyName = null)
    {
        var context = new ValidationContext(instance) { MemberName = propertyName };
        Validator.ValidateProperty(value, context);
    }
}

public class Money : ValidationAttribute
{
    public Money()
        : base("The price must have only 2 decimal places and be positive.")
    {
    }

    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        if (value is decimal priceValue)
        {
            if (decimal.Round(priceValue, 2) != priceValue)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName });
            }
            
            if (decimal.IsNegative(priceValue))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }

        return new ValidationResult("Price attribute can only be applied to decimal types.");
    }
}



public class NotFutureDate : ValidationAttribute
{
    public NotFutureDate()
        : base("The {0} must be a date in the past.")
    {
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateValue)
        {
            if (dateValue.Date <= DateTime.Today)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName });
            }
        }

        return new ValidationResult("NotFutureDate can only be applied to DateTime types.");
    }
}

public class FutureDate: ValidationAttribute 
{
    public FutureDate()
        : base("The {0} must be a date in the future.")
    {
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateValue)
        {
            if (dateValue.Date > DateTime.Today)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName });
            }
        }

        return new ValidationResult("FutureDate can only be applied to DateTime types.");
    }
}