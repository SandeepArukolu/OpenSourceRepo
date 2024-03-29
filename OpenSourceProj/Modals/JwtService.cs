using Microsoft.IdentityModel.Tokens;
using OpenSourceProj.DbTables;
using OpenSourceProj.Repositorys;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenSourceProj.Modals
{
    public class JwtService : IJwtService
    {
        public string? Secretkey { get; set; }

        public int TokenDuration { get; set; }

        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration _config)
        {
            _configuration = _config;
            this.Secretkey = _config.GetSection("JwtConfig").GetSection("Key").Value;
            this.TokenDuration = Int32.Parse(_config.GetSection("JwtConfig").GetSection("Duration").Value);
        }

        public string GenerateToken(UserInfo userinfo)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Secretkey));
            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var payload = new[]
            {
                new Claim("FullName",userinfo.FullName),
                new Claim("UserId",userinfo.UserId.ToString()),
                new Claim("Email", userinfo.Email),
                new Claim("MobileNo",userinfo.MobileNo.ToString()),
            };
            var jwtToken = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: payload,
                expires: DateTime.Now.AddMinutes(TokenDuration),
                signingCredentials: signature
                );
           return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
