using OpenSourceProj.DbTables;

namespace OpenSourceProj.Repositorys
{
    public interface IJwtService
    {
        Task<string> GenerateToken(UserInfo userinfo);
    }
}
