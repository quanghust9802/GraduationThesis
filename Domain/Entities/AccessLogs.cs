using AccessControllSystem.Domain.Entities;

namespace Domain.Entities
{
    public class AccessLogs : BaseEntity
    {
        public int UserId { get; set; }

        public int AccessRequestId { get; set; }

        public DateTime RequestAt { get; set; }

        public int Status { get; set; }


    }
}
