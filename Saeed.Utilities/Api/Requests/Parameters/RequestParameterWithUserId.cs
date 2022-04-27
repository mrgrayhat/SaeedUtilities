using System.ComponentModel.DataAnnotations;

namespace Saeed.Utilities.API.Requests.Parameters
{
    public class RequestParameterWithUserId : RequestParameterWithSort
    {
        public RequestParameterWithUserId() : base()
        {

        }

        [Required]
        public string UserId { get; set; }
    }
}
