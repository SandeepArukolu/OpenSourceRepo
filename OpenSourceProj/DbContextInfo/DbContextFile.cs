using Microsoft.EntityFrameworkCore;
using OpenSourceProj.DbTables;

namespace OpenSourceProj.DbContextInfo
{
    public class DbContextFile : DbContext
    {
        public DbContextFile(DbContextOptions options)
           : base(options)
        {
        }
        public DbSet<UserInfo> Users { get; set; }
    }
}
