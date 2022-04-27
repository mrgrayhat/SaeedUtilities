using System;
using System.ComponentModel.DataAnnotations;

namespace Saeed.Utilities.Attributes.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        /// <summary>
        /// Value of the <see cref="PropertyName"/> that will trigger the validation
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Independent property name
        /// </summary>
        public string PropertyName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance;
            if (model == null || Value == null)
            {
                return ValidationResult.Success;
            }

            var currentValue = model.GetType()
                .GetProperty(PropertyName)?.GetValue(model, null)?.ToString();

            if (Value.Equals(currentValue, StringComparison.Ordinal) && value == null)
            {
                var propertyInfo = validationContext.ObjectType.GetProperty(validationContext.MemberName);
                return new ValidationResult($"{propertyInfo.Name} is required for the current {PropertyName} value {currentValue}");
            }
            return ValidationResult.Success;
        }
    }
}
