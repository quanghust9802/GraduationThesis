using System.ComponentModel.DataAnnotations;

namespace AccessControllSystem.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 30)]
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        [StringLength(maximumLength: 30)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
