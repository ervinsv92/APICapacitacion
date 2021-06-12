using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICapacitacion.Clases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICapacitacion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionController : ControllerBase
    {
        private readonly IHelperJWT _helperJWT;

        public SesionController(IHelperJWT helperJwt) {
            this._helperJWT = helperJwt;
        }

        /// <summary>
        /// Login de la aplicacion, si es correcto se genera y devuelve el token para utilizar en las demas funciones del API
        /// </summary>
        /// <param name="usuario">Usuario suministrado para ingresar al API</param>
        /// <param name="clave">Clave suministrada para acceder al API</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<JWT>> Login(Usuario usuario)
        {
            JWTClaim jwtClaim = new JWTClaim();
            jwtClaim.IdUser = 13;

            if (usuario.usuario.ToLower() == "ervin")
            {
                return _helperJWT.BuildToken(jwtClaim);
            }
            else
            {
                return NotFound("Usuario o contraseña incorrectos.");
            }
        }
    }
}
