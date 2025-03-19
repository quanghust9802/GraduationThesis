using AccessControllSystem.Domain.Entities;

namespace Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }

        public string RoleType { get; set; }
    }
}
