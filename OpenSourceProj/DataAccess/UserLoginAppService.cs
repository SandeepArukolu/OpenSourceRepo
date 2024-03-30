using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenSourceProj.DbContextInfo;
using OpenSourceProj.DbTables;
using OpenSourceProj.Modals;
using OpenSourceProj.Repositorys;
using System.Threading;
namespace OpenSourceProj.DataAccess
{
    public class UserLoginAppService : IUserLoginAppService
    {
        private readonly DbContextFile _dbcontext;

        public UserLoginAppService(DbContextFile dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<UserInfo> GetLoginInfo(LoginModal loginModal)
        {
            var list = _dbcontext.Users.Where(x => x.Email == loginModal.Email && x.Password == loginModal.Password).FirstOrDefault();
            return await Task.FromResult(list);
        }


        public async Task<bool> SaveUser(UserInfo userinfo, CancellationToken cancellationToken)
        {
            _dbcontext.Users?.AddAsync(userinfo);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            return true;


            //List<UserInfo> list = new List<UserInfo> {
            //    new UserInfo{FullName="sai",Email="sai@gmail.com",Password="snns",RePassword="dhhd",MobileNo=837733773},
            //    new UserInfo{FullName="sai",Email="sai@gmail.com",Password="snns",RePassword="dhhd",MobileNo=837733773}
            //}.ToList();
            // _dbcontext.Users.BulkInsertAsync(list);
            //_dbcontext.SaveChangesAsync();
        }
    }
}
