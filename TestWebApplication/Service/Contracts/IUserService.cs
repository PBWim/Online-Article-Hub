namespace Service.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;
    using ServiceModel;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        Task<bool> SignIn(string email, string password);

        IQueryable<User> Get(bool isAll = false);

        Task<IdentityResult> Create(User user);

        Task<IdentityResult> Update(User user);
    }
}