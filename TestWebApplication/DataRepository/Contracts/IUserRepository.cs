namespace DataRepository.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using ServiceModel;

    public interface IUserRepository
    {
        Task<bool> SignIn(string email, string password);

        IQueryable<User> Get(bool isAll);

        Task<IdentityResult> Create(User user);

        Task<IdentityResult> Update(User user);
    }
}