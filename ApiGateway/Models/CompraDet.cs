using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGateway.Models
{
    public class CompraDet
    {
        [Key]
        public int Id_CompraDet { get; set; }

        public int Id_CompraCab { get; set; }
        public int Id_producto { get; set; }

        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }

        [ForeignKey("Id_CompraCab")]
        public CompraCab? CompraCab { get; set; }

        [ForeignKey("Id_producto")]
        public Producto? Producto { get; set; }
    }
}
