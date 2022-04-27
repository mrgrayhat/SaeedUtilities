namespace Saeed.Utilities.API.Requests.Parameters
{
    public class RequestParameterWithProjectId : RequestParameterWithSort
    {
        public RequestParameterWithProjectId() : base()
        {

        }

        public int ProjectId { get; set; }

        public override string Deconstruct(bool appendTypeName = false)
        {
            return string.Concat(base.Deconstruct(), appendTypeName ? "requestParameterWithProjectId:" : "", $"projectId:{ProjectId}");
        }
    }
}
