using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TesisCon2013.Models
{
    public class ClienteModel
    {
        public int ID { set; get; }
        public string nombre { set; get; }
        public string apellido { set; get; }
        public string direccion { set; get; }
        public string barrio { set; get; }
        public string telefono { set; get; }
    }
}