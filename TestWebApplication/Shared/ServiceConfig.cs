namespace Shared
{
    using DataRepository;
    using DataRepository.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.DependencyInjection;
    using Service;
    using Service.Contracts;
    using Shared.Auth;

    public static class ServiceConfig
    {
        // https://stackoverflow.com/questions/38138100/addtransient-addscoped-and-addsingleton-services-differences
        // Transient objects are always different; a new instance is provided to every controller and every service.
        //          * Since they are created every time they will use more memory & Resources and can have the negative impact on performance
        // Scoped objects are the same within a request, but different across different requests.
        //          * Better option when you want to maintain state within a request.
        // Singleton objects are the same for every object and every request
        //          * Use Singletons where you need to maintain application wide state
        public static void RegisterServices(this IServiceCollection services)
        {
            // Auth
            // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
            // Registered two handler services PoliciesAuthorizationHandler and 
            // RolesAuthorizationHandler of IAuthorizationHandler type
            services.AddScoped<IAuthorizationHandler, PoliciesAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();

            // Services
            services.AddScoped<IUserService, UserService>();

            // Repository
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}