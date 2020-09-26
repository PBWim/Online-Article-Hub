namespace Web.Controllers
{
    using System.Diagnostics;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Service.Contracts;
    using Shared.Models;
    using Web.Models;

    public class HomeController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ILogger<HomeController> logger;

        public HomeController(IUserService userService, IMapper mapper, ILogger<HomeController> logger)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            this.logger.LogInformation($"The Home {nameof(this.Index)} action has been accessed"); 
            return View();
        }

        // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
        // Authorize tag with Policy - Should have both Email and DateOfBirth claims to access this
        [Authorize(Policy = Constants.Policy)]
        public IActionResult Privacy()
        {
            this.logger.LogInformation($"The Home {nameof(this.Privacy)} action has been accessed");
            return View();
        }

        // Authorize tag - Should be only logged in to system to access this 
        [Authorize]
        public ActionResult Users()
        {
            this.logger.LogInformation($"The Home {nameof(this.Users)} action has been accessed");
            var users = this.userService.Get();
            var usersList = users.ProjectTo<UserModel>(this.mapper.ConfigurationProvider);
            return View(usersList);
        }

        // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
        // Authorize tag with Role - Should have 'User' value for "Role" claim to access this. 
        // If no access, redirected to "AccessDenied" page
        // "AccessDenied" is declared from Cookie Auth options
        [Authorize(Roles = Constants.UserRole)]
        public ActionResult UsersRole()
        {
            this.logger.LogInformation($"The Home {nameof(this.UsersRole)} action has been accessed");
            var users = this.userService.Get();
            var usersList = users.ProjectTo<UserModel>(this.mapper.ConfigurationProvider);
            return View(nameof(this.Users), usersList);
        }

        // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
        // Authorize tag with Role - Should have 'Admin' value for "Role" claim to access this. 
        [Authorize(Roles = Constants.AdminRole)]
        public ActionResult AdminUser()
        {
            this.logger.LogInformation($"The Home {nameof(this.AdminUser)} action has been accessed");
            var users = this.userService.Get();
            var usersList = users.ProjectTo<UserModel>(this.mapper.ConfigurationProvider);
            return View(nameof(this.Users), usersList);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            this.logger.LogInformation($"The Home {nameof(this.Error)} action has been accessed");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}