using Microsoft.AspNetCore.Mvc;

namespace AccessControllApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public string UserName => Convert.ToString(User.Claims.First(c => c.Type == "UserName").Value);

        public int UserId => Int16.Parse(User.Claims.First(c => c.Type == "UserId").Value);
        public int RoleId => int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "RoleType")?.Value, out var roleId) ? roleId : 0;

    }
}
