using AccessControllApplication.Controllers;
using Application.DTOs.AccessLogDTOs;
using Application.Services.AccessLogServices;
using Microsoft.AspNetCore.Mvc;

namespace AccessControllApp.Controllers
{
    [Route("api/access-log")]
    [ApiController]
    public class AccessLogController : BaseController
    {
        private readonly IAccessLogService _accessLogService;
        public AccessLogController(IAccessLogService accessLogService)
        {
            _accessLogService = accessLogService;
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _accessLogService.GetByIdAsync(id);
            return Ok(dto);
        }

        [HttpGet("get-by-userid")]
        //[Authorize]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var dto = await _accessLogService.GetByIdAsync(userId);
            return Ok(dto);
        }

        [HttpGet("get-by-requestid")]
        public async Task<IActionResult> GetByRequestId(int requestId)
        {
            var dtos = await _accessLogService.GetByRequestIdAsync(requestId);
            return Ok(dtos);
        }


        [HttpPost("create")]
        public async Task<IActionResult> InsertAsync([FromForm] AccessLogDTO dto)
        {
            var res = await _accessLogService.InsertAsync(dto);
            return Ok(res);
        }


        [HttpGet("get-filter")]
        public async Task<IActionResult> GetFilter([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int? requestId, [FromQuery] int? userId)
        {
            var dtos = await _accessLogService.GetByFilterAsync(startDate, endDate, requestId, userId);
            return Ok(dtos);
        }

    }

}
