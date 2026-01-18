using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace E_Commerce.Application.DTOs.Response
{
    public class DataResponse<T> : BaseResponse
    {
        public T? Data { get; set; } 
        public DataResponse(bool isSuccess,int statusCode,string message , T? data, List<string>? errors = null) 
                     : base(isSuccess, statusCode, message,errors)
        {
            Data = data;
        }
    }
}
