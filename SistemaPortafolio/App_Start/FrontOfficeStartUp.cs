using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaPortafolio.App_Start
{
    //Esta clase se agrega para que cargue por defecto la informacion de un usuario, si no se especifico ninguno
    public class FrontOfficeStartUp
    {
        public static int UsuarioVisualizando()
        {
            int usuario_id_por_defecto = 6;

            //Si se especifica un usuario
            string usuario_id = HttpContext.Current.Request.QueryString["id"];

            //Retornamos la vista del usuario especificado, si no el por defecto
            return usuario_id != null ? Convert.ToInt32(usuario_id) : usuario_id_por_defecto;
        }
    }
}