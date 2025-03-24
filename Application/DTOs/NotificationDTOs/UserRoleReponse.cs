using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.NotificationDTOs
{
    public class UserRoleReponse
    {
        public int UserId { get; set; }

        [Required]
        public RoleType RoleType { get; set; }
    }

    public enum RoleType
    {
        Admin = 1,
        Manager = 2,
        Employee = 3,
        Guest = 4
    }
}
