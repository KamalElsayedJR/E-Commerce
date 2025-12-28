using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Response
{
    public class DataResponse<T> : BaseResponse
    {
        public List<T>? Data { get; set; } = new List<T>();
        public List<string>? Errors { get; set; } = new List<string>();
        public DataResponse(bool isSuccess, string message, List<T>? data, List<string>? errors): base(isSuccess,message)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            Errors = errors;
        }
    }
}
