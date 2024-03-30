using Microsoft.IdentityModel.Tokens;
using OpenSourceProj.DbTables;
using OpenSourceProj.Repositorys;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenSourceProj.DataAccess
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration _config)
        {
            _configuration = _config;
        }
        public async Task<string> GenerateToken(UserInfo userinfo)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig").GetSection("Key").Value));
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
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration.GetSection("JwtConfig").GetSection("Duration").Value)),
                signingCredentials: signature
                );

            string FinalToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return await Task.FromResult(FinalToken);
        }
    }
}
