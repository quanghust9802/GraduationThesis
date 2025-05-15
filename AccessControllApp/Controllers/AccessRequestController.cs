using AccessControllApplication.Controllers;
using Application.DTOs.AccessRequestDTOs;
using Application.Services.AccessRequestServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControllApp.Controllers
{
    [Route("api/access-request")]
    [ApiController]
    public class AccessRequestController : BaseController
    {
        private readonly IAccessRequestService _accessRequestService;
        public AccessRequestController(IAccessRequestService accessRequestService)
        {
            _accessRequestService = accessRequestService;
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var dto = await _accessRequestService.GetByIdAsync(id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var dtos = await _accessRequestService.GetAllAsync();
            return Ok(dtos);
        }



        [HttpPost("create")]
        public async Task<IActionResult> InsertAsync([FromForm] AccessRequestDTO dto)
        {
            var res = await _accessRequestService.InsertAsync(dto);
            return Ok(res);
        }


        [HttpDelete("{id}")]
        //[Authorize(Roles = nameof(UserRole.RoleType))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _accessRequestService.DeleteAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = nameof(UserRole.RoleType))]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] AccessRequestDTO dto)
        {
            var res = await _accessRequestService.UpdateAsync(id, dto);
            return Ok(res);
        }

        [HttpPut("update-status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id, [FromQuery] int status)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Không tìm thấy hoặc UserId không hợp lệ trong token");
            }

            var res = await _accessRequestService.UpdateStatus(id, status, userId);
            return Ok(res);
        }

        [HttpGet("get-filter")]
        public async Task<IActionResult> GetFilter([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int? status, [FromQuery] int? userId)
        {
            var dtos = await _accessRequestService.GetByFilterAsync(startDate, endDate, status, userId);
            return Ok(dtos);
        }
        [HttpGet("get-status")]
        public async Task<IActionResult> GetByStatus([FromQuery] int? userId, [FromQuery] int? status)
        {
            var dtos = await _accessRequestService.GetByStatus(userId, status);
            return Ok(dtos);
        }

        [HttpGet("verify-infor")]
        public async Task<IActionResult> VerifyInfor([FromQuery] string cccd)
        {
            var dtos = await _accessRequestService.VerifyInfor(cccd);
            return Ok(dtos);
        }



    }

}
