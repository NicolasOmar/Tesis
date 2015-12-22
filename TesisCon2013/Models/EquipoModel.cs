using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesisCon2013.Models
{
    public class EquipoModel
    {
        public int ID { set; get; }
        public string tipo { set; get; }
        public string marca { set; get; }
        public string modelo { set; get; }
        public string numSerie { set; get; }
        public string cliente { set; get; }
    }
}