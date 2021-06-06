using System;
using System.Collections.Generic;
using System.Text;

namespace APICapacitacion.Model
{
    public class Pelicula
    {
        public int Id { get; set; }
        public String Titulo { get; set; }
        public List<Genero> ListaGeneros { get; set; }

    }
}
