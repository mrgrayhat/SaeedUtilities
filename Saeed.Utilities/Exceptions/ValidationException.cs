using System;
using System.Collections.Generic;

using FluentValidation.Results;

using Huddeh.LocalizedResource.ErrorMessages;

namespace Saeed.Utilities.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base(ErrorMessages.ValidationError)
        {
            Errors = new List<string>();
        }
        public List<string> Errors { get; }
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            foreach (var failure in failures)
            {
                Errors.Add(failure.ErrorMessage);
            }
        }

        //public ValidationException()
        //    : base("One or more validation failures have occurred.")
        //{
        //    Errors = new Dictionary<string, string[]>();
        //}

        //public ValidationException(List<ValidationFailure> errors)
        //    : this()
        //{
        //    var propertyNames = errors
        //        .Select(e => e.PropertyName)
        //        .Distinct();

        //    foreach (var propertyName in propertyNames)
        //    {
        //        var propertyFailures = errors
        //            .Where(e => e.PropertyName == propertyName)
        //            .Select(e => e.ErrorMessage)
        //            .ToArray();

        //        Errors.Add(propertyName, propertyFailures);
        //    }
        //}

        //public IDictionary<string, string[]> Errors { get; }
    }
}
