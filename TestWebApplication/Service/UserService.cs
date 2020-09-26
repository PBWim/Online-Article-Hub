namespace Service
{
    using System.Linq;
    using System.Threading.Tasks;
    using DataRepository.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Service.Contracts;
    using ServiceModel;

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UserService> logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public Task<bool> SignIn(string email, string password)
        {
            this.logger.LogInformation($"User sign in on {nameof(SignIn)} in UserService with Email : {email}");
            var result = this.userRepository.SignIn(email, password);
            return result;
        }

        public IQueryable<User> Get(bool isAll = false)
        {
            this.logger.LogInformation($"Get users on {nameof(Get)} in UserService with isAll : {isAll}");
            var result = this.userRepository.Get(isAll);
            return result;
        }

        public async Task<IdentityResult> Create(User user)
        {
            this.logger.LogInformation($"Create user on {nameof(Create)} in UserService with user details : {user}");
            var result = await this.userRepository.Create(user);
            return result;
        }

        public async Task<IdentityResult> Update(User user)
        {
            this.logger.LogInformation($"Update user on {nameof(Update)} in UserService with user details : {user}");
            var result = await userRepository.Update(user);
            return result;
        }
    }
}