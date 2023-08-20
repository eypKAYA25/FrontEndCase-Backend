using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace RestApi.Middlewares;

public class DomainExceptionHandlerMiddleware: IMiddleware
{
    public class ExtendedProblemDetails: ProblemDetails
    {
        
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {

            string request = $"[{context.Request.Method}] {context.Request.Path}";
            await next(context);
        }
        catch (Domain.Base.Exceptions.DomainException domainException)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = domainException.StatusCode;
            Console.WriteLine(domainException);
            await context.Response.WriteAsync
            (
                new ExtendedProblemDetails()
                {
                    Type = domainException.GetType().ToString()
                    , Title = domainException.Message
                    , Status = context.Response.StatusCode
                    , Detail = domainException.StackTrace
                }.ToJson()
            );
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message, exception);
            throw;
        }
    }
}