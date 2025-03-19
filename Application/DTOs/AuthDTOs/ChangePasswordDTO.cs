using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AuthDTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? OldPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;

        [Required, Compare(nameof(NewPassword))]
        public string? ConfirmPassword { get; set; } = string.Empty;
    }
}
