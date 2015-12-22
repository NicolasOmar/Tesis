using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisCon2013.Models;

namespace TesisCon2013.Controllers
{
    public class OrdenController : Controller
    {
        MethodController Metodo = new MethodController();
        bdServTecDataContext bdCargada = new bdServTecDataContext();
        
        public ActionResult Index()
        {
            Metodo.LimpiarSesiones();

            if (Session["interfaz"] == null)
                Session["interfaz"] = "consuOrden";

            Session["lstClientes"] = bdCargada.Clientes.OrderBy(C => C.apeClie).ToList();
            Session["lstEquipos"] = bdCargada.Equipos.OrderBy(E => E.idEquipo).ToList();
            Session["lstEstados"] = bdCargada.Estados.ToList();
            Session["lstTipos"] = bdCargada.Tipos.OrderBy(T => T.tipo).ToList();
            Session["lstTiMa"] = bdCargada.TipoMarca.OrderBy(TM => TM.Marcas.marca).ToList();
            Session["lstUsuarios"] = bdCargada.Usuarios.OrderBy(U => U.nomUsu).ToList();

            return View(); }

        [HttpGet]
        public ActionResult FiltrarOrden(string txtNumero, int idEquipo, string txtFalla, int idEstado, string fechaInicio, string fechaFin)
        {
            var listaClientes = bdCargada.Ordenes.ToList();
            var dbFiltrada = listaClientes.Select(O => O);
            int canTotal = listaClientes.Count;

            if (txtNumero.Length > 0)
                dbFiltrada = dbFiltrada.Where(O => O.idOrden == Convert.ToInt32(txtNumero));

            if (Session["ClienteMod"] == null)
            {
                if (idEquipo > 0)
                    dbFiltrada = dbFiltrada.Where(O => O.Equipos.idTipoMarca == idEquipo);
                Session.Remove("ClienteMod");
            }
            else
            {
                dbFiltrada = dbFiltrada.Where(O => O.idEquipo == idEquipo);

                Session.Remove("ClienteMod");
                Session.Remove("sirve");
            }

            if (txtFalla.Length > 0)
                dbFiltrada = dbFiltrada.Where(O => O.falla.ToLower().Contains(txtFalla.ToLower()));

            if (idEstado > 0)
                dbFiltrada = dbFiltrada.Where(O => O.idEstado == idEstado);

            if (!fechaInicio.Equals("") || Convert.ToDateTime(fechaFin) < DateTime.Today.Date)
            { 
                if(!fechaInicio.Equals(""))
                    dbFiltrada = dbFiltrada.Where(O => (DateTime)O.fecRecib.Value.Date >= DateTime.Parse(fechaInicio) && (DateTime)O.fecRecib.Value.Date <= DateTime.Parse(fechaFin)); 
                else
                    dbFiltrada = dbFiltrada.Where(O => (DateTime)O.fecRecib.Value.Date <= DateTime.Parse(fechaFin)); 
            }

            dbFiltrada = dbFiltrada.OrderByDescending(O => O.fecRecib).OrderByDescending(O => O.idOrden);

            if (dbFiltrada.ToList().Count > 0 && dbFiltrada.ToList().Count < canTotal)
            {
                var listaModel = dbFiltrada.Select(orden => new OrdenModel
                    {
                        ID = orden.idOrden,
                        cliente = orden.Equipos.Clientes.apeClie + ", " + orden.Equipos.Clientes.nomClie,
                        equipo = orden.Equipos.TipoMarca.Tipos.tipo + " " + orden.Equipos.TipoMarca.Marcas.marca,
                        falla = orden.falla,
                        observaciones = orden.observ,
                        fecRecibido = (DateTime)orden.fecRecib,
                        estado = orden.Estados.estado,
                        aviso = (bool)orden.aviso,
                        fecAviso = (DateTime?)orden.fecAviso,
                        fecEntregado = (DateTime?)orden.fecEntrega,
                    }).ToList();

                Session.Remove("interfaz");

                Session["lstOrdenes"] = listaModel;
                return View("ListaOrdenes");
            }
            else
                if (dbFiltrada.ToList().Count == canTotal)
                {
                    TempData["mensaje"] = "No ha seleccionado ninguna opcion para realizar la busqueda de Ordenes. Porfavor, seleccione al menos una";
                    return RedirectToAction("Index", "Orden");
                }
                else
                    if(dbFiltrada.ToList().Count == 0)
                    {
                        TempData["mensaje"] = "No se han podido encontrar ninguna Orden con los datos ingresados";
                        return RedirectToAction("Index", "Orden");
                    }

            return RedirectToAction("Index", "Orden");
        }

        [HttpGet]
        public ActionResult ListaOrdenes()
        { return View(); }

        [HttpGet]
        public ActionResult ConsultarOrden(int ID)
        {
            Ordenes unicaO = bdCargada.Ordenes.Single(O => O.idOrden == ID);
            var listaD = bdCargada.Tareas.Where(T => T.idOrden == unicaO.idOrden).OrderByDescending(T => T.fecTarea).OrderByDescending(T => T.idTarea);
            int canTareas = unicaO.Tareas.Count;

            var listaTM = listaD.Select(D => new TareaModel
            {
                detalle = D.detalles,
                tecnico = D.Usuarios.nomUsu,
                fecDetalle = (DateTime)D.fecTarea,
            }).ToList();

            Session["lsTareas"] = listaTM;
            Session["canTareas"] = canTareas;
            Session["OrdenMod"] = unicaO;

            return View();
        }

        [HttpGet]
        public ActionResult RegistrarOrden()
        {
            if (Session["interfaz"] == null || (Session["interfaz"] != null && Convert.ToString(Session["interfaz"]).Equals("consuOrden")))
                Session["interfaz"] = "regOrden";
            return View();
        }

        [HttpPost]
        public ActionResult BuscarCliente(string buscar)
        {
            var dbFiltrada = bdCargada.Clientes.Select(C => C);

            dbFiltrada = dbFiltrada.Where(C => C.apeClie.ToLower().Contains(buscar.ToLower()) || C.nomClie.ToLower().Contains(buscar.ToLower()));
            dbFiltrada = dbFiltrada.OrderBy(C => C.apeClie);

            if (dbFiltrada.ToList().Count > 0)
            {
                var listaCFiltrada = dbFiltrada.Select(C => new ClienteModel
                {
                    ID = C.idClie,
                    nombre = C.nomClie,
                    apellido = C.apeClie,
                    direccion = C.direClie,
                    barrio = C.Barrios.barrio,
                    telefono = C.telClie,
                }).ToList();

                Session["lstClientes"] = listaCFiltrada;
                return Json(new { result = true, url = Url.Action("ListaClientes", "Cliente") });
            }
            else
                if (Session["interfaz"] != null && Convert.ToString(Session["interfaz"]).Equals("regOrden"))
                    return Json(new { result = false, url = Url.Action("RegistrarOrden", "Orden") });
                else
                    return Json(new { result = false, url = Url.Action("Index", "Orden") });
        }

        [HttpGet]
        public ActionResult SeleccionarCliente(int ID)
        {
            Clientes unico = bdCargada.Clientes.Single(C => C.idClie == ID);
            var listaEquipos = bdCargada.Equipos.Where(E => E.idClie == ID).ToList();

            Session["lstEquipos"] = listaEquipos;
            Session["ClienteMod"] = unico;

            Session["sirve"] = true;

            if(Session["interfaz"] != null && Convert.ToString(Session["interfaz"]).Equals("regOrden"))
                return RedirectToAction("RegistrarOrden");
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RegistrarOrden(int idEquipo, string falla, string observ, string confirm)
        {
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;

            if (UsuarioLog.passLog.Equals(confirm))
            {
                if (idEquipo == 0)
                {
                    TempData["mensaje"] = "No ha seleccionado ningun equipo";
                    return RedirectToAction("RegistrarOrden", Session["sirve"] = true);
                }

                Ordenes nuevaOrden = new Ordenes
                {
                    idEquipo = idEquipo,
                    falla = falla.ToLower(),
                    observ = observ.ToLower(),
                    fecRecib = DateTime.Today,
                    aviso = false,
                    idEstado = 1,
                    fecAviso = null,
                    fecEntrega = null,
                };

                Session.Remove("interfaz");
                Session.Remove("lstEquipos");
                Session.Remove("ClienteMod");
                Session.Remove("sirve");

                bdCargada.Ordenes.InsertOnSubmit(nuevaOrden);
                bdCargada.SubmitChanges();
                nuevaLista(nuevaOrden);

                TempData["mensaje"] = "Se ha logrado registrar una nueva Orden";
                return RedirectToAction("ListaOrdenes");
            }
            else
            {
                TempData["mensaje"] = "La contraseña ingresada debe coincidir con la del usuario logueado";
                return RedirectToAction("RegistrarOrden");
            }
        }

        [HttpGet]
        public ActionResult ModificarOrden(int ID)
        {
            Ordenes unico = bdCargada.Ordenes.Single(O => O.idOrden == ID);
            Session["OrdenMod"] = unico;
            return View(); 
        }
        
        [HttpPost]
        public ActionResult ModificarOrden(string fallaM, string observM, string confirm)
        {
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;
            Ordenes OrdenMod = Session["OrdenMod"] as Ordenes;
            Ordenes unico = bdCargada.Ordenes.Single(O => O.idOrden == OrdenMod.idOrden);            

            if (UsuarioLog.passLog.Equals(confirm))
            {
                if (!fallaM.ToLower().Equals(unico.falla.ToLower()))
                    unico.falla = fallaM.ToLower();
                if (!observM.ToLower().Equals(unico.observ.ToLower()))
                    unico.observ = observM.ToLower();

                bdCargada.SubmitChanges();
                recargarLista(unico);
                Session.Remove("OrdenMod");

                TempData["mensaje"] = "Se ha modificado los datos de la orden N° " + OrdenMod.idOrden;
                return RedirectToAction("ListaOrdenes");
            }
            else
            {
                TempData["mensaje"] = "La contraseña ingresada debe coincidir con la del usuario logueado";
                return RedirectToAction("ModificarOrden", new { ID = OrdenMod.idOrden});
            }
        }

        [HttpGet]
        public ActionResult RegistrarTarea()
        { return View(); }

        [HttpPost]
        public ActionResult RegistrarTarea(string detalle, int boton)
        {
            Ordenes OrdenMod = Session["OrdenMod"] as Ordenes;
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;
            Ordenes unico = bdCargada.Ordenes.Single(O => O.idOrden == OrdenMod.idOrden);

            if (boton > 1)
            {
                if (boton == 2)
                {
                    unico.idEstado = 2;
                    bdCargada.SubmitChanges();
                    Session["OrdenMod"] = unico;
                    TempData["mensaje"] = "Se ha finalizado la Orden y SE ENCONTRO UNA SOLUCION";
                }
                else
                {
                    unico.idEstado = 3;
                    bdCargada.SubmitChanges();
                    Session["OrdenMod"] = unico;
                    TempData["mensaje"] = "Se ha finalizado la Orden y NO SE ENCONTRO NINGUNA SOLUCION";
                }
            }

            Tareas nuevaTarea = new Tareas
            {
                detalles = detalle.ToLower(),
                idOrden = OrdenMod.idOrden,
                idUsu = UsuarioLog.idUsu,
                fecTarea = DateTime.Today,
            };

            bdCargada.Tareas.InsertOnSubmit(nuevaTarea);
            bdCargada.SubmitChanges();

            var listaT = bdCargada.Tareas.Where(T => T.idOrden == unico.idOrden).OrderByDescending(T => T.fecTarea).OrderByDescending(T => T.idTarea);
            int canTareas = listaT.ToList().Count;

            var listaTM = listaT.Select(D => new TareaModel
            {
                detalle = D.detalles,
                tecnico = D.Usuarios.nomUsu,
                fecDetalle = (DateTime)D.fecTarea,
            }).ToList();

            Session["canTareas"] = canTareas;
            Session["lsTareas"] = listaTM;

            if(TempData["mensaje"] == null)
                TempData["mensaje"] = "Se ha realizado una nueva Tarea";

            return RedirectToAction("ConsultarOrden", new { ID = OrdenMod.idOrden });            
        }

        [HttpGet]
        public ActionResult AvisarCliente(int ID)
        {
            Ordenes orden = bdCargada.Ordenes.Single(O => O.idOrden == ID);
            orden.aviso = true;
            orden.fecAviso = DateTime.Today;

            bdCargada.SubmitChanges();
            recargarLista(orden);

            TempData["mensaje"] = "Se aviso al cliente " + orden.Equipos.Clientes.apeClie + ", " + orden.Equipos.Clientes.nomClie + " de la Orden N° " + orden.idOrden;
            orden = null;
            
            return RedirectToAction("ListaOrdenes");
        }
        
        [HttpGet]
        public ActionResult EntregarEquipo(int ID)
        {
            Ordenes orden = bdCargada.Ordenes.Single(O => O.idOrden == ID);
            orden.fecEntrega = DateTime.Today;

            bdCargada.SubmitChanges();
            recargarLista(orden);            

            TempData["mensaje"] = "Se entrego el equipo al cliente " + orden.Equipos.Clientes.apeClie + ", " + orden.Equipos.Clientes.nomClie + " de la Orden N° " + orden.idOrden;
            orden = null;

            return RedirectToAction("ListaOrdenes"); 
        }

        void nuevaLista(Ordenes orden)
        {
            var nuevaLista = new List<Models.OrdenModel>();

            OrdenModel ordenM = new OrdenModel
            {
                ID = orden.idOrden,
                cliente = orden.Equipos.Clientes.apeClie + ", " + orden.Equipos.Clientes.nomClie,
                equipo = orden.Equipos.TipoMarca.Tipos.tipo + " " + orden.Equipos.TipoMarca.Marcas.marca,
                falla = orden.falla,
                observaciones = orden.observ,
                fecRecibido = (DateTime)orden.fecRecib,
                estado = orden.Estados.estado,
                aviso = (bool)orden.aviso,
                fecAviso = (DateTime?)orden.fecAviso,
                fecEntregado = (DateTime?)orden.fecEntrega,
            };

            nuevaLista.Add(ordenM);
            Session["lstOrdenes"] = nuevaLista;
            Session.Remove("OrdenMod");
        }

        void recargarLista(Ordenes orden)
        {
            var listaModel = Session["lstOrdenes"] as List<Models.OrdenModel>;
            var arrayModel = listaModel.ToArray();

            for (int i = 0; i < arrayModel.Length; i++)
            {
                OrdenModel ordenArray = arrayModel[i];

                if (ordenArray.ID == orden.idOrden)
                {
                    OrdenModel ordenM = new OrdenModel
                    {
                        ID = orden.idOrden,
                        cliente = orden.Equipos.Clientes.apeClie + ", " + orden.Equipos.Clientes.nomClie,
                        equipo = orden.Equipos.TipoMarca.Tipos.tipo + " " + orden.Equipos.TipoMarca.Marcas.marca,
                        falla = orden.falla,
                        observaciones = orden.observ,
                        fecRecibido = (DateTime)orden.fecRecib,
                        estado = orden.Estados.estado,
                        aviso = (bool)orden.aviso,
                        fecAviso = (DateTime?)orden.fecAviso,
                        fecEntregado = (DateTime?)orden.fecEntrega,
                    };

                    arrayModel[i] = ordenM;
                    Session["lstOrdenes"] = arrayModel.ToList();
                }
            }
        }        
    }
}
