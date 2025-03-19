using Domain.Enum;
using ProcessManagement.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AuthDTOs
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";

        [StringLength(maximumLength: 40)]
        public string LastName { get; set; } = "";
        public Gender Gender { get; set; }

        public Role RoleType { get; set; }

        public string PhoneNumber { get; set; } = "";

        public string Address { get; set; } = "";

        public DateTime DateOfBirth { get; set; }

        public string ImageUrl { get; set; }
    }
}
