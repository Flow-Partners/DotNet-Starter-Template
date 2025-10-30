using System.ComponentModel.DataAnnotations;

namespace DotNet_Starter_Template.Models.DTOs.Users
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string UserName { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? State { get; set; }

        [StringLength(50)]
        public string? Country { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [StringLength(50)]
        public string? Timezone { get; set; }

        [StringLength(10)]
        public string? Language { get; set; }

        [StringLength(20)]
        public string? Theme { get; set; }

        [StringLength(500)]
        public string? ProfilePicture { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
