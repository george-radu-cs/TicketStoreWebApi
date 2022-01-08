using System.Threading.Tasks;
using TicketStore.Models;
using TicketStore.ResponseModels;

namespace TicketStore.Managers
{
    public interface IAuthenticationManager
    {
        Task<ResponseSuccessWithErrors> SignUp(SignUpUserModel signUpUserModel);
        Task<ResponseRecordWithErrors<TokenModel>> Login(LoginUserModel loginUserModel);
        Task<ResponseRecordWithErrors<UserResponseModel>> GetUser(string userEmail);
    }
}