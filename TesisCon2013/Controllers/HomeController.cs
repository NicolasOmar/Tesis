using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisCon2013.Models;

namespace TesisCon2013.Controllers
{
    public class HomeController : Controller
    {
        MethodController Metodo = new MethodController();

        public ActionResult Index()
        {
            Metodo.LimpiarSesiones();
            return View();
        }
        //System.Web.HttpContext.Current.Session
    }
}
