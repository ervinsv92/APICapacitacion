using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APICapacitacion.Clases
{
    public class HelperJWT : IHelperJWT
    {
        private const string KEY_CLAIM = "JWTClaim";
        private readonly IConfiguration _configuration;

        public HelperJWT(IConfiguration configuration) {
            this._configuration = configuration;
        }
        public JWT BuildToken(JWTClaim jwtClaim)
        {
            var claims = new[] {
                new Claim(KEY_CLAIM, JsonConvert.SerializeObject(jwtClaim))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //TODO: Expiración del token
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new JWT()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        public JWTClaim ObtenerJWTClaim(ClaimsPrincipal claim) {
            string c = claim.FindFirstValue(KEY_CLAIM);

            if (c != null)
            {
                return JsonConvert.DeserializeObject<JWTClaim>(c);
            }
            else {
                return null;
            }
        }
    }
}
