using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGateway.Models
{
    public class MovimientoDet
    {
        [Key]
        public int Id_MovimientoDet { get; set; }

        public int Id_MovimientoCab { get; set; }
        public int Id_Producto { get; set; }

        public int Cantidad { get; set; }

        [ForeignKey("Id_MovimientoCab")]
        public MovimientoCab? MovimientoCab { get; set; }

        [ForeignKey("Id_Producto")]
        public Producto? Producto { get; set; }
    }
}
