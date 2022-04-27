using System.ComponentModel.DataAnnotations;

namespace Saeed.Utilities.API.Requests.Parameters
{
    public class GenericRequestParameter<TKey> : RequestParameterWithSort
    {
        public GenericRequestParameter() : base()
        {

        }

        public virtual string Deconstruct()
        {
            return $"{nameof(Parameter)}:{Parameter}:";
        }

        public virtual TKey Parameter { get; set; }
    }
}
