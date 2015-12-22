using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisCon2013.Models;

namespace TesisCon2013.Controllers
{
    public class ClienteController : Controller
    {
        MethodController Metodo = new MethodController();
        bdServTecDataContext bdCargada = new bdServTecDataContext();

        public ActionResult Index()
        {
            Metodo.LimpiarSesiones();
            Session.Remove("interfaz");

            Session["lstBarrios"] = bdCargada.Barrios.OrderBy(B => B.barrio).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult FiltrarCliente(string txtNumero, string txtNom, string txtApe, string txtDire, int barrioFiltro, string txtTel)
        {
            var listaClientes = bdCargada.Clientes.ToList();
            var dbFiltrada = listaClientes.Select(C => C);
            int canTotal = listaClientes.Count;

            if (txtNumero.Length > 0)
                dbFiltrada = dbFiltrada.Where(C => C.idClie == Convert.ToInt32(txtNumero));

            if (txtNom.Length > 0)
                dbFiltrada = dbFiltrada.Where(C => C.nomClie.ToLower().Contains(txtNom.ToLower())); 

            if (txtApe.Length > 0)
                dbFiltrada = dbFiltrada.Where(C => C.apeClie.ToLower().Contains(txtApe.ToLower()));

            if (txtDire.Length > 0)
                dbFiltrada = dbFiltrada.Where(C => C.direClie.ToLower().Contains(txtDire.ToLower()));

            if (barrioFiltro > 0)
                dbFiltrada = dbFiltrada.Where(C => C.idBarrio == barrioFiltro);

            if (txtTel.Length > 0)
                dbFiltrada = dbFiltrada.Where(C => C.telClie.Contains(txtTel));

            dbFiltrada = dbFiltrada.OrderByDescending(C => C.apeClie);

            if (dbFiltrada.ToList().Count > 0 && dbFiltrada.ToList().Count < canTotal)
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
                return View("ListaClientes");
            }
            else
                if (dbFiltrada.ToList().Count == canTotal)
                {
                    TempData["mensaje"] = "No ha seleccionado ninguna opcion para realizar la busqueda de Clientes. Porfavor, seleccione al menos una";
                    return RedirectToAction("Index", "Cliente");
                }
                else
                    if (dbFiltrada.ToList().Count == 0)
                    {
                        TempData["mensaje"] = "No se han podido encontrar ningun Cliente con los datos ingresados";
                        return RedirectToAction("Index", "Cliente");
                    }

            return View("ListaClientes");
        }

        [HttpGet]
        public ActionResult ListaClientes()
        {
            if (Session["interfaz"] != null && Convert.ToString(Session["interfaz"]).Equals("modCliente"))
                Session.Remove("interfaz");
            return View(); 
        }

        [HttpGet]
        public ActionResult RegistrarCliente()
        {
            if (Session["interfaz"] == null)
                Session["interfaz"] = "regCliente";       

            if(Session["lstBarrios"] == null)
                Session["lstBarrios"] = bdCargada.Barrios.OrderBy(B => B.barrio).ToList();
            return View(); 
        }

        [HttpPost]
        public ActionResult RegistrarCliente(Clientes nuevoCliente, string telClie, string confirm)
        {
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;

            if (UsuarioLog.passLog.Equals(confirm))
            {
                UsuarioLog = null;
                var arrayClientes = bdCargada.Clientes.ToArray();

                for (int i = 0; i < arrayClientes.Length; i++)
                {
                    Clientes clienteCargado = arrayClientes[i];
                    if (clienteCargado.nomClie.ToLower().Equals(nuevoCliente.nomClie.ToLower()) && clienteCargado.apeClie.ToLower().Equals(nuevoCliente.apeClie.ToLower()))
                    {
                        TempData["mensaje"] = "Ya hay un cliente con el nombre y apellido que ingreso, intente con unos diferentes";
                        return RedirectToAction("RegistrarCliente", "Cliente");
                    }
                }

                if (nuevoCliente.idBarrio == 1)
                {
                    TempData["mensaje"] = "No ha seleccionado un barrio para el Cliente que quiso registrar";
                    return RedirectToAction("RegistrarCliente", "Cliente");
                }

                nuevoCliente.nomClie = Metodo.Modificador(nuevoCliente.nomClie);
                nuevoCliente.apeClie = Metodo.Modificador(nuevoCliente.apeClie);
                nuevoCliente.direClie = Metodo.Modificador(nuevoCliente.direClie);
                nuevoCliente.telClie = telClie;

                bdCargada.Clientes.InsertOnSubmit(nuevoCliente);
                bdCargada.SubmitChanges();

                nuevoCliente = bdCargada.Clientes.Single(C => C.idClie == nuevoCliente.idClie);
                nuevaLista(nuevoCliente);

                TempData["mensaje"] = "Se ha registrado un nuevo Cliente";

                if (Convert.ToString(Session["interfaz"]).Equals("regOrden"))
                {
                    Session["ClienteMod"] = nuevoCliente;
                    return RedirectToAction("RegistrarEquipo", "Equipo");
                }
                else
                    if (Convert.ToString(Session["interfaz"]).Equals("regEquipo"))
                    {
                        Session["ClienteMod"] = nuevoCliente;
                        Session["sirve"] = true;
                        return RedirectToAction("RegistrarEquipo", "Equipo");
                    }
                    else
                        Session.Remove("interfaz");
            }
            else
                TempData["mensaje"] = "Debe ingresar la contraseña del usuario logueado para poder registrar un nuevo cliente";

            return RedirectToAction("RegistrarCliente");
        }

        [HttpGet]
        public ActionResult ConsultarCliente(int ID)
        {
            var unico = bdCargada.Clientes.Single(C => C.idClie == ID);
            Session["ClienteMod"] = unico;
            return View(); }
        
        [HttpGet]
        public ActionResult ModificarCliente()
        {
            Session["interfaz"] = "modCliente";
            Session["lstBarrios"] = bdCargada.Barrios.OrderBy(U => U.barrio).ToList();
            return View();
        }
        
        [HttpPost]
        public ActionResult ModificarCliente(string nombre, string apellido, string direccion, int? idBarrio, string telefono, string confirm)
        {            
            Clientes ClienteMod = Session["ClienteMod"] as Clientes;
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;

            Clientes unico = bdCargada.Clientes.Single(C => C.idClie == ClienteMod.idClie);

            if(UsuarioLog.passLog.Equals(confirm))
            {
                if (!unico.nomClie.Equals(nombre))
                    unico.nomClie = Metodo.Modificador(nombre);
                if (!unico.apeClie.Equals(apellido))
                    unico.apeClie = Metodo.Modificador(apellido);
                if (!unico.direClie.Equals(direccion))
                    unico.direClie = Metodo.Modificador(direccion);

                if(unico.idBarrio != idBarrio)
                    unico.idBarrio = (short)idBarrio;

                if (!unico.telClie.Equals(telefono))
                    unico.telClie = telefono;

                bdCargada.SubmitChanges();
                recargarLista(unico);
                Session.Remove("interfaz");

                TempData["mensaje"] = "Se ha modificado los datos del Cliente N° " + @ClienteMod.idClie;
                return RedirectToAction("ListaClientes", "Cliente");
            }
            else
                TempData["mensaje"] = "Debe ingresar la contraseña del usuario logueado para poder modificar los datos del cliente";
            return RedirectToAction("ModificarCliente");
        }

        void nuevaLista(Clientes cElegido)
        {
            var nuevaLista = new List<Models.ClienteModel>();

            ClienteModel clienteM = new ClienteModel
            {
                ID = cElegido.idClie,
                nombre = cElegido.nomClie,
                apellido = cElegido.apeClie,
                direccion = cElegido.direClie,
                barrio = cElegido.Barrios.barrio,
                telefono = cElegido.telClie,
            };

            nuevaLista.Add(clienteM);
            Session["lstClientes"] = nuevaLista;
            clienteM = null;
            cElegido = null;

            Session.Remove("ClienteMod");
        }

        void recargarLista(Clientes cElegido)
        {
            var listaModel = Session["lstClientes"] as List<Models.ClienteModel>;
            var arrayModel = listaModel.ToArray();

            for (int i = 0; i < arrayModel.Length; i++)
            {
                ClienteModel clienteArray = arrayModel[i];
                if (clienteArray.ID == cElegido.idClie)
                {
                    ClienteModel clienteM = new ClienteModel
                    {
                        ID = cElegido.idClie,
                        nombre = cElegido.nomClie,
                        apellido = cElegido.apeClie,
                        direccion = cElegido.direClie,
                        barrio = cElegido.Barrios.barrio,
                        telefono = cElegido.telClie,
                    };

                    arrayModel[i] = clienteM;
                    Session["lstClientes"] = arrayModel.ToList();
                }
            }
        }
    }
}
