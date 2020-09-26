namespace Web.ApiControllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Service.Contracts;
    using Shared.Models;
    using Web.Models;

    // Swagger - JWT Auth
    // https://www.c-sharpcorner.com/article/authentication-authorization-using-net-core-web-api-using-jwt-token-and/
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ILogger<AuthController> logger;
        private readonly IConfiguration configuration;

        public AuthController(IUserService userService, IMapper mapper, ILogger<AuthController> logger,
            IConfiguration configuration)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.logger = logger;
            this.configuration = configuration;
        }

        /// <summary>  
        /// Login Authenticaton using JWT Token Authentication  
        /// </summary>  
        /// <param name="data"></param>  
        /// <returns></returns> 
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(AuthUserModel user)
        {
            this.logger.LogInformation($"The Api Account {nameof(this.Login)} action has been accessed with User Model : {user}");
            var result = await this.userService.SignIn(user.Email, user.Password);
            if (result)
            {
                this.logger.LogInformation($"JWT Token created User Model : {user}");
                var tokenString = this.GenerateJSONWebToken(user);
                return Ok(new { Token = tokenString, Message = "Success" });
            }
            this.logger.LogWarning($"Invalid User Login of User Model : {user}");
            ModelState.AddModelError(string.Empty, "Invalid User Login");
            return BadRequest(user);
        }

        /// <summary>  
        /// Authorize the Method  
        /// </summary>  
        /// <returns></returns> 
        [HttpGet(nameof(Get))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get()
        {
            this.logger.LogInformation($"Get JWT Token created User");
            var users = this.userService.Get();
            var usersList = users.ProjectTo<UserModel>(this.mapper.ConfigurationProvider);
            return Ok(usersList);
        }

        private string GenerateJSONWebToken(AuthUserModel userInfo)
        {
            this.logger.LogInformation($"Generate JWT Token for User {userInfo}");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(this.configuration["Jwt:Issuer"],
              this.configuration["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120), // 2 hours
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}