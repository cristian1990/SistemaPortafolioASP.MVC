using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helper;
using Models;
using SistemaPortafolio.Areas.Admin.Filters;

namespace SistemaPortafolio.Areas.Admin.Controllers
{
    //Filtro creado en AdminFilters
    [Autenticado]
    public class TestimoniosController : Controller
    {
     
        private Testimonio testimonio = new Testimonio();

        // GET: Admin/Testimonios
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar(AnexGRID grid)
        {
            return Json(testimonio.Listar(grid, SessionHelper.GetUser()));
        }

        public ActionResult Crud(int id)
        {
            return View(testimonio.Obtener(id));
        }

        public JsonResult Guardar(Testimonio testimonio)
        {
            var rm = new ResponseModel();

            if (ModelState.IsValid)
            {
                rm = testimonio.Guardar();

                if (rm.response)
                {
                    rm.href = Url.Content("~/admin/testimonios/");
                }
            }

            return Json(rm);
        }

        public JsonResult Eliminar(int id)
        {
            return Json(testimonio.Eliminar(id));
        }
    }
}