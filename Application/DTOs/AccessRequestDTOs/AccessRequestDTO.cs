namespace Application.DTOs.AccessRequestDTOs
{
    public class AccessRequestDTO
    {
        public int? Id { get; set; }
        public int? UserRequestId { get; set; }

        public int? UserApprovalid { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string? Purpose { get; set; }

        public int? status { get; set; }

        public DateTime? ApprovalAt { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public string? Mrz { get; set; }

    }
}
