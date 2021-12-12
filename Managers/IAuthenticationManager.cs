using System.Threading.Tasks;
using TicketStore.Models;

namespace TicketStore.Managers
{
    public interface IAuthenticationManager
    {
        Task<bool> SignUp(SignUpUserModel signUpUserModel);
        Task<TokenModel> Login(LoginUserModel loginUserModel);
    }
}