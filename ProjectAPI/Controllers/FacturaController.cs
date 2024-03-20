using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Context;
using ProjectAPI.Models;

namespace ProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class FacturaController : Controller
    {
        private readonly AppDbContext _context;

        public FacturaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CrearFactura([FromBody] Factura factura)
        {
            await _context.Facturas.AddAsync(factura);
            await _context.SaveChangesAsync();

            return Ok(factura);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFacturas()
        {
            var facturas = await _context.Facturas.ToListAsync();
            return Ok(facturas);
        }

        [HttpPost("{facturaId}/asignar-requisicion/{requisicionId}")]
        public IActionResult AsignarFacturaARequisicion(int facturaId, int requisicionId)
        {
            var factura = _context.Facturas.Find(facturaId);
            var requisicion = _context.Requisiciones.Find(requisicionId);

            if (factura == null || requisicion == null)
            {
                return NotFound();
            }

            // Actualizar UltimoCambio a la fecha y hora actual
            requisicion.UltimoCambio = DateTime.Now;

            factura.RequisicionId = requisicionId;
            _context.SaveChanges();

            return Ok("Factura asignada a la requisición correctamente.");
        }


    }
}
