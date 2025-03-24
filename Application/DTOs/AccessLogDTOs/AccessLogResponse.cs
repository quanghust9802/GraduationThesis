using Application.DTOs.AccessRequestDTOs;
using Application.DTOs.AuthDTOs;

namespace Application.DTOs.AccessLogDTOs
{
    public class AccessLogResponse
    {
        public int UserId { get; set; }

        public int AccessRequestId { get; set; }


        public DateTime AccessTime { get; set; }

        public int Status { get; set; } //granted or denied

        public UserResponse? User { get; set; }

        public AccessRequestDTO? AccessRequest { get; set; }
    }
}
