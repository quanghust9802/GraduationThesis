using AccessControllApplication.Controllers;
using Application.Services.NotificationService;
using Microsoft.AspNetCore.Mvc;

namespace AccessControllApp.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int userId)
        {
            var dtos = await _notificationService.GetNotificationsByUserId(userId);
            return Ok(dtos);
        }





    }

}
