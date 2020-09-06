namespace Shared.Auth
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Service.Contracts;
    using Shared.Models;

    // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
    // Same like PoliciesAuthorizationHandler class. 
    // This class helps to validate user based on Role.
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public RolesAuthorizationHandler(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RolesAuthorizationRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var validRole = false;
            if (requirement.AllowedRoles == null ||
                requirement.AllowedRoles.Any() == false)
            {
                validRole = true;
            }
            else
            {
                var claims = context.User.Claims;
                var userName = claims.FirstOrDefault(c => c.Type == "UserName").Value;
                var roles = requirement.AllowedRoles;
                var users = this.userService.Get(true);
                var usersList = users.ProjectTo<UserModel>(this.mapper.ConfigurationProvider);
                validRole = usersList.Where(p => roles.Contains(p.Role) && p.UserName == userName).Any();
            }

            if (validRole)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}