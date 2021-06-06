using System;
using System.Collections.Generic;
using System.Text;

namespace APICapacitacion.Clases
{
    public class ConexionBD
    {
        public string StringConexion { get;}

        public ConexionBD(string Conexion) {
            this.StringConexion = Conexion;
        }
    }
}
