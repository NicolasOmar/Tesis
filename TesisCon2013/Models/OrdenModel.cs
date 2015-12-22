using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesisCon2013.Models
{
    public class OrdenModel
    {
        public int ID { set; get; }
        public string cliente { set; get; }
        public string equipo { set; get; }
        public string falla { set; get; }
        public string observaciones { set; get; }
        public DateTime fecRecibido { set; get; }
        public string estado { set; get; }
        public bool aviso { set; get; }
        public DateTime? fecAviso { set; get; }
        public DateTime? fecEntregado { set; get; }
    }
}