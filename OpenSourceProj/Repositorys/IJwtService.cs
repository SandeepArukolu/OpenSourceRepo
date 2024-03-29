using OpenSourceProj.DbTables;

namespace OpenSourceProj.Repositorys
{
    public interface IJwtService
    {
        public string GenerateToken(UserInfo userinfo);
    }
}
