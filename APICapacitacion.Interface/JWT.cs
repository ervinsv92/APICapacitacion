using System;
using System.Collections.Generic;
using System.Text;

namespace APICapacitacion.Clases
{
    public class JWT
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
