using Microsoft.AspNetCore.Mvc;
using UsersAPI.Models;
using UsersAPI.Services;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenExchangeService tokenExchange;
        public UsersController(IUserService userService, ITokenExchangeService tokenExchange)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.tokenExchange = tokenExchange;
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
        [HttpPost]
        public async Task<ActionResult> GenerateToken(AuthenticationModel request)
        {
            var authenticationResponse = await tokenExchange.AuthenticateUser(request);

            return Ok(authenticationResponse);
        }

    }

}
