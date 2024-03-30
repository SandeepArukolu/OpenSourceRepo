using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenSourceProj.DbTables;
using OpenSourceProj.Modals;
using OpenSourceProj.Repositorys;

namespace OpenSourceProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginOperation : ControllerBase
    {
        private readonly IUserLoginAppService _userRepostiory;
        private readonly IJwtService _jwtService;
        public UserLoginOperation(IUserLoginAppService userRepostiory, IJwtService jwtService)
        {
            _userRepostiory = userRepostiory;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("UserLogin")]
        public async Task<IActionResult> Login(LoginModal loginModal)
        {
            var response = await _userRepostiory.GetLoginInfo(loginModal);
            if (response != null)
            {
                string Token = await _jwtService.GenerateToken(response);
                return Ok(Token);
            }
            return Ok();
        }
        [HttpPost]
        [Route("UserSignIn")]
        [AllowAnonymous]
        public async Task<IActionResult> register(UserInfo userinfo)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var response = await _userRepostiory.SaveUser(userinfo, cancellationTokenSource.Token);
            return Ok();
        }
    }
}
