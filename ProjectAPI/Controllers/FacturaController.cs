using Microsoft.AspNetCore.Authorization;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                factura.IsActive = true;
                _context.Facturas.Add(factura);
                await _context.SaveChangesAsync();
                return Ok(factura);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllFacturas()
        {
            var facturas = await _context.Facturas.Where(f => f.IsActive == true).ToListAsync();
            return Ok(facturas);
        }

        /* [HttpPost("{facturaId}/asignar-requisicion/{requisicionId}")]
        public IActionResult AsignarFacturaARequisicion(int facturaId, int requisicionId)
        {
            var factura = _context.Facturas.Find(facturaId);
            var requisicion = _context.Requisiciones.Find(requisicionId);

            if (factura == null || requisicion == null)
            {
                return NotFound();
            }

            factura.RequisicionId = requisicionId;
            _context.Facturas.Update(factura);
            _context.SaveChanges();

            return Ok("Factura asignada a la requisición correctamente.");
        } */

        [HttpPost("{facturaId}/asignar-requisicion/{requisicionId}")]
        public IActionResult AsignarFacturaARequisicion(int facturaId, int requisicionId)
        {
            var factura = _context.Facturas.Find(facturaId);
            var requisicion = _context.Requisiciones.Find(requisicionId);

            if (factura == null || requisicion == null)
            {
                return NotFound();
            }

            factura.RequisicionId = requisicionId;
            _context.SaveChanges();

            return Ok("Factura asignada a la requisición correctamente.");
        }
        [HttpGet]
        [Route("{codigo}")]
        public async Task<IActionResult> GetFactura([FromRoute] int codigo)
        {
            var factura = await _context.Facturas.FirstOrDefaultAsync(x => x.codigo == codigo);
            if (factura == null)
            {
                return NotFound();
            }
            return Ok(factura);
        }

        [HttpPut]
        [Route("{codigo}")]
        public async Task<IActionResult> UpdateFactura([FromRoute] int codigo, Factura updateFacturaRequest)
        {
            var factura = await _context.Facturas.FindAsync(codigo);
            if (factura == null)
            {
                return NotFound();
            }
            //factura.codigoFactura = updateFacturaRequest.codigoFactura;
            factura.codigoFactura = updateFacturaRequest.codigoFactura;
            factura.NombreCliente = updateFacturaRequest.NombreCliente;
            factura.Fecha = updateFacturaRequest.Fecha;
            factura.Pedido = updateFacturaRequest.Pedido;
            factura.NumFactura = updateFacturaRequest.NumFactura;
            factura.CorreoCliente = updateFacturaRequest.CorreoCliente;
            factura.Descripcion = updateFacturaRequest.Descripcion;

            await _context.SaveChangesAsync();

            return Ok(factura);
        }

        [HttpDelete]
        [Route("{codigo}")]
        public async Task<IActionResult> DeleteFactura([FromRoute] int codigo)
        {
            var factura = await _context.Facturas.FindAsync(codigo);
            if (factura == null)
            {
                return NotFound();
            }
            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            return Ok(factura);
        }

        [HttpPut]
        [Route("deactive/{codigo}")]
        public async Task<IActionResult> DeactivateFactura([FromRoute] int codigo)
        {
            var factura = await _context.Facturas.FindAsync(codigo);
            if (factura == null)
            {
                return NotFound();
            }
            factura.IsActive = false; //Desactiva la factura

            await _context.SaveChangesAsync();
            return Ok(factura);
        }

        [Authorize]
        [HttpGetAttribute("search")]
        public async Task<ActionResult<List<Factura>>> SearchFacturas([FromQuery] string nombreCliente)
        {
            if (string.IsNullOrWhiteSpace(nombreCliente))
            {
                return BadRequest(new { Message = "El nombre de cliente no puede estar vacío" });
            }

            var facturas = await _context.Facturas
                .Where(f => f.NombreCliente.Contains(nombreCliente))
                .ToListAsync();
            if (facturas.Count == 0)
            {
                return NotFound(new { Message = "No se encontraron facturas con el nombre proporcionado" });
            }
            return Ok(facturas);
        }


    }
}
