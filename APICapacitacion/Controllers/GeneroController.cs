using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using APICapacitacion.Clases;
using APICapacitacion.IRepositorio;
using APICapacitacion.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICapacitacion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GeneroController : ControllerBase
    {
        //TODO: 5- Controlador del repositorio de genero
        private readonly IGeneroRepositorio _generoRespositorio;
        private readonly IHelperJWT _helperJwt;

        public GeneroController(IHelperJWT helperJwt, IGeneroRepositorio generoRepositorio)
        {
            this._helperJwt = helperJwt;
            this._generoRespositorio = generoRepositorio;
        }


        [HttpPost]
        public async Task<ActionResult> RegistrarGenero([FromBody] Genero genero)
        {
            try
            {
                await this._generoRespositorio.RegistrarGenero(genero);
                return Ok();
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarGenero([FromBody] Genero genero)
        {
            try
            {
                await this._generoRespositorio.ActualizarGenero(genero);
                return Ok();
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }

        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Genero>> ObtenerGeneroPorId([FromRoute] int Id)
        {
            try
            {
                Genero genero = await this._generoRespositorio.ObtenerGeneroPorId(Id);

                if (genero == null)
                {
                    return NotFound();
                }

                return genero;
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }

        [HttpGet("{Id}/fn")]
        public async Task<ActionResult<Genero>> ObtenerGeneroPorId_FN([FromRoute] int Id)
        {
            try
            {
                Genero genero = await this._generoRespositorio.ObtenerGeneroPorId_FN(Id);

                if (genero == null)
                {
                    return NotFound();
                }

                return genero;
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }

        [HttpGet()]
        public async Task<ActionResult<List<Genero>>> ObtenerGeneros()
        {
            try
            {
                //TODO: Ejemplo de como leer los claims del token, User es un objeto que se encuentra en la case ControlBase, se llena con el token que realiza la peticion
                JWTClaim claim = this._helperJwt.ObtenerJWTClaim(User);
                List<Genero> listaGeneros = await this._generoRespositorio.ObtenerListaGeneros();

                if (listaGeneros == null)
                {
                    return NotFound();
                }

                return listaGeneros;
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Genero>> EliminarGeneroPorId([FromQuery] int Id)
        {
            try
            {
                await this._generoRespositorio.EliminarGeneroPorId(Id);
                return Ok();
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }
    }
}
