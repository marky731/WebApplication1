using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

public class UserIdAuthorizationHandler : AuthorizationHandler<UserIdRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIdRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        string? targetUserId = null;

        if (context.Resource is HttpContext httpContext)
        {
            targetUserId = httpContext.GetRouteValue("userId")?.ToString();
        }
        else if (context.Resource is AuthorizationFilterContext authFilterContext)
        {
            targetUserId = authFilterContext.HttpContext.GetRouteValue("userId")?.ToString();
        }

        if (context.User.IsInRole("admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (!string.IsNullOrEmpty(userIdClaim) && !string.IsNullOrEmpty(targetUserId) && userIdClaim == targetUserId)
        {
            context.Succeed(requirement); 
        }
        else
        {
            context.Fail(); // Optionally explicitly fail.  Not succeeding is enough.
        }

        return Task.CompletedTask;
    }
}