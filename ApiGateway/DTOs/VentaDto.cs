namespace ApiGateway.DTOs
{
    public class VentaDetalleDTO
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }

    public class VentaDTO
    {
        public List<VentaDetalleDTO> Detalles { get; set; } = new List<VentaDetalleDTO>();
    }
}
