using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisCon2013.Models;

namespace TesisCon2013.Controllers
{
    public class EquipoController : Controller
    {
        MethodController Metodo = new MethodController();
        bdServTecDataContext bdCargada = new bdServTecDataContext();

        public ActionResult Index()
        {
            Metodo.LimpiarSesiones();

            Session["lstTipos"] = bdCargada.Tipos.OrderBy(T => T.tipo).ToList();
            Session["lstMarcas"] = bdCargada.Marcas.OrderBy(M => M.marca).ToList();
            Session["lstTiMa"] = bdCargada.TipoMarca.OrderBy(TM => TM.Marcas.marca).ToList();
            Session["lstClientes"] = bdCargada.Clientes.OrderBy(C => C.apeClie).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult FiltrarEquipo(string txtNumero, int idTipoMarca, string txtModelo, string txtSerie, string txtCliente)
        {
            var dbFiltrada = bdCargada.Equipos.Select(E => E);
            int canTotal = dbFiltrada.ToList().Count;

            if (txtNumero.Length > 0)
                dbFiltrada = dbFiltrada.Where(E => E.idEquipo == Convert.ToInt32(txtNumero));

            if (idTipoMarca > 0)
                dbFiltrada = dbFiltrada.Where(E => E.idTipoMarca == idTipoMarca);

            if (txtModelo.Length > 0)
                dbFiltrada = dbFiltrada.Where(E => E.modelo.ToLower().Contains(txtModelo.ToLower()));

            if (txtSerie.Length > 0)
                dbFiltrada = dbFiltrada.Where(E => E.numSerie.ToLower().Contains(txtSerie.ToLower()));

            if (txtCliente.Length > 0)
                dbFiltrada = dbFiltrada.Where(E => E.Clientes.apeClie.ToLower().Contains(txtCliente.ToLower()) || E.Clientes.nomClie.ToLower().Contains(txtCliente.ToLower()));

            if (dbFiltrada.ToList().Count > 0 && dbFiltrada.ToList().Count < canTotal)
            {
                var listaEquiposM = dbFiltrada.Select(equipo => new EquipoModel
                    {
                        ID = equipo.idEquipo,
                        tipo = equipo.TipoMarca.Tipos.tipo,
                        marca = equipo.TipoMarca.Marcas.marca,
                        modelo = equipo.modelo,
                        numSerie = equipo.numSerie,
                        cliente = equipo.Clientes.apeClie + ", " + equipo.Clientes.nomClie,
                    }).ToList();

                Session["lstEquipos"] = listaEquiposM;

                return View("listaEquipos");
            }
            else
                if (dbFiltrada.ToList().Count == canTotal)
                {
                    TempData["mensaje"] = "No ha seleccionado ninguna opcion para realizar la busqueda de Equipos. Porfavor, seleccione al menos una";
                    return RedirectToAction("Index", "Equipo");
                }
                else
                    if(dbFiltrada.ToList().Count == 0)
                    {
                        TempData["mensaje"] = "No se ha podido encontrar ningun Equipo con los datos ingresados";
                        return RedirectToAction("Index", "Equipo");
                    }

            return RedirectToAction("Index", "Equipo");
        }

        [HttpGet]
        public ActionResult ListaEquipos()
        {
            Session.Remove("sirve");
            return View();
        }

        [HttpGet]
        public ActionResult ConsultarEquipo(int ID)
        {
            Equipos unico = bdCargada.Equipos.Single(E => E.idEquipo == ID);
            Session["EquipoMod"] = unico;
            Session.Remove("sirve");
            return View(); }

        [HttpGet]
        public ActionResult ModificarEquipo()
        {
            if (Session["sirve"] == null)
            { Session["sirve"] = 0; }

            if(Session["interfaz"] == null)
                Session["interfaz"] = "modEquipo";
            return View(); 
        }

        [HttpPost]
        public ActionResult ModificarEquipo(short idTipoMarcaM, string modeloM, string numSerieM, short idClienteM, string confirm)
        {
            if(Session["lstClientes"] == null)
                Session["lstClientes"] = bdCargada.Clientes.OrderBy(C => C.apeClie).ToList();

            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;
            Equipos EquipoMod = Session["EquipoMod"] as Equipos;
            Equipos unico = bdCargada.Equipos.Single(E => E.idEquipo == EquipoMod.idEquipo);

            if (UsuarioLog.passLog.Equals(confirm))
            {
                if (unico.idTipoMarca != idTipoMarcaM)
                    unico.idTipoMarca = idTipoMarcaM;
                if (unico.modelo != modeloM)
                    unico.modelo = modeloM;
                if (unico.numSerie != numSerieM)
                    unico.numSerie = numSerieM;
                if (unico.idClie != idClienteM)
                    unico.idClie = idClienteM;

                bdCargada.SubmitChanges();

                reacargarLista(unico);
                Session.Remove("interfaz");
                Session.Remove("sirve");
                TempData["mensaje"] = "Se ha modificado los datos del Equipo N° " + unico.idEquipo;
                return RedirectToAction("ListaEquipos", "Equipo");
            }
            else
            {
                TempData["mensaje"] = "Debe ingresar correctamente la contraseña del usuario logueado para modificar el equipo";
                return RedirectToAction("ListaEquipos", "Equipo");
            }
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
                return Json(new { result = false, url = Url.Action("ModificarEquipo", "Equipo") });
        }

        [HttpGet]
        public ActionResult SeleccionarCliente(int ID)
        {
            Clientes unico = bdCargada.Clientes.Single(C => C.idClie == ID);
            Session["ClienteMod"] = unico;

            if (Convert.ToString(Session["interfaz"]).Equals("modEquipo"))
            {
                Session["sirve"] = 2;
                return RedirectToAction("ModificarEquipo");
            }
            else
                Session["sirve"] = true;

            return RedirectToAction("RegistrarEquipo");
        }

        [HttpGet]
        public ActionResult RegistrarEquipo()
        {
            if (Session["interfaz"] == null)
                Session["interfaz"] = "regEquipo";

            Session["lstClientes"] = bdCargada.Clientes.OrderBy(C => C.apeClie).ToList();
            return View();
        }
        
        [HttpPost]
        public ActionResult RegistrarEquipo(short idTipoMarca, string modelo, string numSerie, short idCliente, string confirm)
        {
             Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;

             if (UsuarioLog.passLog.Equals(confirm))
             {
                 UsuarioLog = null;
                 Equipos nuevoEquipo = new Equipos()
                 {
                     idTipoMarca = idTipoMarca,
                     modelo = modelo,
                     numSerie = numSerie,
                     idClie = idCliente,
                 };

                 bdCargada.Equipos.InsertOnSubmit(nuevoEquipo);
                 bdCargada.SubmitChanges();

                 TempData["mensaje"] = "Se ha logrado registrar un nuevo Equipo";

                 if (Convert.ToString(Session["interfaz"]).Equals("regOrden"))
                 {
                     Clientes ClienteMod = Session["ClienteMod"] as Clientes;
                     Session["lstEquipos"] = bdCargada.Equipos.Where(E => E.idClie == ClienteMod.idClie).ToList();
                     Session["sirve"] = true;
                     return RedirectToAction("RegistrarOrden", "Orden");
                 }
                 else
                 {
                     nuevaLista(nuevoEquipo);
                     Session.Remove("interfaz");
                     Session.Remove("sirve");
                 }
             }
             else
                 TempData["mensaje"] = "Debe ingresar la contraseña del usuario logueado para poder registrar un nuevo equipo";
            
            return RedirectToAction("RegistrarEquipo");
        }

        void nuevaLista(Equipos equipo)
        {
            var nuevaLista = new List<Models.EquipoModel>();

            EquipoModel equipoM = new EquipoModel
            {
                ID = equipo.idEquipo,
                tipo = equipo.TipoMarca.Tipos.tipo,
                marca = equipo.TipoMarca.Marcas.marca,
                modelo = equipo.modelo,
                numSerie = equipo.numSerie,
                cliente = equipo.Clientes.apeClie + ", " + equipo.Clientes.nomClie,
            };

            nuevaLista.Add(equipoM);
            equipoM = null;
            Session["lstEquipos"] = nuevaLista;
            Session.Remove("EquipoMod");
        }

        void reacargarLista(Equipos equipo)
        {
            var listaModel = Session["lstEquipos"] as List<Models.EquipoModel>;
            var arrayModel = listaModel.ToArray();

            for (int i = 0; i < arrayModel.Length; i++)
            {
                EquipoModel equipoArray = arrayModel[i];

                if (equipoArray.ID == equipo.idEquipo)
                {
                    EquipoModel equipoM = new EquipoModel
                    {
                        ID = equipo.idEquipo,
                        tipo = equipo.TipoMarca.Tipos.tipo,
                        marca = equipo.TipoMarca.Marcas.marca,
                        modelo = equipo.modelo,
                        numSerie = equipo.numSerie,
                        cliente = equipo.Clientes.apeClie + ", " + equipo.Clientes.nomClie,
                    };

                    arrayModel[i] = equipoM;
                    equipoM = null;
                    Session["lstEquipos"] = arrayModel.ToList();
                }
            }
        }
    }
}
