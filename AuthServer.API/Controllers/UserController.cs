using AuthServer.Core.Dtos.UserDtos;
using AuthServer.Core.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto userDto)
        {
            var result = await _userService.CreateUserAsync(userDto);
            return CreateCustomResult(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserByName()
        {
            var userName = HttpContext.User.Identity.Name;
            var result = await _userService.GetUserByName(userName);
            return CreateCustomResult(result);
        }
    }
}
