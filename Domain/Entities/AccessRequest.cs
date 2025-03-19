namespace Domain.Entities
{
    public class AccessRequest
    {

        public int Id { get; set; }
        public int UserRequestId { get; set; }

        public int UserApprovalid { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Purpose { get; set; }

        //trạng thái duyệt request
        public int status { get; set; }

        public DateTime ApprovalAt { get; set; }
    }
}
