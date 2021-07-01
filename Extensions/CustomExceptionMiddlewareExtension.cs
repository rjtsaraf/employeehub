using EmpDepAPI.ErrorHandler;
using Microsoft.AspNetCore.Builder;

namespace EmpDepAPI.Extensions
{
    public static class  CustomExceptionMiddlewareExtension
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}