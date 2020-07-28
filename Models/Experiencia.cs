namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Experiencia")]
    public partial class Experiencia
    {
        public int id { get; set; }

        public int Usuario_id { get; set; }

        public byte Tipo { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(10)]
        public string Desde { get; set; }

        [Required]
        [StringLength(10)]
        public string Hasta { get; set; }

        [Column(TypeName = "text")]
        public string Descripcion { get; set; }

        public virtual Usuario Usuario { get; set; }


        //OBTENEMOS EL ID DEL USUARIO DE LA SESION
        public Experiencia Obtener(int id)
        {
            var experiencia = new Experiencia();

            try
            {
                using (var ctx = new AppContext())
                {
                    experiencia = ctx.Experiencia.Where(x => x.id == id)
                                                 .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return experiencia;
        }

        //PARA GUARDAR LOS DATOS 
        public ResponseModel Guardar()
        {
            var responseModel = new ResponseModel();

            try
            {
                using (var ctx = new AppContext())
                {
                    //Si id es mayor a 0, es que ya existe
                    if (this.id > 0)
                    {
                        ctx.Entry(this).State = EntityState.Modified; //Entonces modificamos
                    }
                    else //Si no existe
                    {
                        ctx.Entry(this).State = EntityState.Added; //Agregamos
                    } 

                    ctx.SaveChanges();
                    responseModel.SetResponse(true);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return responseModel;
        }

        //PARA ELIMINAR LOS DATOS
        public ResponseModel Eliminar(int id)
        {
            var responseModel = new ResponseModel();

            try
            {
                using (var ctx = new AppContext())
                {
                    this.id = id;
                    ctx.Entry(this).State = EntityState.Deleted;

                    ctx.SaveChanges();
                    responseModel.SetResponse(true);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return responseModel;
        }

        //PARA LISTAR CON AnexGrid
        public AnexGRIDResponde Listar(AnexGRID grid, int tipo, int Usuario_id)
        {
            try
            {
                using (var ctx = new AppContext())
                {
                    //Desactivamos, para eviitar errores
                    ctx.Configuration.LazyLoadingEnabled = false;
         
                    grid.Inicializar(); //Inicializamos la grilla

                    //Elegimos tabla a paginar (Experiencia)
                    var query = ctx.Experiencia.Where(x => x.Tipo == tipo) 
                                               .Where(x => x.Usuario_id == Usuario_id); //filtro la experiencia x usuario

                    //CAMPOS DE LA TABLA: id, Nombre, Titulo, Desde, Hasta

                    // Ordenamiento
                    if (grid.columna == "id")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.id)
                                                             : query.OrderBy(x => x.id);
                    }

                    if (grid.columna == "Nombre")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Nombre)
                                                             : query.OrderBy(x => x.Nombre);
                    }

                    if (grid.columna == "Titulo")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Titulo)
                                                             : query.OrderBy(x => x.Titulo);
                    }

                    if (grid.columna == "Desde")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Desde)
                                                             : query.OrderBy(x => x.Desde);
                    }

                    if (grid.columna == "Hasta")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Hasta)
                                                             : query.OrderBy(x => x.id);
                    }
                 
                    var experiencias = query.Skip(grid.pagina) //Trae registros a partir de... 
                                            .Take(grid.limite) //Toma registros a partir de... 
                                            .ToList();

                    var total = query.Count();

                    grid.SetData(experiencias, total);
                }
            }
            catch (Exception e)
            {

                throw;
            }

            return grid.responde();
        }
    }
}
