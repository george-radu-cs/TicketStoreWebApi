using System.Threading.Tasks;
using TicketStore.Models;

namespace TicketStore.Managers
{
    public interface IAuthenticationManager
    {
        Task<(bool success, string errorMessage, string errorType)> SignUp(SignUpUserModel signUpUserModel);
        Task<(TokenModel token, string errorMessage, string errorType)> Login(LoginUserModel loginUserModel);
    }
}