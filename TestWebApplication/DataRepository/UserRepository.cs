namespace DataRepository
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using DataRepository.Contracts;
    using Microsoft.AspNetCore.Identity;
    using BO = ServiceModel;
    using DO = DataModel;

    public class UserRepository : IUserRepository
    {
        private readonly UserManager<DO.User> userManager;
        private readonly IMapper mapper;
        private const string superUserEmail = "SuperAdmin@example.com";

        public UserRepository(UserManager<DO.User> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IdentityResult> Create(BO.User user)
        {
            var userObj = this.mapper.Map<DO.User>(user);
            var adminUser = await this.userManager.FindByEmailAsync(superUserEmail);
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