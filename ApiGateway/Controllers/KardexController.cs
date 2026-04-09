using Microsoft.AspNetCore.Mvc;
using ApiGateway.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ApiGateway.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class KardexController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KardexController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = _context.Productos.Select(p => new
            {
                p.Id_producto,
                p.Nombre_producto,
                StockActual = _context.MovimientoDet
                                .Where(md => md.Id_Producto == p.Id_producto)
                                .Join(_context.MovimientoCab,
                                      md => md.Id_MovimientoCab,
                                      mc => mc.Id_MovimientoCab,
                                      (md, mc) => new { md, mc })
                                .Sum(x => x.mc.Id_TipoMovimiento == 1 ? x.md.Cantidad : -x.md.Cantidad),
                p.Costo,
                p.PrecioVenta
            }).ToList();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetalle(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound(new { message = "Producto no encontrado" });

            var data = (
                from md in _context.MovimientoDet
                join mc in _context.MovimientoCab on md.Id_MovimientoCab equals mc.Id_MovimientoCab
                where md.Id_Producto == id
                select new
                {
                    mc.Fec_registro,
                    Tipo = mc.Id_TipoMovimiento == 1 ? "Entrada" : "Salida",
                    md.Cantidad
                }
            ).ToList();

            return Ok(data);
        }
    }
}
