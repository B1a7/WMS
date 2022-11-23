using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace WMS.IntegrationTests
{
    public class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "4"),
                    new Claim(ClaimTypes.Role, "admin"),
                }));

            context.HttpContext.User = claimsPrincipal;

            await next();
        }
    }
}
