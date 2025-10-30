using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.Auth;
using DotNet_Starter_Template.Models.ViewModels.Auth;

namespace DotNet_Starter_Template.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseViewModel>> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<LoginResponseViewModel>> RegisterAsync(RegisterDto registerDto);
        Task<ApiResponse<string>> RefreshTokenAsync(string refreshToken);
        Task<ApiResponse<bool>> LogoutAsync(string userId);
        Task<ApiResponse<UserProfileViewModel>> GetUserProfileAsync(string userId);
        Task<ApiResponse<bool>> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<ApiResponse<bool>> ForgotPasswordAsync(string email);
        Task<ApiResponse<bool>> ResetPasswordAsync(string token, string email, string newPassword);
    }
}

