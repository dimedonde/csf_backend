using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Models
{
    public class MovimientoCab
    {
        [Key]
        public int Id_MovimientoCab { get; set; }

        public DateTime Fec_registro { get; set; }

        // 1 = Compra, 2 = Venta
        public int Id_TipoMovimiento { get; set; }

        // FK al documento origen (CompraCab o VentaCab)
        public int Id_DocumentoOrigen { get; set; }

        public ICollection<MovimientoDet>? Detalles { get; set; }
    }
}
