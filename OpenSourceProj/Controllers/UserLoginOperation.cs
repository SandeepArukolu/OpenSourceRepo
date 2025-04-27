using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OpenSourceProj.DataAccess;
using OpenSourceProj.DateFilterGenericRepo;
using OpenSourceProj.DateFilterGenericRepo.GenericRepoService;
using OpenSourceProj.DateFilterGenericRepo.IGenericService;
using OpenSourceProj.DbTables;
using OpenSourceProj.Migrations;
using OpenSourceProj.Modals;
using OpenSourceProj.Repositorys;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OpenSourceProj.Controllers
{
    //[Authorize]
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

        [HttpPost("UserLogin")]  
        public async Task<IActionResult> Login(LoginModal loginModal)
        {         
            var response = await _userRepostiory.GetLoginInfo(loginModal);
            if (response != null)
            {
                string token =  await _jwtService.GenerateToken(response);

                return Ok(new { Token = token });
            }
            return Ok(new { Token = string.Empty });
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


        [HttpPost("GetResultById/{id:int}")]
        [Authorize]
        public IActionResult GetResultById(int id)
        {
            return Ok(id);
        }

    }
}
