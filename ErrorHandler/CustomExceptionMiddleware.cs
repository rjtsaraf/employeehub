using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EmpDepAPI.ErrorHandler
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(HttpStatusCodeException ex)
            {
                await HandleExceptionAsync(context,ex);
            }
            catch(Exception exceptionObj)
            {
                await HandleExceptionAsync(context,exceptionObj);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, HttpStatusCodeException ex)
        {
            string result=null;
            context.Response.ContentType = "application/json";
            if(ex is HttpStatusCodeException)
            {
                result=new ErrorDetails
                {
                    Message=ex.Message,
                    StatusCode=(int)ex.StatusCode
                }.ToString();
                context.Response.StatusCode=(int)ex.StatusCode;
            }
            else
            {
                result=new ErrorDetails
                {
                    Message="Runtime Error",
                    StatusCode=(int)HttpStatusCode.BadGateway
                }.ToString();
                context.Response.StatusCode=(int)HttpStatusCode.BadGateway;
            }
            return context.Response.WriteAsync(result);
        }
        private Task HandleExceptionAsync(HttpContext context,Exception exceptionObj)
        {
            string result= new ErrorDetails
            {
                Message=exceptionObj.Message,
                StatusCode=(int)HttpStatusCode.InternalServerError
            }.ToString();
            context.Response.StatusCode=(int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsync(result);
        }
    }
}