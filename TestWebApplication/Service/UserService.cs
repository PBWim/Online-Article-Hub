namespace Service
{
    using System.Linq;
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

        public Task<bool> SignIn(string email, string password)
        {
            var result = this.userRepository.SignIn(email, password);
            return result;
        }

        public IQueryable<User> Get(bool isAll = false)
        {
            var result = this.userRepository.Get(isAll);
            return result;
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