using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Context;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequisicionController : Controller
    {
        private readonly AppDbContext _context;
        public RequisicionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CrearRequisicion([FromBody] Requisicion requisicion)
        {
            if (requisicion == null)
            {
                return BadRequest("Los datos de la requisición son nulos.");
            }

            _context.Requisiciones.Add(requisicion);
            _context.SaveChanges();

            return Ok(requisicion);
        }

        [HttpGet("facturas/{requisicionId}")]
        public IActionResult ObtenerFacturasDeRequisicion(int requisicionId)
        {
            var requisicion = _context.Requisiciones.Find(requisicionId);

            if (requisicion == null)
            {
                return NotFound("Requisición no encontrada.");
            }

            // Cargar las facturas relacionadas con esta requisición
            var facturas = _context.Facturas.Where(f => f.RequisicionId == requisicionId).ToList();

            return Ok(facturas);
        }

        [HttpGet]
        public IActionResult ObtenerRequisiciones()
        {
            var requisiciones = _context.Requisiciones.ToList();

            // Iterar sobre cada requisición y establecer la lista de facturas como nula
            foreach (var requisicion in requisiciones)
            {
                requisicion.Facturas = null;
            }

            return Ok(requisiciones);
        }
    }
}
