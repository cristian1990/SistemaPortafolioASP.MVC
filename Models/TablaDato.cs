namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("TablaDato")]
    public partial class TablaDato
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Relacion { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Valor { get; set; }

        [Required]
        [StringLength(50)]
        public string Descripcion { get; set; }

        public int Orden { get; set; }


        public List<TablaDato> Listar(string relacion)
        {
            var datos = new List<TablaDato>();

            try
            {
                using (var ctx = new AppContext())
                {
                    datos = ctx.TablaDato.OrderBy(x => x.Orden) //Ordenamos por campo Orden
                                         .Where(x => x.Relacion == relacion) //Buscamos la relacion (pais)
                                         .ToList(); //Pasamos a una lista
                }
            }
            catch (Exception)
            {

                throw;
            }

            return datos;
        }

        //Para traer el Pais
        public TablaDato Obtener(string relacion, string valor)
        {
            var dato = new TablaDato();

            try
            {
                using (var ctx = new AppContext())
                {
                    dato = ctx.TablaDato.Where(x => x.Relacion == relacion)
                                        .Where(x => x.Valor == valor)
                                        .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return dato;
        }
    }
}
