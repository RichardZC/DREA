﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DREA.Dominio;
using DREA.Modelo;
using DREA.Models;

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

        [HttpPost]
        public JsonResult Adjuntar(int DocumentoId, HttpPostedFileBase documento)
        {
            var respuesta = new ResponseModel
            {
                respuesta = true,
                error = ""
            };

            if (documento != null)
            {
                string adjunto = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(documento.FileName);
                documento.SaveAs(Server.MapPath("~/Documentos/" + adjunto));

                DocumentoDetBL.Crear(new DocumentoDet {DocumentoId= DocumentoId,Archivo= adjunto });

               
            }
            else
            {
                respuesta.respuesta = false;
                respuesta.error = "Debe adjuntar un documento";
            }

            return Json(respuesta);
        }

        public PartialViewResult Adjuntos(int DocumentoId) {

            var docs = DocumentoDetBL.Listar(x => x.DocumentoId == DocumentoId);
            return PartialView(docs);
        }

        public JsonResult EliminarDocumento(int DocumentoDetId)
        {
            var rpt = new ResponseModel()
            {
                respuesta = true,
                error = ""
            };
            var doc = DocumentoDetBL.Obtener(DocumentoDetId);

            if (System.IO.File.Exists(Server.MapPath("~/Documentos/" + doc.Archivo)))
                System.IO.File.Delete(Server.MapPath("~/Documentos/" + doc.Archivo));

            DocumentoDetBL.Eliminar(DocumentoDetId);

            return Json(rpt);
        }


    }
}