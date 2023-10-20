using Microsoft.AspNetCore.Routing;
using System.Runtime.CompilerServices;

namespace JobHunting.Middleware;

public class RedirectMiddleware
{
    private readonly EndpointDataSource _eds;
    private readonly RequestDelegate _requestDelegate;
    private readonly HashSet<string> setRoutes = new HashSet<string>();
    public RedirectMiddleware(RequestDelegate requestDelegate, EndpointDataSource eds)
    {
        _requestDelegate = requestDelegate;

        foreach (var endpoint in eds.Endpoints)
        {
            if (endpoint is RouteEndpoint routeEndpoint)
            {
                //var routePattern = routeEndpoint.RoutePattern.RawText;
                if (routeEndpoint.RoutePattern.RawText != null)
                {
                    setRoutes.Add("/" + routeEndpoint.RoutePattern.RawText.ToLowerInvariant());
                }
            }
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {

        if (setRoutes.Contains(context.Request.Path.Value.ToLower())) await _requestDelegate(context);
        else context.Response.Redirect("/api/error/notfound");
    }
}
