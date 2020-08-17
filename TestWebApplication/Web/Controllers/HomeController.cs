namespace Web.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shared.Models;
    using Web.Models;

    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
        // Authorize tag with Policy - Should have both Email and DateOfBirth claims to access this
        [Authorize(Policy = "UserPolicy")]
        public IActionResult Privacy()
        {
            return View();
        }

        // Authorize tag - Should be only logged in to system to access this 
        [Authorize]
        public ActionResult Users()
        {
            var uses = new UserModel();
            return View(uses.GetUsers());
        }

        // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
        // Authorize tag with Role - Should have 'User' value for "Role" claim to access this. 
        // If no access, redirected to "AccessDenied" page
        // "AccessDenied" is declared from Cookie Auth options
        [Authorize(Roles = "User")]
        public ActionResult UsersRole()
        {
            var uses = new UserModel();
            return View(nameof(this.Users), uses.GetUsers());
        }

        // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
        // Authorize tag with Role - Should have 'Admin' value for "Role" claim to access this. 
        [Authorize(Roles = "Admin")]
        public ActionResult AdminUser()
        {
            var uses = new UserModel();
            return View(nameof(this.Users), uses.GetUsers());
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}