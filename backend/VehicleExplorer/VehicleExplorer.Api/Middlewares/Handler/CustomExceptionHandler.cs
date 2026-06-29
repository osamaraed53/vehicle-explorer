using Microsoft.AspNetCore.Diagnostics;

namespace VehicleExplorer.Api.Middlewares.Handler;

public class CustomExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
