using APICapacitacion.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APICapacitacion.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        Task<Boolean> RegistrarPelicula(Pelicula pelicula);
        Task<List<Pelicula>> ObtenerPeliculasGenero();
        Task<IEnumerable<object>> ObtenerPeliculasDiccionario();
    }
}
