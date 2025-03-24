using AccessControllSystem.Domain.Entities;

namespace Domain.Entities
{
    public class AccessLogs : BaseEntity
    {
        public int UserId { get; set; }

        public int? AccessRequestId { get; set; }


        public DateTime AccessTime { get; set; }

        public int Status { get; set; } //granted or denied

        public virtual User User { get; set; }

        public virtual AccessRequest? AccessRequest { get; set; }
    }
}
