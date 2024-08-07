namespace chat.Server.models
{
    public enum Code
    {
        HTTP200,
        HTTP201,
        HTTP204,//No Content
        HTTP400,
        HTTP401,
        HTTP403,
        HTTP404,
        HTTP500,
    }
    public class Status
    {
        public int StatusCode { get; set; }
        public Code Code { get; set; }
        public string Message { get; set; }
    }
}
