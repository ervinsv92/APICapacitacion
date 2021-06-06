using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace APICapacitacion.Clases
{
    public interface IHelperJWT
    {
        public JWT BuildToken(JWTClaim jwtClaim);
        public JWTClaim ObtenerJWTClaim(ClaimsPrincipal claim);
    }
}
