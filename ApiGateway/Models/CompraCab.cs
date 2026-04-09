using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Models
{
    public class CompraCab
    {
        [Key]
        public int Id_CompraCab { get; set; }

        public DateTime FecRegistro { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }

        public ICollection<CompraDet>? Detalles { get; set; }
    }
}
