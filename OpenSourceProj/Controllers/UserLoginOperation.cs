using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenSourceProj.DataAccess;
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
        private readonly ILogger<UserLoginOperation> _logger;
        public UserLoginOperation(IUserLoginAppService userRepostiory, IJwtService jwtService,ILogger<UserLoginOperation> logger)
        {
            _userRepostiory = userRepostiory;
            _jwtService = jwtService;
            _logger= logger;
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
            throw new DivideByZeroException();
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


        [HttpGet("GetResultById/{id:int}")]
        public IActionResult GetResultById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet()]
        public IActionResult GetResultById()
        {           
            return Ok();
        }
    }
}
