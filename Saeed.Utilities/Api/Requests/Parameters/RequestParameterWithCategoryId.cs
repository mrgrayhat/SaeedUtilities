using System.ComponentModel.DataAnnotations;

namespace Saeed.Utilities.API.Requests.Parameters
{
    public class RequestParameterWithCategoryId : RequestParameterWithSort
    {
        public RequestParameterWithCategoryId() : base()
        {

        }

        public int CategoryId { get; set; }
    }
}
