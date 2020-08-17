namespace DataRepository.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using ServiceModel;

    public interface IUserRepository
    {
        IQueryable<User> Get();

        Task<IdentityResult> Create(User user);

        Task<IdentityResult> Update(User user);
    }
}