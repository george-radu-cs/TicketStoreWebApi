using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketStore.Entities;
using TicketStore.Models;

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

        public async Task<bool> SignUp(SignUpUserModel signUpUserModel)
        {
            var date = DateTime.Now.ToUniversalTime();
            var user = new User
            {
                FirstName = signUpUserModel.FirstName,
                LastName = signUpUserModel.LastName,
                Email = signUpUserModel.Email,
                UserName = signUpUserModel.Email,
                PhoneNumber = signUpUserModel.PhoneNumber,
                PhonePrefix = signUpUserModel.PhonePrefix,
                Age = signUpUserModel.Age,
                IsStudent = signUpUserModel.IsStudent,
                CreatedAt = date,
                UpdatedAt = date,
            };

            var (passEnhancerBefore, passEnhancerAfter, isValid) = GetPasswordEnhancers().Result;
            if (!isValid) return false;
            var result = await _userManager.CreateAsync(user,
                passEnhancerBefore + signUpUserModel.Password + passEnhancerAfter);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, signUpUserModel.RoleId);
                return true;
            }

            return false;
        }

        public async Task<TokenModel> Login(LoginUserModel loginUserModel)
        {
            var user = await _userManager.FindByEmailAsync(loginUserModel.Email);
            if (user == null) return null;
            var (passEnhancerBefore, passEnhancerAfter, isValid) = GetPasswordEnhancers().Result;
            if (!isValid) return null;
            var result = await _signInManager.CheckPasswordSignInAsync(user,
                passEnhancerBefore + loginUserModel.Password + passEnhancerAfter, false);
            if (!result.Succeeded) return null;
            // create jwt token for user
            var token = await _tokenManager.CreateToken(user);
            return token == null ? null : new TokenModel { Token = token };
        }
    }
}