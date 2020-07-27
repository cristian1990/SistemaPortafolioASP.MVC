using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaPortafolio.Areas.Admin.Filters;

namespace SistemaPortafolio.Areas.Admin.Controllers
{
    //Filtro creado en AdminFilters
    [Autenticado]
    public class TestimoniosController : Controller
    {
        // GET: Admin/Testimonios
        public ActionResult Index()
        {
            return View();
        }
    }
}