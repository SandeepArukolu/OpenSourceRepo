using OpenSourceProj.DbTables;
using OpenSourceProj.Modals;

namespace OpenSourceProj.Repositorys
{
    public interface IUserLoginAppService
    {
       UserInfo GetLoginInfo(LoginModal loginModal);
        Task SaveUser(UserInfo userinfo);
    }
}
