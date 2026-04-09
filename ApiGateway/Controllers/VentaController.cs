using Microsoft.AspNetCore.Mvc;
using ApiGateway.Data;
using ApiGateway.DTOs;
using ApiGateway.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ApiGateway.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var ventas = _context.VentaCab.ToList();
            return Ok(ventas);
        }

        [HttpPost]
        public IActionResult Post([FromBody] VentaDTO dto)
        {
            if (dto == null || dto.Detalles == null || !dto.Detalles.Any())
                return BadRequest(new { message = "Detalles de venta vacíos" });

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var cab = new VentaCab { FecRegistro = DateTime.Now };
                _context.VentaCab.Add(cab);
                _context.SaveChanges();

                decimal subtotal = 0;

                foreach (var item in dto.Detalles)
                {
                    var entradas = _context.MovimientoDet
                        .Where(md => md.Id_Producto == item.IdProducto)
                        .Join(_context.MovimientoCab, md => md.Id_MovimientoCab, mc => mc.Id_MovimientoCab,
                              (md, mc) => new { md, mc })
                        .Where(x => x.mc.Id_TipoMovimiento == 1)
                        .Sum(x => (int?)x.md.Cantidad) ?? 0;

                    var salidas = _context.MovimientoDet
                        .Where(md => md.Id_Producto == item.IdProducto)
                        .Join(_context.MovimientoCab, md => md.Id_MovimientoCab, mc => mc.Id_MovimientoCab,
                              (md, mc) => new { md, mc })
                        .Where(x => x.mc.Id_TipoMovimiento == 2)
                        .Sum(x => (int?)x.md.Cantidad) ?? 0;

                    var stock = entradas - salidas;
                    if (item.Cantidad > stock)
                        return BadRequest(new { message = $"Stock insuficiente para el producto {item.IdProducto}. Disponible: {stock}" });

                    decimal sub = item.Cantidad * item.Precio;
                    decimal igv = sub * 0.18m;
                    decimal total = sub + igv;

                    subtotal += sub;

                    _context.VentaDet.Add(new VentaDet
                    {
                        Id_VentaCab = cab.Id_VentaCab,
                        Id_producto = item.IdProducto,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio,
                        Sub_Total = sub,
                        Igv = igv,
                        Total = total
                    });
                }

                cab.SubTotal = subtotal;
                cab.Igv = subtotal * 0.18m;
                cab.Total = cab.SubTotal + cab.Igv;

                var movCab = new MovimientoCab
                {
                    Fec_registro = DateTime.Now,
                    Id_TipoMovimiento = 2,
                    Id_DocumentoOrigen = cab.Id_VentaCab
                };

                _context.MovimientoCab.Add(movCab);
                _context.SaveChanges();

                foreach (var item in dto.Detalles)
                {
                    _context.MovimientoDet.Add(new MovimientoDet
                    {
                        Id_MovimientoCab = movCab.Id_MovimientoCab,
                        Id_Producto = item.IdProducto,
                        Cantidad = item.Cantidad
                    });
                }

                _context.SaveChanges();
                transaction.Commit();

                return Ok(cab);
            }
            catch
            {
                transaction.Rollback();
                return BadRequest(new { message = "Error al registrar venta" });
            }
        }
    }
}
