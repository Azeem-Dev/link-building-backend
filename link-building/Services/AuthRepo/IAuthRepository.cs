using link_building.Dtos;
using link_building.Dtos.User;
using link_building.Models.User;
using System.Threading.Tasks;

namespace link_building.Services.AuthRepo
{
    public interface IAuthRepository
    {
         Task<ServiceResponse<int>> Register(UserEntity user, string Password);
         Task<ServiceResponse<string>> Login(string username, string password);
         Task<bool> UserExists(string username);
         Task<ServiceResponse<string>> ForgotPassword(UserPasswordUpdateDto request);
         Task<ServiceResponse<string>> UpdateRole(UserRoleUpdateDto request);

    }
}
