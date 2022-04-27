using Huddeh.LocalizedResource.ErrorMessages;
using Saeed.Utilities.Gaurds;

using System.ComponentModel.DataAnnotations;

namespace Saeed.Utilities.Types.ValueObjects
{
    public class EmailValueObject
    {
        public EmailValueObject(string email)
        {
            EmailAddress = Guard.Against.InvalidEmail(email, nameof(email), ErrorMessages.InvalidEmailAddress);
        }

        [EmailAddress]
        public readonly string EmailAddress;

        public string Username => EmailAddress.Split('@')[0];
        public string Host => EmailAddress.Split('@')[1];
        public override string ToString()
        {
            return EmailAddress;
        }
    }
}
