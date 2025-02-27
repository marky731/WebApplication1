using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EntityLayer.ApiResponse;

namespace ApiLayer.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var apiResponse = new ApiResponse<string>(false, context.Exception.Message, null);
            context.Result = new JsonResult(apiResponse)
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}