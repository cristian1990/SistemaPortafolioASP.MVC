using Helper;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaPortafolio.Areas.Admin.Filters;

namespace SistemaPortafolio.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private Usuario usuario = new Usuario();

        // GET: Admin/Login
        //Filtro creado en AdminFilters
        [NoLogin]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Acceder(string Email, string Password)
        {
            var responseModel = usuario.Acceder(Email, Password);

            if (responseModel.response) //Si existe el usuario y pass
            {
                //Redireccionamos al controlador Usuario 
                responseModel.href = Url.Content("~/admin/usuario");
            }
            return Json(responseModel);
        }

        public ActionResult Logout()
        {
            //Destruimos la sesion actual y redireccionamos
            SessionHelper.DestroyUserSession();
            return Redirect("~/");
        }
    }
}