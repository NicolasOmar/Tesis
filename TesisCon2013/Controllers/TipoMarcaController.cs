using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisCon2013.Models;

namespace TesisCon2013.Controllers
{
    public class TipoMarcaController : Controller
    {
        MethodController Metodo = new MethodController();
        bdServTecDataContext bdCargada = new bdServTecDataContext();

        public ActionResult Index()
        {
            Metodo.LimpiarSesiones();

            Session["lstTipos"] = bdCargada.Tipos.OrderBy(T => T.tipo).ToList();
            Session["lstMarcas"] = bdCargada.Marcas.OrderBy(M => M.marca).ToList();
            Session["lstTiMa"] = bdCargada.TipoMarca.OrderBy(TM => TM.Marcas.marca).ToList();

            return View();
        }
        
        [HttpGet]
        public ActionResult FiltrarTiMa(int idTipo, int idMarca)
        {
            var dbFiltrada = bdCargada.TipoMarca.Select(TM => TM);
            int canTotal = dbFiltrada.ToList().Count;

            var lstTipos = Session["lstTipos"] as List<Tipos>;
            var lstMarcas = Session["lstMarcas"] as List<Marcas>;

            TipoMarca ejemplo = null;
            Tipos ejTipo = null;
            Marcas ejMarca = null;

            if (idTipo > 0)
            {
                ejTipo = lstTipos.Single(T => T.idTipo == idTipo);
                dbFiltrada = dbFiltrada.Where(TM => TM.idTipo == idTipo);
            }

            if (idMarca > 0)
            {
                dbFiltrada = dbFiltrada.Where(TM => TM.idMarca == idMarca);
                ejMarca = lstMarcas.Single(M => M.idMarca == idMarca);
            }
            
            if (dbFiltrada.ToList().Count > 0 && dbFiltrada.ToList().Count < canTotal)
            {
                ejemplo = dbFiltrada.ToList().ElementAt(0);

                if (idTipo > 0 && idMarca > 0)
                {
                    TipoMarca TiMaMod = bdCargada.TipoMarca.Single(TM => TM.idTipo == ejTipo.idTipo && TM.idMarca == ejMarca.idMarca);
                    Session["TiMaMod"] = TiMaMod;
                    TempData["mensaje"] = "La relacion " + ejemplo.Tipos.tipo + " | " + ejemplo.Marcas.marca + " esta registrada en la base de datos";
                    Session["filtrado"] = 3;
                }
                else
                    if (idTipo > 0)
                    {
                        TempData["mensaje"] = "Los equipos de tipo " + ejemplo.Tipos.tipo + " tiene " + dbFiltrada.ToList().Count + " marca/s relacionadas";
                        Session["lstTiMa"] = dbFiltrada.OrderBy(TM => TM.Marcas.marca).ToList();
                        Session["filtrado"] = 1;
                    }
                    else
                        if (idMarca > 0)
                        {
                            TempData["mensaje"] = "La marca " + ejemplo.Marcas.marca + " tiene " + dbFiltrada.ToList().Count + " tipo/s de equipo relacionados";
                            Session["lstTiMa"] = dbFiltrada.OrderBy(TM => TM.Tipos.tipo).ToList();
                            Session["filtrado"] = 2;
                        }
                
                return RedirectToAction("ListaTima");
            }
            else
                if (dbFiltrada.ToList().Count == 0)
                {
                    if (idTipo > 0 && idMarca > 0)
                    {
                        TempData["mensaje"] = "La relacion " + ejTipo.tipo + " | " + ejMarca.marca + " NO esta registrada en la base de datos";
                        Session["filtrado"] = 3;
                        return RedirectToAction("ListaTima");
                    }
                    else
                        if (idTipo > 0)
                        {
                            TempData["mensaje"] = "Los equipos de tipo " + ejTipo.tipo + " NO tiene marcas asignadas. Porfavor registre alguna relacion Tipo|Marca";
                            return RedirectToAction("Index");
                        }
                        else
                            if(idMarca > 0)
                            {
                                TempData["mensaje"] = "La marca " + ejMarca.marca + " NO tiene tipos de equipos asignados. Porfavor registre alguna relacion Tipo|Marca";
                                return RedirectToAction("Index");
                            }
                }
                else
                    if(dbFiltrada.ToList().Count == canTotal)
                        TempData["mensaje"] = "No se ha seleccionado ningun tipo o marca. Porfavor, seleccione alguna";
                    
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ListaTiMa()
        { return View(); }
        
        [HttpGet]
        public ActionResult RegistrarTipo()
        { return View(); }

        [HttpPost]
        public ActionResult RegistrarTipo(string txtTipo, string confirm)
        {
            var arrayTipos = bdCargada.Tipos.ToArray();
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;

            if (UsuarioLog.passLog.Equals(confirm))
            {
                for (int i = 0; i < arrayTipos.Length; i++)
                {
                    Tipos tipo = arrayTipos[i];
                    if (tipo.tipo.ToLower() == txtTipo.ToLower())
                    {
                        TempData["mensaje"] = "Ya hay un tipo de equipo registrado con el mismo nombre que intento ingresar";
                        return RedirectToAction("RegistrarTipo");
                    }
                }

                Tipos nuevoTipo = new Tipos() { tipo = Metodo.Modificador(txtTipo), };

                bdCargada.Tipos.InsertOnSubmit(nuevoTipo);
                bdCargada.SubmitChanges();
                Session["lstTipos"] = bdCargada.Tipos.ToList();

                TempData["mensaje"] = "Se ha registrado un nuevo tipo de equipo";
                return View("RegistrarRelacion");
            }
            else
            {
                TempData["mensaje"] = "La contraseña ingresada debe coincidir con la del usuario logueado";
                return RedirectToAction("RegistrarTipo");
            }
        }
        
        [HttpGet]
        public ActionResult RegistrarMarca()
        { return View(); }

        [HttpPost]
        public ActionResult RegistrarMarca(string txtMarca, string confirm)
        {
            var arrayMarcas = bdCargada.Marcas.ToArray();
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;

            if (UsuarioLog.passLog.Equals(confirm))
            {
                for (int i = 0; i < arrayMarcas.Length; i++)
                {
                    Marcas marca = arrayMarcas[i];
                    if (marca.marca.ToLower() == txtMarca.ToLower())
                    {
                        TempData["mensaje"] = "Ya hay una marca registrada con el mismo nombre que intento ingresar";
                        return RedirectToAction("RegistrarMarca");
                    }
                }

                Marcas nuevaMarca = new Marcas() { marca = Metodo.Modificador(txtMarca), };

                bdCargada.Marcas.InsertOnSubmit(nuevaMarca);
                bdCargada.SubmitChanges();
                Session["lstMarcas"] = bdCargada.Marcas.ToList();

                return View("RegistrarRelacion");
            }
            else
            {
                TempData["mensaje"] = "La contraseña ingresada debe coincidir con la del usuario logueado";
                return RedirectToAction("RegistrarMarca");
            }
        }

        [HttpGet]
        public ActionResult RegistrarRelacion()
        {
            if (Session["lstTipos"] == null)
                Session["lstTipos"] = bdCargada.Tipos.OrderBy(T => T.tipo).ToList();
            if (Session["lstMarcas"] == null)
                Session["lstMarcas"] = bdCargada.Marcas.OrderBy(M => M.marca).ToList();
            if (Session["lstTiMa"] == null)
                Session["lstTiMa"] = bdCargada.TipoMarca.OrderBy(TM => TM.Marcas.marca).ToList();

            return View(); 
        }

        [HttpPost]
        public ActionResult RegistrarRelacion(TipoMarca relacion, string confirm)
        {
            var arrayTiMa = bdCargada.TipoMarca.ToArray();
            Usuarios UsuarioLog = Session["UsuarioLog"] as Models.Usuarios;

            if (UsuarioLog.passLog.Equals(confirm))
            {
                for (int i = 0; i < arrayTiMa.Length; i++)
                {
                    TipoMarca consultado = arrayTiMa[i];
                    if (consultado.idTipo == relacion.idTipo && consultado.idMarca == relacion.idMarca)
                    {
                        TempData["mensaje"] = "Ya esta registrada la relacion Tipo/Marca que quiso ingresar";
                        return RedirectToAction("RegistrarRelacion");
                    }
                }

                bdCargada.TipoMarca.InsertOnSubmit(relacion);
                bdCargada.SubmitChanges();

                Session["lstTiMa"] = bdCargada.TipoMarca.OrderBy(TM => TM.Marcas.marca).ToList();
                TempData["mensaje"] = "Se ha registrado una nueva relacion Tipo|Marca";

                if (Convert.ToString(Session["interfaz"]).Equals("regOrden") || Convert.ToString(Session["interfaz"]).Equals("regEquipo"))
                    return RedirectToAction("RegistrarEquipo", "Equipo");
                else
                    if (Convert.ToString(Session["interfaz"]).Equals("modEquipo"))
                        return RedirectToAction("ModificarEquipo", "Equipo");

                return RedirectToAction("Index");
            }
            else
            {
                TempData["mensaje"] = "La contraseña ingresada debe coincidir con la del usuario logueado";
                return RedirectToAction("RegistrarRelacion");
            }
        }
        
        [HttpGet]
        public ActionResult ModificarTipo(int ID)
        {
            Tipos unico = bdCargada.Tipos.Single(T => T.idTipo == ID);
            Session["TipoMod"] = unico;
            return View();
        }

        [HttpPost]
        public ActionResult ModificarTipo(string nuevo, string confirm)
        {
            var arrayTipos = bdCargada.Tipos.ToArray();
            Tipos TipoMod = Session["TipoMod"] as Tipos;
            Tipos unico = bdCargada.Tipos.Single(T => T.idTipo == TipoMod.idTipo);
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;

            if (UsuarioLog.passLog.Equals(confirm))
            {
                for (int i = 0; i < arrayTipos.Length; i++)
                {
                    Tipos tipoA = arrayTipos[i];
                    if (tipoA.tipo.ToLower().Equals(nuevo.ToLower()))
                    {
                        TempData["mensaje"] = "Ya hay un tipo de equipo registrado con el nombre que quiso ingresar. Porfavor. Intente con otro";
                        return RedirectToAction("ModificarTipo");
                    }
                }

                unico.tipo = Metodo.Modificador(nuevo);
                bdCargada.SubmitChanges();
                recargarLista(unico, null);

                TempData["mensaje"] = "Se ha modificado el tipo de equipo " + unico.tipo;
                return RedirectToAction("ListaTiMa");
            }
            else
            {
                TempData["mensaje"] = "La contraseña ingresada debe coincidir con la del usuario logueado";
                return RedirectToAction("ModificarTipo");
            }
        }
        
        [HttpGet]
        public ActionResult ModificarMarca(int ID)
        {
            Marcas unico = bdCargada.Marcas.Single(M => M.idMarca == ID);
            Session["MarcaMod"] = unico;
            return View(); 
        }

        [HttpPost]
        public ActionResult ModificarMarca(string nuevo, string confirm)
        {
            var arrayMarcas = bdCargada.Marcas.ToArray();
            Marcas MarcaMod = Session["MarcaMod"] as Marcas;
            Marcas unico = bdCargada.Marcas.Single(T => T.idMarca == MarcaMod.idMarca);
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;

            if (UsuarioLog.passLog.Equals(confirm))
            {
                for (int i = 0; i < arrayMarcas.Length; i++)
                {
                    Marcas marcaA = arrayMarcas[i];
                    if (marcaA.marca.ToLower().Equals(nuevo.ToLower()))
                    {
                        TempData["mensaje"] = "Ya hay una marca de equipo registrada con el nombre que quiso ingresar. Porfavor. Intente con otro";
                        return RedirectToAction("ModificarMarca");
                    }
                }

                unico.marca = Metodo.Modificador(nuevo);
                bdCargada.SubmitChanges();
                recargarLista(null, unico);

                TempData["mensaje"] = "Se ha modificado la marca de equipo " + unico.marca;
                return RedirectToAction("ListaTiMa");
            }
            else
            {
                TempData["mensaje"] = "La contraseña ingresada debe coincidir con la del usuario logueado";
                return RedirectToAction("ModificarMarca");
            }
        }

        public ActionResult EliminarRelacion(int ID)
        {
            TipoMarca unico = bdCargada.TipoMarca.Single(TM => TM.idTipoMarca == ID);
            TempData["mensaje"] = "Se ha eliminado la relacion " + unico.Tipos.tipo + "|" + unico.Marcas.marca + " de la base de datos";

            bdCargada.TipoMarca.DeleteOnSubmit(unico);
            bdCargada.SubmitChanges();

            Session.Remove("TiMaMod");
            unico = null;
            Session["lstTiMa"] = bdCargada.TipoMarca.OrderBy(TM => TM.Marcas.marca).ToList();            

            return RedirectToAction("Index");
        }

        public ActionResult EliminarTipo(int ID)
        {
            var lstTiMa = bdCargada.TipoMarca.ToList();
            Tipos unico = bdCargada.Tipos.Single(T => T.idTipo == ID);

            foreach (var tipoMarca in lstTiMa)
            {
                TipoMarca eliminar = bdCargada.TipoMarca.Single(TM => TM.idTipoMarca == tipoMarca.idTipoMarca);
                if (tipoMarca.idTipo == unico.idTipo)
                {
                    bdCargada.TipoMarca.DeleteOnSubmit(eliminar);
                    bdCargada.SubmitChanges();
                }
                eliminar = null;
            }

            bdCargada.Tipos.DeleteOnSubmit(unico);
            bdCargada.SubmitChanges();

            lstTiMa = null;

            TempData["mensaje"] = "Se ha eliminado el Tipo de Equipo " + unico.tipo + " de la base de datos como asi tambien sus relaciones";
            unico = null;

            return RedirectToAction("Index");
        }

        public ActionResult EliminarMarca(int ID)
        {
            var lstTiMa = bdCargada.TipoMarca.ToList();
            Marcas unico = bdCargada.Marcas.Single(M => M.idMarca == ID);

            foreach (var tipoMarca in lstTiMa)
            {
                TipoMarca eliminar = bdCargada.TipoMarca.Single(TM => TM.idTipoMarca == tipoMarca.idTipoMarca);
                if (tipoMarca.idMarca == unico.idMarca)
                {
                    bdCargada.TipoMarca.DeleteOnSubmit(eliminar);
                    bdCargada.SubmitChanges();
                }
                eliminar = null;
            }

            bdCargada.Marcas.DeleteOnSubmit(unico);
            bdCargada.SubmitChanges();

            lstTiMa = null;

            TempData["mensaje"] = "Se ha eliminado la Marca " + unico.marca + " de la base de datos como asi tambien sus relaciones";
            unico = null;

            return RedirectToAction("Index");
        }

        void recargarLista(Tipos tElegido, Marcas mElegida)
        {
            var lstTiMa = Session["lstTima"] as List<Models.TipoMarca>;
            var tipoMarcaArray = lstTiMa.ToArray();

            if (tElegido != null)
            {    
                for (int i = 0; i < tipoMarcaArray.Length; i++)
                {
                    TipoMarca tiMa = tipoMarcaArray[i];
                    if (tiMa.idTipo == (int)tElegido.idTipo)
                    {
                        tiMa.idTipo = tElegido.idTipo;
                        tiMa.Tipos.tipo = tElegido.tipo;
                        tipoMarcaArray[i] = tiMa;
                        Session["lstTima"] = tipoMarcaArray.ToList();
                        Session.Remove("TipoMod");
                    }
                }
            }
            else
                if (mElegida != null)
                {
                    for (int i = 0; i < tipoMarcaArray.Length; i++)
                    {
                        TipoMarca tiMa = tipoMarcaArray[i];
                        if (tiMa.idMarca == mElegida.idMarca)
                        {
                            tiMa.idMarca = mElegida.idMarca;
                            tiMa.Marcas.marca = mElegida.marca;
                            tipoMarcaArray[i] = tiMa;
                            Session["lstTima"] = tipoMarcaArray.ToList();
                            Session.Remove("MarcaMod");
                        }
                    }
                }
        }
    }
}
