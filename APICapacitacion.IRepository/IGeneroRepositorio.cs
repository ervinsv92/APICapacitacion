using APICapacitacion.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APICapacitacion.IRepositorio
{
    public interface IGeneroRepositorio
    {
        //TODO: 3- Interface de repositorio de genero
        Task RegistrarGenero(Genero genero);
        Task ActualizarGenero(Genero genero);
        Task EliminarGeneroPorId(int Id);
        Task<Genero> ObtenerGeneroPorId(int Id);
        Task<Genero> ObtenerGeneroPorId_FN(int Id);
        Task<List<Genero>> ObtenerListaGeneros();
        Task<List<string>> Test(int param);
    }
}
