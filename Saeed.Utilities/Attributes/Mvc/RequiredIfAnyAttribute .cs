using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Saeed.Utilities.Attributes.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredIfAnyAttribute : ValidationAttribute
    {
        public RequiredIfAnyAttribute(string propertyName)
        {
            PropertyName = propertyName;
            ErrorMessage = "The {0} field is required because of selected values."; //used if error message is not set on attribute itself
        }
        /// <summary>
        /// Values of the <see cref="PropertyName"/> that will trigger the validation
        /// </summary>
        public string[] Values { get; set; }

        /// <summary>
        /// Independent property name
        /// </summary>
        public string PropertyName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance;
            if (model == null || Values == null)
            {
                return ValidationResult.Success;
            }

            var currentValue = model.GetType()
                .GetProperty(PropertyName)?.GetValue(model, null)?.ToString();

            if (Values.Contains(currentValue) && value == null)
            {
                var propertyInfo = validationContext.ObjectType.GetProperty(validationContext.MemberName);
                return new ValidationResult(ErrorMessage ?? $"{propertyInfo.Name} is required for the current {PropertyName} value {currentValue}");
            }
            return ValidationResult.Success;
        }
    }
}
