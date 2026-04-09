using System;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Models
{
    public class Producto
    {
        [Key]
        public int Id_producto { get; set; }

        [Required]
        public string Nombre_producto { get; set; } = string.Empty;

        [Required]
        public string NroLote { get; set; } = string.Empty;

        public decimal Costo { get; set; }
        public decimal PrecioVenta { get; set; }

        public DateTime Fec_registro { get; set; }
    }
}
