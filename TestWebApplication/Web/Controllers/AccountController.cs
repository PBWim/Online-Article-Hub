﻿namespace Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service.Contracts;
    using ServiceModel;
    using Shared.Models;

    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AccountController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            return View();
        }

        // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2#create-an-authentication-cookie-1
        // Create Auth Cookie
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(UserModel user)
        {
            var result = await this.userService.SignIn(user.EmailId, user.Password);
            if (result)
            {
                if (string.IsNullOrWhiteSpace(user.ReturnUrl))
                {
                    var loggedUser = this.userService.Get(true).FirstOrDefault(x => x.Email == user.EmailId);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, $"{loggedUser.FirstName} {loggedUser.LastName}"), // Display this value on Bottom footer                   
                        new Claim("UserName", loggedUser.UserName), // UserName Custom Claim - used for auth (role)
                        new Claim(ClaimTypes.Email, loggedUser.Email), // Email Claim - used for auth (policy)                   
                        new Claim(ClaimTypes.Role, loggedUser.Role), // Role claim - used for auth                    
                        new Claim(ClaimTypes.DateOfBirth, DateTime.Now.AddYears(-20).ToString()), // DateOfBirth claim - used for auth (policy)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
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
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(user.ReturnUrl);
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid User Login");
            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp(UserModel user)
        {
            if (ModelState.IsValid)
            {
                user.Role = Constants.UserRole;
                var userObj = this.mapper.Map<User>(user);
                var result = await this.userService.Create(userObj);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                // https://www.tutorialspoint.com/asp.net_core/asp.net_core_create_a_user.htm
                else if (result.Errors.Any())
                {
                    foreach (var item in result.Errors)
                    {
                        // * Put empty for the key. Otherwise the error msgs won't show
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }
            }
            return View();
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