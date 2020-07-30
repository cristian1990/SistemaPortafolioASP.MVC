using Models;
using SistemaPortafolio.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaPortafolio.Controllers
{
    public class HomeController : Controller
    {
        private Usuario usuario = new Usuario();

        // GET: Home
        public ActionResult Index()
        {
            return View(usuario.Obtener(FrontOfficeStartUp.UsuarioVisualizando(), true));
        }
    }
}