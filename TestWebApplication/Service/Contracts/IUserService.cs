namespace Service.Contracts
{
    using System.Threading.Tasks;
    using ServiceModel;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        Task<IdentityResult> Create(User user);

        Task<IdentityResult> Update(User user);
    }
}