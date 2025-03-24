using AccessControllSystem.Domain.Enum;
using Application.DTOs.NotificationDTOs;

namespace Application.DTOs.AuthDTOs
{
    public class UserResponse
    {
        public string CccdId { get; set; }
        public string Username { get; set; } = "";

        public string Password { get; set; } = "";
        public string FullName { get; set; } = "";

        public Gender Gender { get; set; }

        public string PhoneNumber { get; set; } = "";
        public string? Email { get; set; }

        public string ImageUrl { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Address { get; set; } = "";
        public int UserRoleId { get; set; }

        public UserRoleReponse UserRole { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
