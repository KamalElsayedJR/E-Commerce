using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Response
{
    public class ApiExceptionResponse : BaseResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(bool isSuccess, int statusCode, string message, string? details = null)
            : base(isSuccess, statusCode, message)
        {
            details = Details;
        }
    }
}
