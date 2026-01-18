using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Response
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<string>? Errors { get; set; } = new List<string>();
        public int StatusCode { get; set; }

        public BaseResponse(bool isSuccess, int statusCode , string message = null, List<string>? errors = null)
        {
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            Message = message ?? GetDefaultMessage(StatusCode);
            Errors = errors;
        }

        private string GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => "Operation completed",
            };
        }
    }
}
