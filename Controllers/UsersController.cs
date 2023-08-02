using Microsoft.AspNetCore.Mvc;
using UsersAPI.Services;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost]
        public async Task<ActionResult> BulkUserUpload(IFormFile usersFile)
        {
            bool result = await _userService.BulkUserUpload(usersFile);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }


    }

}
