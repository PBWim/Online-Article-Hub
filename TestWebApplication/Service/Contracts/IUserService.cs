namespace Service.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;
    using ServiceModel;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        IQueryable<User> Get();

        Task<IdentityResult> Create(User user);

        Task<IdentityResult> Update(User user);
    }
}