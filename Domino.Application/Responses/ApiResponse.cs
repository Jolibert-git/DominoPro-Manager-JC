using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.Responses
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string? Message { get; private set; }
        public int StatusCode { get; private set; }


        protected ApiResponse() { }

        public static ApiResponse<T> SuccessResponse(T data, string? message = null)
        {
            return new()
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                StatusCode = 200
            };
        }

        public static ApiResponse<T> CreatedResponse(T data, string? message = "Resource created successfully")
        {
            return new()
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                StatusCode = 201
            };
        }

        public static ApiResponse<T> ErrorResponse(string message, int statusCode)
        {
            return new()
            {
                IsSuccess = false,
                Data = default,
                Message = message,
                StatusCode = statusCode
            };
        }
    }


    public class ApiResponse : ApiResponse<object>
    {
        public static ApiResponse<object> SuccessResponse(string? message = "Operation completed successfully")
        {
            return ApiResponse<object>.SuccessResponse(new { }, message);
        }

        public static ApiResponse<object> ErrorResponse(string message, int statusCode)
        {
            return ApiResponse<object>.ErrorResponse(message, statusCode);
        }
    }

}
