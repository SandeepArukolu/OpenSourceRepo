using OpenSourceProj.DbTables;
using OpenSourceProj.Modals;

namespace OpenSourceProj.Repositorys
{
    public interface IUserLoginAppService
    {
       Task<UserInfo> GetLoginInfo(LoginModal loginModal);
        Task<bool> SaveUser(UserInfo userinfo, CancellationToken cancellationToken);
    }
}
