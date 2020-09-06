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
    using ServiceModel;
    using BO = ServiceModel;
    using DO = DataModel;

    public class UserRepository : IUserRepository
    {
        private readonly UserManager<DO.User> userManager;
        private readonly SignInManager<DO.User> signInManager;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;

        public UserRepository(UserManager<DO.User> userManager, SignInManager<DO.User> signInManager,
            IMapper mapper, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<bool> SignIn(string email, string password)
        {
            var userObj = await this.userManager.FindByEmailAsync(email);
            var isValid = await this.userManager.CheckPasswordAsync(userObj, password);
            return isValid;
        }

        public IQueryable<User> Get(bool isAll)
        {
            var users = isAll ? this.context.Users : this.context.Users.Where(x => x.Role == Constants.UserRole);
            var usersList = users.ProjectTo<BO.User>(this.mapper.ConfigurationProvider);
            return usersList;
        }

        public async Task<IdentityResult> Create(BO.User user)
        {
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
            var userObj = this.mapper.Map<DO.User>(user);
            var result = await userManager.UpdateAsync(userObj);
            return result;
        }
    }
}