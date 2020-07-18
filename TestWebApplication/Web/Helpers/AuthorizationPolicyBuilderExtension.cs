namespace Web.Helpers
{
    using Microsoft.AspNetCore.Authorization;

    // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
    // for addding a new policy for Authorization using AuthorizationPolicyBuilder
    public static class AuthorizationPolicyBuilderExtension
    {
        public static AuthorizationPolicyBuilder UserRequireCustomClaim(
            this AuthorizationPolicyBuilder builder, string claimType)
        {
            builder.AddRequirements(new CustomUserRequireClaim(claimType));
            return builder;
        }
    }
}