using AccessControllSystem.Domain.Entities;

namespace Domain.Entities
{
    public class AccessRequest : BaseEntity
    {
        public int UserRequestId { get; set; }

        public int UserApprovalid { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Purpose { get; set; }

        //trạng thái duyệt request
        public int status { get; set; } //chờ duyệt/ đồng ý/ từ chối.

        public DateTime ApprovalAt { get; set; }
        public virtual User RequestUser { get; set; }

        public virtual User ApproveUser { get; set; }

        public virtual ICollection<AccessLogs> AccessLogs { get; set; }

    }
}
