using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenSourceProj.DbTables
{
    public class UserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; } 
        public string FullName { get; set; }= string.Empty;
        public string Email { get; set; }= string.Empty;
        public string Password { get; set; } = string.Empty;    
        public string RePassword { get; set; }=string.Empty;
        public long MobileNo { get; set; } 
}
}
