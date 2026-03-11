using CareBridge.Models.Common;
using CareBridge.Models.DTOs.Auth;
using CareBridge.Models.ViewModels.Auth;

namespace CareBridge.Services.Interfaces
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

