using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AuthDTOs
{
    public class LoginDTO
    {
        [Required]

        public string? Username { get; set; } = string.Empty;
        [Required]
        public string? Password { get; set; } = string.Empty;
    }
}
