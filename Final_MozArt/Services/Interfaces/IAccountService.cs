using Final_MozArt.Models;
using Final_MozArt.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace Final_MozArt.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterVM request);
        Task<SignInResult> LoginAsync(LoginVM request);
        Task ConfirmEmailAsync(string userId, string token);
        Task<bool> ForgotPasswordAsync(ForgotPasswordVM model);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordVM model);
        Task LogoutAsync();
        Task<string> UpdateEmailAsync(string userId, string newEmail);
        Task<string> UpdateUsernameAsync(string userId, string newUsername);

        Task<AppUser> GetUserByIdAsync(string userId);

    }
}
