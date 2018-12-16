using DREA.Dominio;
using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DREA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(DocumentoBL.Listar(null, x => x.OrderByDescending(y => y.DocumentoId),
                    includeProperties: "TipoDoc,Oficina").Take(15).ToList());
            }

            return View(DocumentoBL.Listar(x => x.Nombre.Contains(id) || x.NroDoc.Contains(id),
                x => x.OrderByDescending(y => y.DocumentoId),
            includeProperties: "TipoDoc,Oficina"));
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Autenticar(string user, string pass)
        {
           
                var aut = UsuarioBL.Obtener(x => x.Nombre == user && x.Clave == pass);
                if (aut == null)
                    return RedirectToAction("Login", "Home", new { error = "Error en Usuario o Contraseña" });

                SessionHelper.AddUserToSession(aut.UsuarioId.ToString());
                return RedirectToAction("Index", "Home");
            
        }

        public ActionResult CerrarSesion()
        {
            SessionHelper.DestroyUserSession();
            return RedirectToAction("Login", "Home");
        }
    }
}