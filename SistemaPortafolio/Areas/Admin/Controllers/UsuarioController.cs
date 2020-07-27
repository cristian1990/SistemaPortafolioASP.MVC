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
    public class UsuarioController : Controller
    {
        private Usuario usuario = new Usuario();
        private TablaDato dato = new TablaDato();

        //PARA OBTENER INFO DEL USUARIO ACTUAL
        // GET: Admin/Usuario
        public ActionResult Index()
        {
            //Asigno la lista de paises a un ViewBag
            ViewBag.Paises = dato.Listar("pais");
            //Obtengo el usuario actual que esta en sesion
            var usuarioEnSesion = usuario.Obtener(SessionHelper.GetUser());
            //Retorno a la vista el usuario
            return View(usuarioEnSesion);
        }

        //PARA GUARDAR INFO DE UN NUEVO USUARIO
        public JsonResult Guardar(Usuario usuario, HttpPostedFileBase foto) //Debe coincidir con name="foto" en Index
        {
            var ResponseModel = new ResponseModel();

            // Retiramos la validación de esta propiedad
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                ResponseModel = usuario.Guardar(foto);
            }

            return Json(ResponseModel);
        }
    }
}