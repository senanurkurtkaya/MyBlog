using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MyBlog.AdminAPI.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                ArgumentException => 400,
                InvalidOperationException => 409,
                 _ => 500
            };

            var problem = new ProblemDetails()
            {
                Status = statusCode,
                Title = "İstek İşlenirken bir hata oluştu",
                Detail = statusCode == 500 ? "Beklenmeyen bir sunucu hatası oluştu." : exception.Message
            };
            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problem, options: null, contentType: "application/problem+json", cancellationToken: cancellationToken);
            return true;

        }
    }
}
