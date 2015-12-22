using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesisCon2013.Models
{
    public class UsuarioModel
    {
        public int ID { set; get; }
        public string nombre { set; get; }
        public string direccion { set; get; }
        public string barrio { set; get; }
        public string telefono { set; get; }
        public string logueo { set; get; }
        public string password { set; get; }
        public string rol { set; get; }
        public bool habilitar { set; get; }
    }
}