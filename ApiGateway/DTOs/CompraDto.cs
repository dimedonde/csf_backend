namespace ApiGateway.DTOs
{
    public class CompraDetalleDTO
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }

    public class CompraDTO
    {
        public List<CompraDetalleDTO> Detalles { get; set; } = new List<CompraDetalleDTO>();
    }
}
