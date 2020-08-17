namespace Service
{
    using System.Threading.Tasks;
    using DataRepository.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Service.Contracts;
    using ServiceModel;

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IdentityResult> Create(User user)
        {
            var result = await this.userRepository.Create(user);
            return result;
        }

        public async Task<IdentityResult> Update(User user)
        {
            var result = await userRepository.Update(user);
            return result;
        }
    }
}