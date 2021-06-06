using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    //TODO: Se agrega el middleware del JWT
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeliculaController : ControllerBase
    {
        private readonly IPeliculaRepositorio _peliculaRespositorio;

        public PeliculaController(IPeliculaRepositorio peliculaRespositorio)
        {
            this._peliculaRespositorio = peliculaRespositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> ObtenerPeliculasDiccionario()
        {
            try
            {
                return Ok(await _peliculaRespositorio.ObtenerPeliculasDiccionario());
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }

        }

        [HttpGet("obtener_peliculas_genero")]
        public async Task<ActionResult<List<Pelicula>>> ObtenerPeliculasGenero()
        {
            try
            {
                return Ok(await _peliculaRespositorio.ObtenerPeliculasGenero());
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }

        }

        [HttpPost("registrar_pelicula_genero")]
        public async Task<ActionResult<Boolean>> RegistrarPeliculasGenero(Pelicula pelicula)
        {
            try
            {
                return Ok(await _peliculaRespositorio.RegistrarPelicula(pelicula));
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }

        }
    }
}
