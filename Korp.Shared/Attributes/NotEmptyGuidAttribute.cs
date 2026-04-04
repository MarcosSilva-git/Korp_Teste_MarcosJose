using System.ComponentModel.DataAnnotations;

namespace Korp.Shared.Attributes;


[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class NotEmptyGuidAttribute : ValidationAttribute
{
    private const string DefaultErrorMessage = "The {0} field must not be an empty GUID.";

    public NotEmptyGuidAttribute() : base(DefaultErrorMessage) { }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is Guid guidValue && guidValue == Guid.Empty)
        {
            var message = string.Format(ErrorMessageString, validationContext.DisplayName);
            return new ValidationResult(message, [validationContext.MemberName!]);
        }

        return ValidationResult.Success;
    }
}
