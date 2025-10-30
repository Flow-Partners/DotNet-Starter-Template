namespace DotNet_Starter_Template.Models.ViewModels.Auth
{
    public class LoginResponseViewModel
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserProfileViewModel User { get; set; } = new UserProfileViewModel();
    }
}

