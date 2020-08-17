namespace Shared.Auth
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    public class PoliciesAuthorizationHandler : AuthorizationHandler<CustomUserRequireClaim>
    {
        // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
        // Helps to validate user based on policy
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
               CustomUserRequireClaim requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);

            if (hasClaim)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}