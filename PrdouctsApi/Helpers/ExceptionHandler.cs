using Microsoft.AspNetCore.Diagnostics;
using Serilog;

namespace ProductInventoryManagerAPI.Helpers
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            Log.Error(exception.Message);

            await httpContext.Response.WriteAsJsonAsync("Something went wrong", cancellationToken: cancellationToken);
            
            return true;

        }
    }
}
