using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DREA.Dominio;
using DREA.Modelo;

namespace DREA.Controllers
{
    public class DocumentoController : Controller
    {
        public ActionResult Index()
        {
            return View(DocumentoBL.Listar());
        }
        public ActionResult Crear()
        {
            ViewBag.TipoDocId = new SelectList(TipoDocBL.Listar(), "TipoDocId", "Nombre");
            ViewBag.OficinaId = new SelectList(OficinaBL.Listar(), "OficinaId", "Nombre");

            return View();
        }

        [HttpPost]
        public ActionResult Guardar(Documento doc)
        {
            doc.Fecha = DateTime.Now;
            doc.Estado = "P";
            doc.UsuarioId = 1;
            DocumentoBL.Crear(doc);

            return RedirectToAction("Index");
        }

        
    }
}