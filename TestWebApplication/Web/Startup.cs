﻿namespace Web
{
    using System;
    using System.IO;
    using System.Security.Claims;
    using AutoMapper;
    using Data;
    using DataModel;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Shared;
    using Shared.Auth;
    using Shared.Mapper;
    using Web.Helpers;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            // Register DBContext with ConnectionString
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.0&tabs=visual-studio#create-a-web-app-with-authentication
            // User Identity settings are defined here. So in the UserManager (on repository when user creating, updating, login etc)
            // these settings are validated.
            services.AddDefaultIdentity<User>(options =>
            {
                // User settings
                options.User = new UserOptions
                {
                    RequireUniqueEmail = true
                };
                // Password settings
                options.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequiredLength = 10,
                    RequireLowercase = true,
                    RequireUppercase = true,
                    RequireNonAlphanumeric = true,
                    RequiredUniqueChars = 0
                };
                // Lockout settings
                options.Lockout = new LockoutOptions
                {
                    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15),
                    AllowedForNewUsers = true,
                    MaxFailedAccessAttempts = 5
                };
                // SignIn settings
                options.SignIn = new SignInOptions
                {
                    RequireConfirmedEmail = false,
                    RequireConfirmedPhoneNumber = false
                };
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // https://www.c-sharpcorner.com/article/cookie-authentication-in-net-core-3-0/
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2
            // Cookie based authorization - with Default values
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            // Or we can customize further as below
            //services.AddAuthentication("CookieAuthentication")
            //     .AddCookie("CookieAuthentication", config =>
            //     {
            //         config.Cookie = new CookieBuilder
            //         {
            //             Name = "UserLoginCookie",
            //             HttpOnly = true,
            //             Expiration = new TimeSpan(0, 30, 0),
            //             IsEssential = true,
            //             SameSite = SameSiteMode.Strict,
            //             SecurePolicy = CookieSecurePolicy.Always,
            //         };
            //         config.LoginPath = "/Account/Login";
            //         config.LogoutPath = "/Account/SignOut";
            //         config.AccessDeniedPath = "/Account/AccessDenied";
            //     });

            services.AddAuthorization(config =>
            {
                // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
                // Added claims for user defined policy
                config.AddPolicy("UserPolicy", policyBuilder =>
                {
                    policyBuilder.UserRequireCustomClaim(ClaimTypes.Email);
                    policyBuilder.UserRequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            // Register Services and DI
            services.RegisterServices();

            // AutoMapper
            // https://code-maze.com/automapper-net-core/
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // https://www.c-sharpcorner.com/article/add-file-logging-to-an-asp-net-core-mvc-application/
            // Logging
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log.txt");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2
            // Invoke the Authentication Middleware that sets the HttpContext.User property
            app.UseAuthentication();

            // https://blog.rsuter.com/automatically-migrate-your-entity-framework-core-managed-database-on-asp-net-core-application-start/
            // https://stackoverflow.com/questions/34343599/how-to-seed-users-and-roles-with-code-first-migration-using-identity-asp-net-cor
            // Run Migrate Database scripts and Seed default data
            app.UpdateDatabase();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}