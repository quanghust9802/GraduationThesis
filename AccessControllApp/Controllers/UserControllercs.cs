using AccessControllApplication.Controllers;
using Application.DTOs.AuthDTOs;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Acces.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var dto = await _userService.GetByIdAsync(id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var dtos = await _userService.GetAllAsync();
            return Ok(dtos);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetAllUserRole()
        {
            var dtos = await _userService.GetAllUserRole();
            return Ok(dtos);
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginDTO dto)
        {
            var result = await _userService.LoginAsync(dto);
            return Ok(result);
        }

        [HttpPost("change_password")]
        [Authorize]
        public async Task<ActionResult<LoginResponse>> ChangePasswordAsync(ChangePasswordDTO dto)
        {
            var result = await _userService.ChangePasswordAsync(dto);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromForm] UserDTO dto, [FromForm] IFormFile imageUrl)
        {
            var res = await _userService.InsertAsync(dto, imageUrl);
            return Ok(res);
        }


        [HttpPut("disable/{id}")]
        [Authorize(Roles = nameof(UserRole.RoleType))]
        public async Task<IActionResult> DisableUserAsync(int id)
        {
            await _userService.DisableUserAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.RoleType))]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] UserDTO dto, [FromForm] IFormFile imageUrl)
        {
            var res = await _userService.UpdateAsync(id, dto, imageUrl);
            return Ok(res);
        }

    }

}
