using AccessControllSystem.Domain.Entities;
using AccessControllSystem.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User : BaseEntity
    {

        public string CccdId { get; set; }
        [StringLength(maximumLength: 35)]
        public string Username { get; set; } = "";

        [StringLength(maximumLength: 200)]
        public string Password { get; set; } = "";

        [StringLength(maximumLength: 20)]
        public string FullName { get; set; } = "";

        public Gender Gender { get; set; }

        public string PhoneNumber { get; set; } = "";

        public string ImageUrl { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Address { get; set; } = "";

        public ICollection<Notification> Notifications { get; set; }

    }
}
