using Models;
using SistemaPortafolio.App_Start;
using SistemaPortafolio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail; //Necesario para correo
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

        //PARA EL ENVIO DEL CORREO
        public JsonResult EnviarCorreo(ContactoViewModel model)
        {
            var rm = new ResponseModel(); //Por defecto False

            if (ModelState.IsValid)
            {
                try
                {
                    //Obtenemos el usuario visualizado
                    var _usuario = usuario.Obtener(FrontOfficeStartUp.UsuarioVisualizando());

                    var mail = new MailMessage();
                    mail.From = new MailAddress(model.Correo, model.Nombre); //El que escribe el correo
                    mail.To.Add(_usuario.Email); //Enviamos el correo al usuario visualizado (el mail de la base)
                    mail.Subject = "Correo desde contacto";
                    mail.IsBodyHtml = true;
                    mail.Body = model.Mensaje;

                    var SmtpServer = new SmtpClient("smtp.live.com"); // or "smtp.gmail.com"
                    SmtpServer.Port = 587;
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SmtpServer.UseDefaultCredentials = false;

                    // Agrega tu correo y tu contraseña, hemos usado el servidor de Outlook.
                    SmtpServer.Credentials = new System.Net.NetworkCredential("cristian___777@hotmail.com", "Rebecris");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                catch (Exception e)
                {
                    rm.SetResponse(false, e.Message);
                    return Json(rm);
                    throw;
                }

                //Si se envio el correo
                rm.SetResponse(true);
                rm.function = "CerrarContacto();"; //Ejecuamos esta funcion para cerrar el modal
            }

            return Json(rm);
        }
    }
}


