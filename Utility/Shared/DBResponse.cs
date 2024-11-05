namespace Utility
{
    public class DBResponse
    {
        public DBResponse()
        {

        }
        public DBResponse(object responseData, bool hasData, string responseMessage="")
        {
            ResponseData = responseData;
            HasData = hasData;
            ResponseMessage = responseMessage;
        }

        public bool HasData { get; set; }
        public object ResponseData { get; set; }
        public string ResponseMessage { get; set; }
    }
}
