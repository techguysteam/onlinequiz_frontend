using Microsoft.IdentityModel.Tokens;
using ThiHuong.Framework.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ThiHuong.Framework.Models;

namespace ThiHuong.Framework.Helpers
{
    public class JwtSecurityTokenProvider
    {
        public ExtensionSettings extensionSettings;

        public JwtSecurityTokenProvider(ExtensionSettings extensionSettings)
        {
            this.extensionSettings = extensionSettings;
        }

        public string CreateAccesstoken(Account user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(extensionSettings.AppSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.UserId, user.Id.ToString()),
                    new Claim(ClaimTypes.Username ,user.Username),
                    new Claim(ClaimTypes.Roles, user.Role.Name),
                }),

                Expires = DateTime.UtcNow.AddHours(extensionSettings.AppSettings.TokenExpireTime),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        

    }
}
