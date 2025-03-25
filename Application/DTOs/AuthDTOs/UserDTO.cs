using AccessControllSystem.Domain.Enum;


namespace Application.DTOs.AuthDTOs
{
    public class UserDTO
    {
        public string CccdId { get; set; }
        public string? Username { get; set; } = "";

        public string? Password { get; set; } = "";

        public string? FullName { get; set; } = "";

        public Gender? Gender { get; set; }
        public string? PhoneNumber { get; set; } = "";

        //public string ImageUrl { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; } = "";

        public int? UserRoleId { get; set; }
        public string? Email { get; set; }
    }
}
