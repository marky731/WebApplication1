using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiLayer.Authorization;

public class AuthorizeUserIdAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public AuthorizeUserIdAttribute()
    {
        Policy = "UserIdPolicy"; 
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
    }
}