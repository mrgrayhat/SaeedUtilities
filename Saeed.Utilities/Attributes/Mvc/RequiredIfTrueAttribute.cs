using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Saeed.Utilities.Attributes.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfTrueAttribute : ValidationAttribute, IClientModelValidator
    {
        private string PropertyName { get; set; }

        public RequiredIfTrueAttribute(string propertyName)
        {
            PropertyName = propertyName;
            ErrorMessage = "The {0} field is required."; //used if error message is not set on attribute itself
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();

            bool.TryParse(type.GetProperty(PropertyName)
                .GetValue(instance)?.ToString(), out bool propertyValue);

            if (propertyValue && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-requirediftrue", errorMessage);
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }
}
