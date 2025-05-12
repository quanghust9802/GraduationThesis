namespace Application.DTOs.AccessLogDTOs
{
    public class AccessLogDTO
    {
        public int UserId { get; set; }

        public int AccessRequestId { get; set; }


        public DateTime AccessTime { get; set; }

        public int Status { get; set; }

        public string CccdId { get; set;}
    }
}
