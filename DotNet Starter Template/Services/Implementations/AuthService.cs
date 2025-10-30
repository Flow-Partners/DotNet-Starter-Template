using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.Auth;
using DotNet_Starter_Template.Models.Entities;
using DotNet_Starter_Template.Models.ViewModels.Auth;
using DotNet_Starter_Template.Repositories.Interfaces;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<ApiResponse<LoginResponseViewModel>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return ApiResponse<LoginResponseViewModel>.ErrorResult("Invalid email or password");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded)
                {
                    return ApiResponse<LoginResponseViewModel>.ErrorResult("Invalid email or password");
                }

                if (!user.IsActive)
                {
                    return ApiResponse<LoginResponseViewModel>.ErrorResult("Account is deactivated");
                }

                // Update last login
                user.LastLoginAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                // Generate JWT token
                var token = await GenerateJwtTokenAsync(user);
                var refreshToken = GenerateRefreshToken();

                var userProfile = await GetUserProfileAsync(user.Id);

                var response = new LoginResponseViewModel
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                    User = userProfile.Data!
                };

                return ApiResponse<LoginResponseViewModel>.SuccessResult(response, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponseViewModel>.ErrorResult($"Login failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<LoginResponseViewModel>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return ApiResponse<LoginResponseViewModel>.ErrorResult("User with this email already exists");
                }

                existingUser = await _userManager.FindByNameAsync(registerDto.UserName);
                if (existingUser != null)
                {
                    return ApiResponse<LoginResponseViewModel>.ErrorResult("Username is already taken");
                }

                // Create new user
                var user = new User
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    EmailConfirmed = false,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ApiResponse<LoginResponseViewModel>.ErrorResult("Registration failed", errors);
                }

                // Assign default role (User)
                var defaultRole = await _roleRepository.GetByNameAsync("User");
                if (defaultRole != null)
                {
                    await _userManager.AddToRoleAsync(user, defaultRole.Name!);
                }

                // Generate JWT token
                var token = await GenerateJwtTokenAsync(user);
                var refreshToken = GenerateRefreshToken();

                var userProfile = await GetUserProfileAsync(user.Id);

                var response = new LoginResponseViewModel
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                    User = userProfile.Data!
                };

                return ApiResponse<LoginResponseViewModel>.SuccessResult(response, "Registration successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponseViewModel>.ErrorResult($"Registration failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> RefreshTokenAsync(string refreshToken)
        {
            // Implementation for refresh token logic
            return ApiResponse<string>.ErrorResult("Refresh token functionality not implemented yet");
        }

        public async Task<ApiResponse<bool>> LogoutAsync(string userId)
        {
            try
            {
                // Implementation for logout logic (token blacklisting, etc.)
                return ApiResponse<bool>.SuccessResult(true, "Logout successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Logout failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserProfileViewModel>> GetUserProfileAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<UserProfileViewModel>.ErrorResult("User not found");
                }

                var roles = await _userManager.GetRolesAsync(user);
                var permissions = new List<string>(); // Get user permissions

                var profile = new UserProfileViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    ProfilePicture = user.ProfilePicture,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt,
                    IsActive = user.IsActive,
                    Roles = roles.ToList(),
                    Permissions = permissions
                };

                return ApiResponse<UserProfileViewModel>.SuccessResult(profile);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserProfileViewModel>.ErrorResult($"Failed to get user profile: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.ErrorResult("User not found");
                }

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ApiResponse<bool>.ErrorResult("Password change failed", errors);
                }

                return ApiResponse<bool>.SuccessResult(true, "Password changed successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Password change failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return ApiResponse<bool>.ErrorResult("User not found");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                // Send email with reset token (implement email service)
                
                return ApiResponse<bool>.SuccessResult(true, "Password reset email sent");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to send reset email: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ResetPasswordAsync(string token, string email, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return ApiResponse<bool>.ErrorResult("User not found");
                }

                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ApiResponse<bool>.ErrorResult("Password reset failed", errors);
                }

                return ApiResponse<bool>.SuccessResult(true, "Password reset successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Password reset failed: {ex.Message}");
            }
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
            var issuer = jwtSettings["Issuer"] ?? "DotNetStarterTemplate";
            var audience = jwtSettings["Audience"] ?? "DotNetStarterTemplate";
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("FullName", $"{user.FirstName} {user.LastName}")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
