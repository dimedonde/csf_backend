using Microsoft.AspNetCore.Mvc;
using ApiGateway.Data;
using ApiGateway.DTOs;
using ApiGateway.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiGateway.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CompraController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CompraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var compras = _context.CompraCab.ToList();
            return Ok(compras);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CompraDTO dto)
        {
            if (dto == null || dto.Detalles == null || !dto.Detalles.Any())
                return BadRequest(new { message = "Detalles de compra vacíos" });

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var cab = new CompraCab { FecRegistro = DateTime.Now };
                _context.CompraCab.Add(cab);
                _context.SaveChanges();

                decimal subtotal = 0;

                foreach (var item in dto.Detalles)
                {
                    var sub = item.Cantidad * item.Precio;
                    var igv = sub * 0.18m;
                    var total = sub + igv;

                    subtotal += sub;

                    _context.CompraDet.Add(new CompraDet
                    {
                        Id_CompraCab = cab.Id_CompraCab,
                        Id_producto = item.IdProducto,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio,
                        Sub_Total = sub,
                        Igv = igv,
                        Total = total
                    });

                    var prod = _context.Productos.Find(item.IdProducto);
                    if (prod != null)
                    {
                        prod.Costo = item.Precio;
                        prod.PrecioVenta = item.Precio * 1.35m;
                    }
                }

                cab.SubTotal = subtotal;
                cab.Igv = subtotal * 0.18m;
                cab.Total = cab.SubTotal + cab.Igv;

                _context.SaveChanges();
                transaction.Commit();

                return Ok(cab);
            }
            catch
            {
                transaction.Rollback();
                return BadRequest(new { message = "Error en compra" });
            }
        }
    }
}
