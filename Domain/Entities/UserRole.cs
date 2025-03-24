using AccessControllSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserRole : BaseEntity
    {

        [Required]
        public RoleType RoleType { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();

    }

    public enum RoleType
    {
        Admin = 1,
        Employee = 2, //nhân viên nội bộ của công ty
        Guest = 3,
    }
}
