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

        public UserLoginOperation(IUserLoginAppService userRepostiory)
        {
            _userRepostiory = userRepostiory;
        }

        [HttpPost]
        [Route("UserLogin")]
        public IActionResult Login(LoginModal loginModal)
        {
          var response=  _userRepostiory.GetLoginInfo(loginModal);
            return Ok(response);
        }
        [HttpPost]
        [Route("UserRegister")]
        public async Task<IActionResult> register(UserInfo userinfo)
        {
          var response =  _userRepostiory.SaveUser(userinfo);
            return Ok();
        }
        //[HttpGet("{id:int}")]
        //public IActionResult Test(int id)
        //{
        //    List<UserInfo> list = new List<UserInfo> {

        //        new UserInfo{FullName="sai",Email="sai@gmail.com",Password="snns",RePassword="dhhd",MobileNo=837733773}
        //    }.ToList();

        //    return Ok(list);
        //}
        //[HttpGet]
        //[Route("{x:alpha}")]
        //public IActionResult Test(string x)
        //{
        //    return Ok("string method executed");
        //}
    }
}
