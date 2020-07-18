namespace Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Web.Models;

    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2#create-an-authentication-cookie-1
        // Create Auth Cookie
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Users user)
        {
            var users = new Users();
            var allUsers = users.GetUsers();
            var loggedUser = allUsers.FirstOrDefault(x => x.UserName == user.UserName);
            if (loggedUser != null && !string.IsNullOrWhiteSpace(loggedUser.EmailId))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loggedUser.Name), // Display this value on Bottom                   
                    new Claim("UserName", loggedUser.UserName), // UserName Custom Claim - used for auth (role)
                    new Claim(ClaimTypes.Email, loggedUser.EmailId), // Email Claim - used for auth (policy)                   
                    new Claim(ClaimTypes.Role, loggedUser.Role), // Role claim - used for auth                    
                    new Claim(ClaimTypes.DateOfBirth, loggedUser.DateOfBirth), // DateOfBirth claim - used for auth (policy)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                    {
                        // Refreshing the authentication session should be allowed.
                        AllowRefresh = true,

                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),

                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.
                        IsPersistent = true,

                        // The time at which the authentication ticket was issued.
                        IssuedUtc = DateTime.Now,

                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                        RedirectUri = "/Home"
                    });

                if (string.IsNullOrWhiteSpace(user.ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(user.ReturnUrl);
                }                
            }
            return View(user);
        }

        // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
        [HttpGet]
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}