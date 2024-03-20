using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenSourceProj.DbContextInfo;
using OpenSourceProj.DbTables;
using OpenSourceProj.Modals;

namespace OpenSourceProj.Repositorys
{
    public class UserLoginAppService:IUserLoginAppService
    {
        private readonly DbContextFile _dbcontext;

        public UserLoginAppService(DbContextFile dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public UserInfo GetLoginInfo(LoginModal loginModal)
        {         
         var list = _dbcontext.Users.Where(x => x.Email == loginModal.Email && x.Password == loginModal.Password).FirstOrDefault();
            return  list;
        }

        
        public async Task SaveUser(UserInfo userinfo)
        {
            var reg = _dbcontext.Users.AddAsync(userinfo);
            await _dbcontext.SaveChangesAsync();

           
            //List<UserInfo> list = new List<UserInfo> { 

            //    new UserInfo{FullName="sai",Email="sai@gmail.com",Password="snns",RePassword="dhhd",MobileNo=837733773}
            //}.ToList();


            //await _dbcontext.Users.BulkInsertAsync(list);

        }
    }
}
