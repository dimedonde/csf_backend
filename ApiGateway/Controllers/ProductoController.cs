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
    public class ProductoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var productos = _context.Productos.ToList();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound(new { message = "Producto no encontrado" });
            return Ok(producto);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProductoDTO dto)
        {
            if (dto == null) return BadRequest(new { message = "Datos inválidos" });

            var p = new Producto
            {
                Nombre_producto = dto.Nombre_producto,
                NroLote = dto.NroLote,
                Costo = dto.Costo,
                PrecioVenta = dto.PrecioVenta,
                Fec_registro = DateTime.Now
            };

            _context.Productos.Add(p);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = p.Id_producto }, p);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ProductoDTO dto)
        {
            var prod = _context.Productos.Find(id);
            if (prod == null) return NotFound(new { message = "Producto no encontrado" });

            prod.Nombre_producto = dto.Nombre_producto;
            prod.NroLote = dto.NroLote;
            prod.Costo = dto.Costo;
            prod.PrecioVenta = dto.PrecioVenta;

            _context.SaveChanges();
            return Ok(prod);
        }
    }
}
