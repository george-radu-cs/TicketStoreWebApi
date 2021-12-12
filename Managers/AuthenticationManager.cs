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
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._tokenManager = tokenManager;
        }

        public async Task<bool> SignUp(SignUpUserModel signUpUserModel)
        {
            var user = new User
            {
                FirstName = signUpUserModel.FirstName,
                LastName = signUpUserModel.LastName,
                Email = signUpUserModel.Email,
                UserName = signUpUserModel.Email,
                PhoneNumber = signUpUserModel.PhoneNumber,
                PhonePrefix = signUpUserModel.PhonePrefix,
                Age=signUpUserModel.Age,
                IsStudent = signUpUserModel.IsStudent,
            };

            var result = await _userManager.CreateAsync(user, signUpUserModel.Password);
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
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserModel.Password, false);
            if (!result.Succeeded) return null;
            // create jwt token for user
            var token = await _tokenManager.CreateToken(user);

            return new TokenModel { Token = token };
        }
    }
}