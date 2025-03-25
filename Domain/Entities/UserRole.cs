using AccessControllSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserRole : BaseEntity
    {

        [Required]
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();

    }

}
