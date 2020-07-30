namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Data.Entity.Validation;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Helper;

    [Table("Usuario")]
    public partial class Usuario
    {
        public Usuario()
        {
            Experiencia = new HashSet<Experiencia>();
            Habilidad = new HashSet<Habilidad>();
            Testimonio = new HashSet<Testimonio>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        [Column(TypeName = "text")]
        public string Direccion { get; set; }

        [StringLength(50)]
        public string Ciudad { get; set; }

        public int? Pais_id { get; set; }

        [StringLength(50)]
        public string Telefono { get; set; }

        [StringLength(100)]
        public string Facebook { get; set; }

        [StringLength(100)]
        public string Twitter { get; set; }

        [StringLength(100)]
        public string YouTube { get; set; }

        [StringLength(50)]
        public string Foto { get; set; }


        public virtual ICollection<Experiencia> Experiencia { get; set; }

        public virtual ICollection<Habilidad> Habilidad { get; set; }

        public virtual ICollection<Testimonio> Testimonio { get; set; }

        [NotMapped]
        public TablaDato Pais { get; set; }


        //VALIDAMOS LAS CREDENCIALES DEL USUARIO ANTES DE ACCEDER
        public ResponseModel Acceder(string Email, string Password)
        {
            var responseModel = new ResponseModel();

            try
            {
                using (var ctx = new AppContext()) //Habrimos la conexion
                {
                    Password = HashHelper.MD5(Password); //Hasheamos la contraseña

                    //Busco el usuario en la base de datos, y la con clave hasheada
                    var usuario = ctx.Usuario.Where(x => x.Email == Email)
                                             .Where(x => x.Password == Password)
                                             .SingleOrDefault();

                    if (usuario != null) //Si el usuario existe
                    {
                        //Creo la sesion
                        SessionHelper.AddUserToSession(usuario.id.ToString());
                        responseModel.SetResponse(true);
                    }
                    else //Si no existe
                    {
                        responseModel.SetResponse(false, "Correo o contraseña incorrecta");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return responseModel;
        }

        //OBTENEMOS EL ID DEL USUARIO ACTUAL, (Para mostrar la foto)
        public Usuario Obtener(int id, bool includes = false)
        {
            var usuario = new Usuario();

            try
            {
                using (var ctx = new AppContext())
                {
                    if (!includes)
                    {
                        usuario = ctx.Usuario.Where(x => x.id == id)
                                             .SingleOrDefault();
                    }
                    else
                    {
                        usuario = ctx.Usuario.Include("Experiencia")
                                             .Include("Habilidad")
                                             .Include("Testimonio")
                                             .Where(x => x.id == id)
                                             .SingleOrDefault();
                    }

                    // Trayendo un registro adicional de manera manual, sin usar Include
                    usuario.Pais = new TablaDato().Obtener("pais", usuario.Pais_id.ToString());
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return usuario;
        }

        //PARA GUARDAR LOS CAMBIOS DEL USUARIO
        public ResponseModel Guardar(HttpPostedFileBase Foto)
        {
            //Por deferto el RM, espera un error (Devuelve False)
            var responseModel = new ResponseModel();

            try
            {
                using (var ctx = new AppContext())
                {
                    //Para que no valide internamente y pueda guardar EF
                    ctx.Configuration.ValidateOnSaveEnabled = false;

                    var eUsuario = ctx.Entry(this); //Agregamos una entidad del mismo topo (this)
                    eUsuario.State = EntityState.Modified;

                    //Si se subio una foto
                    if (Foto != null)
                    {
                        //Nombre del archivo, es decir, lo renombramos para que no se repita nunca
                        string nombreArchivo = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(Foto.FileName);

                        //La ruta donde lo vamos guardar
                        Foto.SaveAs(HttpContext.Current.Server.MapPath("~/uploads/" + nombreArchivo));

                        //Establecemos en nuestro modelo el nombre del archivo
                        this.Foto = nombreArchivo;
                    }
                    else //Si no se subio foto
                    {
                        //Se mantiene lo anterior, se ignora este campo para que EF no lo sobreescriba con NULL
                        eUsuario.Property(x => x.Foto).IsModified = false;  
                    } 

                    if (this.Password == null) //Si no se ingreso un nuevo Pass
                    {
                        //Se mantiene lo anterior, se ignora este campo para que EF no lo sobreescriba con NULL
                        eUsuario.Property(x => x.Password).IsModified = false; 
                    }

                    ctx.SaveChanges();

                    responseModel.SetResponse(true); //Pasamos el RM a true
                }
            }
            catch (DbEntityValidationException e)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }

            return responseModel;
        }
    }
}
