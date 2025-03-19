using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AuthDTOs
{
    public class LoginDTO
    {
        [Required]

        public string? UserName { get; set; } = string.Empty;
        [Required]
        public string? Password { get; set; } = string.Empty;
    }
}
