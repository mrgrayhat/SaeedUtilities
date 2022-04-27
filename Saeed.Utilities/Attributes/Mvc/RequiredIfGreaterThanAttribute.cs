using System;
using System.ComponentModel.DataAnnotations;

namespace Saeed.Utilities.Attributes.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredIfGreaterThanAttribute : ValidationAttribute
    {
        public RequiredIfGreaterThanAttribute(string propertyName)
        {
            PropertyName = propertyName;
            ErrorMessage = "The {0} field is lower than {1} value."; //used if error message is not set on attribute itself
        }

        /// <summary>
        /// Value of the <see cref="PropertyName"/> that will trigger the validation
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Independent property name
        /// </summary>
        public string PropertyName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance;
            if (model == null || Value == 0)
            {
                return ValidationResult.Success;
            }

            var currentValue = int.Parse(model.GetType()
                .GetProperty(PropertyName)?.GetValue(model, null)?.ToString());

            if (Value < currentValue && value == null)
            {
                var propertyInfo = validationContext.ObjectType.GetProperty(validationContext.MemberName);
                return new ValidationResult($"{propertyInfo.Name} is lower than {currentValue}");
            }
            return ValidationResult.Success;
        }
    }
}
