using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Saeed.Utilities.Extensions.Controllers
{
    public static class ModelStateExtensions
    {

        /// <summary>
        /// extract all model state errors to an array/list of strings
        /// </summary>
        /// <returns></returns>
        public static List<string> GetModelStateErrors(this ModelStateDictionary modelState)
        {
            List<string> validationMessages = new List<string>(modelState.ErrorCount);

            foreach ((string key, ModelStateEntry value) in modelState)
            {
                validationMessages.AddRange(collection: value.Errors.Select(error => $"{key} : {error.ErrorMessage}"));
            }

            return validationMessages;
        }

        /// <summary>
        /// this method extract all ModelState Error and Messages to a Dictionary.
        /// </summary>
        /// <returns></returns>
        public static IDictionary GetModelStateErrorsDictionary(this ModelStateDictionary modelState)
        {
            var validationMessage = new Dictionary<string, string>(modelState.ErrorCount);
            foreach ((KeyValuePair<string, ModelStateEntry> state, ModelError err)
                in modelState.SelectMany(state => state.Value.Errors.Select(e => (state, e))))
            {
                validationMessage.TryAdd(state.Key, err.ErrorMessage);
            }

            return validationMessage;
        }


        public static void AddIdentityResultErrors(this ModelStateDictionary modelState, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }

    }
}
