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
        // GET: Admin/Experiencia
        public ActionResult Index()
        {
            return View();
        }
    }
}