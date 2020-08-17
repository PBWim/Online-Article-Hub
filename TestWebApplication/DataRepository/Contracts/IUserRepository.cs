namespace DataRepository.Contracts
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using ServiceModel;

    public interface IUserRepository
    {
        Task<IdentityResult> Create(User user);

        Task<IdentityResult> Update(User user);
    }
}