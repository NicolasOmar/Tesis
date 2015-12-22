using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisCon2013.Models;

namespace TesisCon2013.Controllers
{
    public class UsuarioController : Controller
    {
        MethodController Metodo = new MethodController();
        bdServTecDataContext bdCargada = new bdServTecDataContext();

        public ActionResult Index()
        {
            Metodo.LimpiarSesiones();

            Session["lstBarrios"] = bdCargada.Barrios.OrderBy(B => B.barrio).ToList();
            Session["lstRoles"] = bdCargada.Roles.OrderBy(O => O.rol).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult IniciarSesion(string nombre, string pass)
        {
            var usuariosCargados = bdCargada.Usuarios.ToList();

            foreach (var U in usuariosCargados)
            {
                if (U.nomLog.Equals(nombre) && U.passLog.Equals(pass))
                {
                    if (U.habilitar == true)
                    {
                        Session["UsuarioLog"] = U;
                        return View("Bienvenida");
                    }

                    TempData["mensaje"] = "El usuario con quien quiso ingresar al sistema esta bloqueado. Porfavor, comuniquese con el Administrador a cargo.";
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["mensaje"] = "El usuario o la contraseña ingresados son incorrectos. Intente de nuevo";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CerrarSesion()
        {
            Session["UsuarioLog"] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Bienvenida()
        {
            Metodo.LimpiarSesiones();
            return View();
        }

        [HttpGet]
        public ActionResult BuscarUsuarios()
        { return View(); }

        [HttpGet]
        public ActionResult FiltrarUsuarios(string txtNom, string txtDire, int idBarrio, string txtTel, string txtUsu, int idRol, int idEstado)
        {
            var listaUsuarios = bdCargada.Usuarios.ToList();
            var dbFiltrada = listaUsuarios.Select(U => U);
            int canTotal = listaUsuarios.Count;

            if (txtNom.Length > 0)
                dbFiltrada = dbFiltrada.Where(U => U.nomUsu.ToLower().Contains(txtNom.ToLower()));

            if (txtDire.Length > 0)
                dbFiltrada = dbFiltrada.Where(U => U.direcUsu.ToLower().Contains(txtDire.ToLower()));

            if (idBarrio > 0)
                dbFiltrada = dbFiltrada.Where(U => U.idBarrio == idBarrio);

            if (txtTel.Length > 0)
                dbFiltrada = dbFiltrada.Where(U => U.telUsu.Contains(txtTel));

            if (txtUsu.Length > 0)
                dbFiltrada = dbFiltrada.Where(U => U.nomLog.ToLower().Contains(txtUsu.ToLower()));

            if (idRol > 0)
                dbFiltrada = dbFiltrada.Where(U => U.idRol == idRol);

            if (idEstado > 0)
            {
                if (idEstado == 1)
                    dbFiltrada = dbFiltrada.Where(U => U.habilitar == true);
                else
                    dbFiltrada = dbFiltrada.Where(U => U.habilitar == false);
            }

            if (dbFiltrada.ToList().Count > 0 && dbFiltrada.ToList().Count < canTotal)
            {
                recargarU(dbFiltrada.ToList());
                return RedirectToAction("ListaUsuarios");
            }
            else
                if (dbFiltrada.ToList().Count == canTotal)
                {
                    TempData["mensaje"] = "No ha seleccionado ninguna opcion para realizar la busqueda de Usuarios. Porfavor, seleccione al menos una";
                    return RedirectToAction("BuscarUsuarios");
                }
                else
                    if (dbFiltrada.ToList().Count == 0)
                    {
                        TempData["mensaje"] = "No se han podido encontrar ningun Usuario con los datos ingresados";
                        return RedirectToAction("BuscarUsuarios");
                    }

            return RedirectToAction("BuscarUsuarios");
        }

        [HttpGet]
        public ActionResult ListaUsuarios()
        {
            Session.Remove("UsuarioMod");
            Session.Remove("regUsuario");
            return View();
        }

        [HttpGet]
        public ActionResult RegistrarUsuario()
        {
            if (Session["interfaz"] == null)
                Session["interfaz"] = "regUsuario";

            return View();
        }

        [HttpPost]
        public ActionResult RegistrarUsuario(Usuarios nuevoUsuario, string apeUsu, string passLog2, string confirm)
        {
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;
            var arrayUsuarios = bdCargada.Usuarios.ToArray();

            if (UsuarioLog.passLog.Equals(confirm))
            {
                for (int i = 0; i < arrayUsuarios.Length; i++)
                {
                    Usuarios usuarioCargado = arrayUsuarios[i];
                    if (nuevoUsuario.nomLog.ToLower().Equals(usuarioCargado.nomLog.ToLower()))
                    {
                        TempData["mensaje"] = "El nombre del usuario que quiere ingresar ya existe. Ingrese uno distinto";
                        return RedirectToAction("RegistrarUsuario");
                    }
                }

                if (nuevoUsuario.passLog.Equals(passLog2))
                {
                    if (nuevoUsuario.passLog.Length < 10)
                    {
                        TempData["mensaje"] = "La contraseña del usuario a registrar debe tener 10 caracteres";
                        return RedirectToAction("RegistrarUsuario");
                    }

                    Usuarios nuevoUsuarioM = new Usuarios
                    {
                        nomUsu = Metodo.Modificador(nuevoUsuario.nomUsu) + " " + Metodo.Modificador(apeUsu),
                        telUsu = nuevoUsuario.telUsu,
                        direcUsu = Metodo.Modificador(nuevoUsuario.direcUsu),
                        idBarrio = nuevoUsuario.idBarrio,
                        nomLog = nuevoUsuario.nomLog,
                        passLog = nuevoUsuario.passLog,
                        idRol = nuevoUsuario.idRol,
                        habilitar = true,
                    };

                    bdCargada.Usuarios.InsertOnSubmit(nuevoUsuarioM);
                    bdCargada.SubmitChanges();
                    agregarU(nuevoUsuarioM);

                    Session.Remove("interfaz");

                    TempData["mensaje"] = "Se ha registrado un nuevo Usuario";
                    return RedirectToAction("ListaUsuarios");
                }
                else
                    TempData["mensaje"] = "La contraseña del usuario a registrar debe ser ingresada 2 veces";
                return RedirectToAction("RegistrarUsuario");
            }
            else
                TempData["mensaje"] = "Debe ingresar la contraseña del usuario logueado para poder registrar uno nuevo";
            return RedirectToAction("RegistrarUsuario");
        }

        [HttpGet]
        public ActionResult ConsultarUsuario(int ID)
        {
            Usuarios unico = bdCargada.Usuarios.Single(U => U.idUsu == ID);
            Session["UsuarioMod"] = unico;
            return View();
        }

        [HttpGet]
        public ActionResult ModificarUsuario()
        {
            if(Session["interfaz"] == null)
                Session["interfaz"] = "modUsuario";

            Session["lstBarrios"] = bdCargada.Barrios.OrderBy(U => U.barrio).ToList();
            return View(); 
        }
        
        [HttpPost]
        public ActionResult ModificarUsuario(string nombre, string direccion, short idBarrio, string telefono, string newPass, string newPass2, string confirm)
        {
            Usuarios UsuarioMod = Session["UsuarioMod"] as Usuarios;
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;

            Usuarios unico = bdCargada.Usuarios.Single(U => U.idUsu == UsuarioMod.idUsu);

            if (UsuarioLog.passLog.Equals(confirm))
            {
                if (!unico.nomUsu.Equals(nombre))
                    unico.nomUsu = nombre;
                if (!unico.direcUsu.Equals(direccion))
                    unico.direcUsu = direccion;

                if (unico.idBarrio != idBarrio)
                    unico.idBarrio = idBarrio;

                if (!unico.telUsu.Equals(telefono))
                    unico.telUsu = telefono;

                bdCargada.SubmitChanges();

                if (UsuarioMod.idUsu == UsuarioLog.idUsu)
                { Session["UsuarioLog"] = unico; }

                Session.Remove("interfaz");

                TempData["mensaje"] = "Se han modificado los datos del usuario N°" + UsuarioMod.idUsu;
                return RedirectToAction("ListaUsuarios", "Usuario");
            }
            else
                TempData["mensaje"] = "Debe ingresar la contraseña del usuario logueado para poder modificar los datos del usuario";
            return RedirectToAction("ModificarUsuario");
        }

        [HttpGet]
        public ActionResult CambiarClave(int ID)
        {
            Usuarios unico = bdCargada.Usuarios.Single(U => U.idUsu == ID);
            Session["UsuarioMod"] = unico;
            return View();
        }
        
        [HttpPost]
        public ActionResult CambiarClave(string newPass, string newPass2, string confirm)
        {
            Usuarios UsuarioMod = Session["UsuarioMod"] as Usuarios;
            Usuarios UsuarioLog = Session["UsuarioLog"] as Usuarios;

            Usuarios unico = bdCargada.Usuarios.Single(U => U.idUsu == UsuarioMod.idUsu);

            if (UsuarioLog.passLog.Equals(confirm))
            {
                if (newPass.Length < 10)
                {
                    TempData["mensaje"] = "La nueva contraseña debe tener 10 caracteres";
                    return RedirectToAction("CambiarClave");
                }
                else
                    if (!newPass.Equals(newPass2))
                    {
                        TempData["mensaje"] = "La nueva contraseña debe ser ingresada 2 veces";
                        return RedirectToAction("CambiarClave");
                    }
                    else
                        if (unico.passLog.Equals(newPass))
                        {
                            TempData["mensaje"] = "La nueva contraseña ingresada es igual a la que ya tiene el usuario";
                            return RedirectToAction("CambiarClave");
                        }
                        else
                            unico.passLog = newPass;

                bdCargada.SubmitChanges();

                if (UsuarioMod.idUsu == UsuarioLog.idUsu)
                { Session["UsuarioLog"] = unico; }

                actualizarU(unico);

                TempData["mensaje"] = "Se ha cambiado la contraseña del usuario N°" + unico.idUsu;

                if(UsuarioLog.idRol == 3)
                    return RedirectToAction("Index", "Usuario");
                else
                    return RedirectToAction("ListaUsuarios", "Usuario");
            }
            else
                TempData["mensaje"] = "Debe ingresar la contraseña del usuario logueado para poder cambiar la contraseña";

            return RedirectToAction("CambiarClave");
        }

        [HttpGet]
        public ActionResult HabilitarUsuario(int ID)
        {
            Usuarios unico = bdCargada.Usuarios.Single(U => U.idUsu == ID);

            unico.habilitar = true;
            bdCargada.SubmitChanges();

            actualizarU(unico);

            TempData["mensaje"] = "Se ha habilitado el acceso del usuario N°" + unico.idUsu + " al sistema";

            return RedirectToAction("ListaUsuarios");
        }

        [HttpGet]
        public ActionResult BloquearUsuario(int ID)
        {
            Usuarios unico = bdCargada.Usuarios.Single(U => U.idUsu == ID);

            unico.habilitar = false;
            bdCargada.SubmitChanges();

            actualizarU(unico);

            TempData["mensaje"] = "Se ha bloqueado el acceso del usuario N°" + unico.idUsu + " al sistema";

            return RedirectToAction("ListaUsuarios");
        }

        public void recargarU(List<Usuarios> lstBD)
        {
            if (lstBD != null)
            {
                var lstUsuariosM = lstBD.Select(U => new UsuarioModel
                {
                    ID = U.idUsu,
                    nombre = U.nomUsu,
                    direccion = U.direcUsu,
                    barrio = U.Barrios.barrio,
                    telefono = U.telUsu,
                    logueo = U.nomLog,
                    password = U.passLog,
                    rol = U.Roles.rol,
                    habilitar = (bool)U.habilitar,
                }).ToList();

                Session["lstUsuarios"] = lstUsuariosM;
            }
            else
            {
                var lstUsuarios = bdCargada.Usuarios.ToList();
                var lstUsuariosM = lstUsuarios.Select(U => new UsuarioModel
                {
                    ID = U.idUsu,
                    nombre = U.nomUsu,
                    direccion = U.direcUsu,
                    barrio = U.Barrios.barrio,
                    telefono = U.telUsu,
                    logueo = U.nomLog,
                    password = U.passLog,
                    rol = U.Roles.rol,
                    habilitar = (bool)U.habilitar,
                }).ToList();

                Session["lstUsuarios"] = lstUsuariosM;
            }
        }

        public void agregarU(Usuarios nuevo)
        {
            var lstUsuariosM = Session["lstUsuarios"] as List<UsuarioModel>;

            UsuarioModel usuarioM = new UsuarioModel
            {
                ID = nuevo.idUsu,
                nombre = nuevo.nomUsu,
                direccion = nuevo.direcUsu,
                barrio = nuevo.Barrios.barrio,
                telefono = nuevo.telUsu,
                logueo = nuevo.nomLog,
                password = nuevo.passLog,
                rol = nuevo.Roles.rol,
                habilitar = (bool)nuevo.habilitar,
            };

            lstUsuariosM.Add(usuarioM);
            Session["lstUsuarios"] = lstUsuariosM;
        }

        public void actualizarU(Usuarios nuevo)
        {
            var lstUsuariosM = Session["lstUsuarios"] as List<UsuarioModel>;

            if(lstUsuariosM == null)
                lstUsuariosM = new List<UsuarioModel>();

            var arrayModel = lstUsuariosM.ToArray();

            if (arrayModel.Length > 0)
            {
                for (int i = 0; i < arrayModel.Length; i++)
                {
                    UsuarioModel usuarioArray = arrayModel[i];

                    if (usuarioArray.ID == nuevo.idUsu)
                    {
                        UsuarioModel usuarioM = new UsuarioModel
                        {
                            ID = nuevo.idUsu,
                            nombre = nuevo.nomUsu,
                            direccion = nuevo.direcUsu,
                            barrio = nuevo.Barrios.barrio,
                            telefono = nuevo.telUsu,
                            logueo = nuevo.nomLog,
                            password = nuevo.passLog,
                            rol = nuevo.Roles.rol,
                            habilitar = (bool)nuevo.habilitar,
                        };

                        arrayModel[i] = usuarioM;
                        Session["lstUsuarios"] = arrayModel.ToList();
                    }
                }
            }
            else
            {
                UsuarioModel usuarioM = new UsuarioModel
                {
                    ID = nuevo.idUsu,
                    nombre = nuevo.nomUsu,
                    direccion = nuevo.direcUsu,
                    barrio = nuevo.Barrios.barrio,
                    telefono = nuevo.telUsu,
                    logueo = nuevo.nomLog,
                    password = nuevo.passLog,
                    rol = nuevo.Roles.rol,
                    habilitar = (bool)nuevo.habilitar,
                };

                lstUsuariosM.Add(usuarioM);
                Session["lstUsuarios"] = lstUsuariosM;
            }
        }
    }
}