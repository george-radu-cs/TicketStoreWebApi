using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.ResponseModels;
using TicketStore.Utils;

namespace TicketStore.Managers
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenManager _tokenManager;

        public AuthenticationManager(UserManager<User> userManager, SignInManager<User> signInManager,
            ITokenManager tokenManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenManager = tokenManager;
        }

        // get password enhancers and a bool to mark if the strings are valid from environment variables
        private static async Task<(string, string, bool)> GetPasswordEnhancers()
        {
            var passEnhancerBefore = Environment.GetEnvironmentVariable("PASS_BEFORE");
            var passEnhancerAfter = Environment.GetEnvironmentVariable("PASS_AFTER");
            // couldn't get the password enhancers, set isValid to false, so we won't do the auth without these strings 
            if (passEnhancerBefore == null || passEnhancerAfter == null)
            {
                return ("", "", false);
            }

            return (passEnhancerBefore, passEnhancerAfter, true);
        }

        public async Task<(bool success, string errorMessage, string errorType)> SignUp(SignUpUserModel signUpUserModel)
        {
            // check if the input data is valid
            var (isValid, validationErrorMessage) = Validations.ValidateRegister(signUpUserModel);
            if (!isValid)
            {
                return (success: false, errorMessage: validationErrorMessage, errorType: ErrorTypes.UserFault);
            }

            // create the model to save in database
            var user = EntityConversions.ConvertToUserEntity(signUpUserModel);

            // get from env the password enhancers to create the password
            var (passEnhancerBefore, passEnhancerAfter, isPassValid) = GetPasswordEnhancers().Result;
            if (!isPassValid) // there was a problem while getting the password enhancers => stop the signup process
            {
                return (success: false, errorMessage: "Server error: Couldn't compute the password",
                    errorType: ErrorTypes.ServerFault);
            }

            // try to create the user in the db
            var result = await _userManager.CreateAsync(user,
                passEnhancerBefore + signUpUserModel.Password + passEnhancerAfter);
            if (!result.Succeeded)
            {
                return (success: false, errorMessage: "Couldn't create the user", errorType: ErrorTypes.ServerFault);
            }

            // add the user role for the user; in case of failure delete the user from the db
            try
            {
                var roleResult = await _userManager.AddToRoleAsync(user, signUpUserModel.Role);
                if (!roleResult.Succeeded)
                {
                    var toDeleteUser = await _userManager.FindByEmailAsync(user.Email);
                    await _userManager.DeleteAsync(toDeleteUser);

                    return (success: false, errorMessage: "Couldn't create the user", errorType: ErrorTypes.ServerFault);
                }
            }
            catch (Exception e)
            {
                var toDeleteUser = await _userManager.FindByEmailAsync(user.Email);
                await _userManager.DeleteAsync(toDeleteUser);
                throw;
            }

            // the user was created successfully
            return (success: true, errorMessage: "", errorType: "");
        }

        public async Task<(TokenModel token, string errorMessage, string errorType)> Login(
            LoginUserModel loginUserModel)
        {
            // check if the input data is valid
            var (isValid, validateErrorMessage) = Validations.ValidateLogin(loginUserModel);
            if (!isValid)
            {
                return (token: null, errorMessage: validateErrorMessage, errorType: ErrorTypes.UserFault);
            }

            // check if the user exists - find by normalized email
            var user = await _userManager.FindByEmailAsync(loginUserModel.Email);
            if (user == null)
            {
                return (token: null, errorMessage: "Error: User doesn't exists.", errorType: ErrorTypes.UserFault);
            }

            // get from env the password enhancers to check the password
            var (passEnhancerBefore, passEnhancerAfter, isPassValid) = GetPasswordEnhancers().Result;
            if (!isPassValid)
            {
                return (token: null, errorMessage: "Error: Couldn't verify the user password.", errorType: ErrorTypes.ServerFault);
            }

            // verify the password 
            var result = await _signInManager.CheckPasswordSignInAsync(user,
                passEnhancerBefore + loginUserModel.Password + passEnhancerAfter, false);
            if (!result.Succeeded)
            {
                return (token: null, errorMessage: "Error: Passwords doesn't match.", errorType: ErrorTypes.UserFault);
            }

            // create jwt token for user
            var token = await _tokenManager.CreateToken(user);
            return token == null
                ? (token: null, errorMessage: "Error: Couldn't create the jwt token.", errorType: ErrorTypes.ServerFault)
                : (token: new TokenModel { Token = token }, errorMessage: "", errorType: "");
        }

        public async Task<(UserResponseModel user, string errorMessage, string errorType)> GetUser(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return (user: null, errorMessage: "Error: User doesn't exists.", errorType: ErrorTypes.UserFault);
            }
            
            return (user: ResponseConversions.ConvertToUserResponseModel(user), errorMessage: null, errorType: null);
        }
    }
}