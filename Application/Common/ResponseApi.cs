namespace Application.Common
{
    public class ResponseApi
    {
        public int ErrCode { get; set; }
        public string ErrDesc { get; set; }
        public string Message { get; set; }
        public Object? Data { get; set; }
    }
}
