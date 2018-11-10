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
            return View(DocumentoBL.Listar(includeProperties:"TipoDoc,Oficina"));
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

        public ActionResult Editar(int id)
        {
            var doc = DocumentoBL.Obtener(id);
            ViewBag.TipoDocId = new SelectList(TipoDocBL.Listar(), "TipoDocId", "Nombre");
            ViewBag.OficinaId = new SelectList(OficinaBL.Listar(), "OficinaId", "Nombre");

            return View(doc);
        }
        [HttpPost]
        public ActionResult Actualizar(Documento doc)
        {
            // DocumentoBL.Actualizar(doc);
            DocumentoBL.ActualizarParcial(new Documento
            {
                DocumentoId = doc.DocumentoId,
                Nombre = doc.Nombre,
                NroDoc = doc.NroDoc,
                TipoDocId = doc.TipoDocId,
                OficinaId = doc.OficinaId
            }, x => x.Nombre, x => x.TipoDocId, x => x.OficinaId, x => x.NroDoc);


            return RedirectToAction("Index");
        }
    }
}