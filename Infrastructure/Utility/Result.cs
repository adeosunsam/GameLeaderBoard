using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

namespace Infrastructure.Utility
{
    public class Result<T>
    {

        public string ResponseMessage { get; set; }
        public string ResponseCode { get; set; }
        public T? Data { get; set; }

        /*public Result(string responseCode, string responseMessage, T data)
        {
            Data = data;
            ResponseCode = responseCode;
            ResponseMessage = responseMessage;
        }*/
        public Result()
        {
        }
        public static Result<T> Fail(string responseMessage, string responseCode)
        {
            return new Result<T> { ResponseMessage = responseMessage, ResponseCode = responseCode };
        }
        public static Result<T> Success(string responseMessage, string responseCode = "200", T? data = default)
        {
            return new Result<T> { ResponseMessage = responseMessage, Data = data, ResponseCode = responseCode };
        }

    }
}
