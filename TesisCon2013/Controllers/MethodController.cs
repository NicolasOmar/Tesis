using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TesisCon2013.Controllers
{
    public class MethodController : Controller
    {
        public string Modificador(string cadena)
        {
            //este nombre a convertir lo convertis en un array de caracteres y lo volcas a una variable
            var letras = cadena.ToArray();

            /*el ciclo for funciona asi. si es el primer caracter, se cambiara a mayusculas
             caso contrario, si el caracter en cuestion es un espacio en blanco, la letra siguiente a esa sera en mayuscula
             pero si tanto el caracter actual como el anterior son espacios, se hara a minuscula, ah... si el caracter es un
             numero, se evita todo lo mencionado anteriormente*/

            for (int i = 0; i < letras.Length; i++)
            {
                String A = "";

                if (!Char.IsNumber(letras[i]))
                {
                    if (i == 0)
                    {
                        A = Convert.ToString(letras[i]).ToUpper();
                        letras[i] = Convert.ToChar(A);
                    }
                    else
                        if (!Char.IsWhiteSpace(letras[i]) && !Char.IsWhiteSpace(letras[i - 1]))
                        {
                            A = Convert.ToString(letras[i]).ToLower();
                            letras[i] = Convert.ToChar(A);

                        }
                        else
                        {
                            A = Convert.ToString(letras[i + 1]).ToUpper();
                            letras[i + 1] = Convert.ToChar(A);
                        }
                }

            }

            cadena = new string(letras);
            return cadena;
        }

        public void LimpiarSesiones()
        {
            if (System.Web.HttpContext.Current.Session["lstUsuarios"] != null)
                System.Web.HttpContext.Current.Session.Remove("lstUsuarios");
            if (System.Web.HttpContext.Current.Session["lstBarrios"] != null)
                System.Web.HttpContext.Current.Session.Remove("lstBarrios");
            if (System.Web.HttpContext.Current.Session["lstClientes"] != null)
                System.Web.HttpContext.Current.Session.Remove("lstClientes");
            if (System.Web.HttpContext.Current.Session["lstEquipos"] != null)
                System.Web.HttpContext.Current.Session.Remove("lstEquipos");
            if (System.Web.HttpContext.Current.Session["lstOrdenes"] != null)
                System.Web.HttpContext.Current.Session.Remove("lstOrdenes");
            if (System.Web.HttpContext.Current.Session["lsTareas"] != null)
                System.Web.HttpContext.Current.Session.Remove("lsTareas");
            if (System.Web.HttpContext.Current.Session["lstRoles"] != null)
                System.Web.HttpContext.Current.Session.Remove("lstRoles");
            if (System.Web.HttpContext.Current.Session["lsTareas"] != null)
                System.Web.HttpContext.Current.Session.Remove("lsTareas");
            if (System.Web.HttpContext.Current.Session["interfaz"] != null)
                System.Web.HttpContext.Current.Session.Remove("interfaz");

            if (System.Web.HttpContext.Current.Session["UsuarioMod"] != null)
                System.Web.HttpContext.Current.Session.Remove("UsuarioMod");
            if (System.Web.HttpContext.Current.Session["BarrioMod"] != null)
                System.Web.HttpContext.Current.Session.Remove("BarrioMod");
            if (System.Web.HttpContext.Current.Session["EquipoMod"] != null)
                System.Web.HttpContext.Current.Session.Remove("EquipoMod");
            if (System.Web.HttpContext.Current.Session["OrdenMod"] != null)
                System.Web.HttpContext.Current.Session.Remove("OrdenMod");
            if (System.Web.HttpContext.Current.Session["TimaMod"] != null)
                System.Web.HttpContext.Current.Session.Remove("TimaMod");
        }
    }
}
