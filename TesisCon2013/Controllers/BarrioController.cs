using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisCon2013.Models;

namespace TesisCon2013.Controllers
{
    public class BarrioController : Controller
    {
        MethodController Metodo = new MethodController();
        bdServTecDataContext bdCargada = new bdServTecDataContext();

        public ActionResult Index()
        {
            Metodo.LimpiarSesiones();
            return View();
        }

        [HttpGet]
        public ActionResult FiltrarBarrio(string txtBarrio)
        {
            var listaBarrios = bdCargada.Barrios.ToList();
            var dbFiltrada = listaBarrios.Select(B => B);
            int canTotal = listaBarrios.Count;
 
            dbFiltrada = dbFiltrada.Where(B => B.barrio.ToLower().Contains(txtBarrio.ToLower()));
            dbFiltrada = dbFiltrada.OrderByDescending(O => O.barrio);

            if (dbFiltrada.ToList().Count > 0 && dbFiltrada.ToList().Count < canTotal)
            {
                Session["lstBarrios"] = dbFiltrada.ToList();                    
                return View("ListaBarrios");
            }
            else
                TempData["mensaje"] = "No se han podido encontrar ningun Barrio con el nombre ingresado, intente con uno distinto";

            return RedirectToAction("Index", "Barrio");            
        }

        [HttpGet]
        public ActionResult ListaBarrios()
        { return View(); }

        [HttpGet]
        public ActionResult RegistrarBarrio()
        { return View(); }

        [HttpPost]
        public ActionResult RegistrarBarrio(Barrios nuevoBarrio, string confirm)
        {
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;
            var arrayBarrios = bdCargada.Barrios.ToArray();

            if (UsuarioLog.passLog.Equals(confirm))
            {
                for (int i = 0; i < arrayBarrios.Length; i++)
                {
                    Barrios barrioCargado = arrayBarrios[i];
                    if (barrioCargado.barrio.ToLower().Equals(nuevoBarrio.barrio.ToLower()))
                    {
                        TempData["mensaje"] = "Ya hay un barrio con el nombre que quiso registrar, intente con uno distinto";
                        return RedirectToAction("RegistrarBarrio");
                    }
                }

                nuevoBarrio.barrio = Metodo.Modificador(nuevoBarrio.barrio);

                bdCargada.Barrios.InsertOnSubmit(nuevoBarrio);
                bdCargada.SubmitChanges();
                nuevaLista(nuevoBarrio);

                TempData["mensaje"] = "Se ha registrado un nuevo barrio";

                if (Session["interfaz"] != null)
                {
                    Session["lstBarrios"] = bdCargada.Barrios.OrderBy(B => B.barrio).ToList();

                    if (Convert.ToString(Session["interfaz"]).Equals("regUsuario"))
                        return RedirectToAction("RegistrarUsuario", "Usuario");
                    else
                        if (Convert.ToString(Session["interfaz"]).Equals("modUsuario"))
                            return RedirectToAction("ModificarUsuario", "Usuario");
                        else
                            if (Convert.ToString(Session["interfaz"]).Equals("regCliente") || Convert.ToString(Session["interfaz"]).Equals("regOrden") || Convert.ToString(Session["interfaz"]).Equals("regEquipo"))
                                return RedirectToAction("RegistrarCliente", "Cliente");
                            else
                                if (Convert.ToString(Session["interfaz"]).Equals("modCliente"))
                                    return RedirectToAction("ModificarCliente", "Cliente");
                }
                else
                    return RedirectToAction("ListaBarrios");
            }
            else
                TempData["mensaje"] = "Debe ingresar la contraseña del usuario logueado para poder registrar un nuevo barrio";
            return RedirectToAction("RegistrarBarrio");
        }

        [HttpGet]
        public ActionResult ModificarBarrio(int ID)
        {
            Barrios unico = bdCargada.Barrios.Single(B => B.idBarrio == ID);
            Session["BarrioMod"] = unico;
            return View();
        }
        
        [HttpPost]
        public ActionResult ModificarBarrio(string txtBarrio, string confirm)
        {
            Barrios BarrioMod = Session["BarrioMod"] as Barrios;
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;

            Barrios unico = bdCargada.Barrios.Single(B => B.idBarrio == BarrioMod.idBarrio);
            var arrayBarrios = bdCargada.Barrios.ToArray();

            if (UsuarioLog.passLog.Equals(confirm))
            {
                for (int i = 0; i < arrayBarrios.Length; i++)
                {
                    Models.Barrios barrioCargado = arrayBarrios[i];
                    if (barrioCargado.barrio.ToLower().Equals(txtBarrio.ToLower()))
                    {
                        TempData["mensaje"] = "Ya hay un barrio con el nuevo nombre que quiso ingresar, intente con uno distinto";
                        return RedirectToAction("ModificarBarrio");
                    }
                }

                unico.barrio = Metodo.Modificador(txtBarrio);
                bdCargada.SubmitChanges();
                nuevaLista(unico);

                arrayBarrios = null;
                unico = null;
                Session.Remove("BarrioMod");
                    
                TempData["mensaje"] = "Se ha modificado el nombre de un barrio";
                return RedirectToAction("ListaBarrios");
            }
            else
                TempData["mensaje"] = "Debe ingresar la contraseña del usuario logueado para poder modificar el nombre del barrio";
            return RedirectToAction("ModificarBarrio");
        }

        [HttpGet]
        public ActionResult EliminarBarrio(int ID)
        {
            var lstBarrios = Session["lstBarrios"] as List<Barrios>;
            Barrios unico = bdCargada.Barrios.Single(B => B.idBarrio == ID);
            reducirLista(unico, lstBarrios);

            bdCargada.Barrios.DeleteOnSubmit(unico);
            bdCargada.SubmitChanges();
            lstBarrios = null;
            TempData["mensaje"] = "Se ha eliminado el barrio " + unico.barrio + " de la base de datos";

            if (lstBarrios.Count == 0)
                return RedirectToAction("Index");
            else
                return RedirectToAction("ListaBarrios");
        }

        public void nuevaLista(Barrios bElegido)
        {
            var nuevaLista = new List<Models.Barrios>();
            nuevaLista.Add(bElegido);
            Session["lstBarrios"] = nuevaLista;
            bElegido = null;

            Session.Remove("BarrioMod");
        }

        public void reducirLista(Barrios bElegido, List<Models.Barrios> listaBa)
        {
            var arrayBarrios = listaBa.ToArray();

            for (int i = 0; i < arrayBarrios.Length; i++)
            {
                Barrios barrioA = arrayBarrios[i];
                if (barrioA.idBarrio == bElegido.idBarrio)
                {
                    listaBa.Remove(barrioA);
                    Session["lstBarrios"] = listaBa;
                }
            }
        }
    }
}
