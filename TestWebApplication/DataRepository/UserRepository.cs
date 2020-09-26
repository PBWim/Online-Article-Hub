namespace DataRepository
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common;
    using Data;
    using DataRepository.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using BO = ServiceModel;
    using DO = DataModel;

    public class UserRepository : IUserRepository
    {
        private readonly UserManager<DO.User> userManager;
        private readonly SignInManager<DO.User> signInManager;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(UserManager<DO.User> userManager, SignInManager<DO.User> signInManager,
            IMapper mapper, ApplicationDbContext context, ILogger<UserRepository> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.context = context;
            this.logger = logger;
        }

        public async Task<bool> SignIn(string email, string password)
        {
            this.logger.LogInformation($"User sign in on {nameof(SignIn)} in UserRepository with Email : {email}");
            var userObj = await this.userManager.FindByEmailAsync(email);
            var isValid = await this.userManager.CheckPasswordAsync(userObj, password);
            return isValid;
        }

        public IQueryable<BO.User> Get(bool isAll)
        {
            this.logger.LogInformation($"Get users on {nameof(Get)} in UserRepository with isAll : {isAll}");
            var users = isAll ? this.context.Users : this.context.Users.Where(x => x.Role == Constants.UserRole);
            var usersList = users.ProjectTo<BO.User>(this.mapper.ConfigurationProvider);
            return usersList;
        }

        public async Task<IdentityResult> Create(BO.User user)
        {
            this.logger.LogInformation($"Create user on {nameof(Create)} in UserRepository with user details : {user}");
            var userObj = this.mapper.Map<DO.User>(user);
            var adminUser = await this.userManager.FindByEmailAsync(Constants.SuperUserEmail);
            userObj.LastModifiedBy = adminUser.Id;
            userObj.LastModifiedOn = DateTime.UtcNow;

            // * Give the Pwd on next parameter. Otherwise it is not hashing
            var result = await this.userManager.CreateAsync(userObj, userObj.PasswordHash);
            return result;
        }

        public async Task<IdentityResult> Update(BO.User user)
        {
            this.logger.LogInformation($"Update user on {nameof(Update)} in UserRepository with user details : {user}");
            var userObj = this.mapper.Map<DO.User>(user);
            var result = await userManager.UpdateAsync(userObj);
            return result;
        }
    }
}