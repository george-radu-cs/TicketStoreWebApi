using System.Threading.Tasks;
using TicketStore.Models;
using TicketStore.ResponseModels;

namespace TicketStore.Managers
{
    public interface IAuthenticationManager
    {
        Task<(bool success, string errorMessage, string errorType)> SignUp(SignUpUserModel signUpUserModel);
        Task<(TokenModel token, string errorMessage, string errorType)> Login(LoginUserModel loginUserModel);
        Task<(UserResponseModel user, string errorMessage, string errorType)> GetUser(string userEmail);
    }
}