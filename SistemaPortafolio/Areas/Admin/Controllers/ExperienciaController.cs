using Helper;
using Models;
using SistemaPortafolio.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaPortafolio.Areas.Admin.Controllers
{
    //Filtro creado en AdminFilters
    [Autenticado] 
    public class ExperienciaController : Controller
    {
        private Experiencia experiencia = new Experiencia();

        // GET: Admin/Experiencia
        public ActionResult Index(int tipo = 1) //Seteo por defecto "Trabajo"
        {
            ViewBag.tipo = tipo;
            ViewBag.Title = tipo == 1 ? "Trabajos realizados" : "Estudios previos";
            return View();
        }

        public JsonResult Listar(AnexGRID grid, int tipo)
        {
            var experienciAListar = experiencia.Listar(grid, tipo, SessionHelper.GetUser());

            return Json(experienciAListar);
        }

        public ActionResult Crud(byte tipo = 0, int id = 0)
        {
            //Si id == 0, es que es un nuevo registro, ya que el valor por defecto de un int es 0
            if (id == 0) 
            {
                //Si el tipo no es 1 o 2, redireccionamos
                if (tipo == 0) return Redirect("~/admin/experiencia"); 

                experiencia.Tipo = tipo;
                experiencia.Usuario_id = SessionHelper.GetUser(); //Obtenemos el usuario de la sesion
            }
            else //Si no es un nuevo registro
            {
                experiencia = experiencia.Obtener(id); //Obtenemos el usuario por su ID
            }

            return View(experiencia);
        }

        public JsonResult Guardar(Experiencia experiencia)
        {
            var responseModel = new ResponseModel();

            if (ModelState.IsValid)
            {
                responseModel = experiencia.Guardar();

                if (responseModel.response) //Si se pudo guardar bien
                {
                    //Redireccionamos
                    responseModel.href = Url.Content("~/admin/experiencia/?tipo=" + experiencia.Tipo);
                }
            }

            return Json(responseModel);
        }

        public JsonResult Eliminar(int id)
        {
            var usuarioAEliminar = experiencia.Eliminar(id);

            return Json(usuarioAEliminar);
        }
    }
}