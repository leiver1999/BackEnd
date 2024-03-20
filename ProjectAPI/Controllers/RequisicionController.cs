using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost("{usuarioId?}")] // El parámetro usuarioId es opcional
        public IActionResult CrearRequisicion([FromBody] Requisicion requisicion, int? usuarioId)
        {
            if (requisicion == null)
            {
                return BadRequest("Los datos de la requisición son nulos.");
            }

            _context.Requisiciones.Add(requisicion);
            _context.SaveChanges();

            if (usuarioId.HasValue)
            {
                // Llama a la función para asignar la requisición al usuario
                var asignacionResult = AsignarRequisicionAUsuario(requisicion.Id, usuarioId.Value);
                if (asignacionResult is BadRequestObjectResult)
                {
                    // Si la asignación falla, puedes manejar el error aquí si es necesario
                    return BadRequest("La asignación de la requisición al usuario falló.");
                }
            }

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

        [HttpPut("{requisicionId}")]
        public IActionResult EditarRequisicion(int requisicionId, [FromBody] Requisicion requisicionEditada)
        {
            var requisicion = _context.Requisiciones.Find(requisicionId);
            if (requisicion == null)
            {
                return NotFound("Requisición no encontrada.");
            }

            // Actualizar los datos de la requisición
            requisicion.Descripcion = requisicionEditada.Descripcion;
            requisicion.Estado = requisicionEditada.Estado;

            // Actualizar UltimoCambio a la fecha y hora actual
            requisicion.UltimoCambio = DateTime.Now;

            _context.SaveChanges();

            return Ok("Requisición editada correctamente.");
        }

        //[Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Requisicion>>> SearchRequisiciones([FromQuery] string searchTerm)
        {
            // Verifica si el término de búsqueda no está vacío
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest(new { Message = "El término de búsqueda no puede estar vacío" });
            }

            // Realiza la búsqueda en la base de datos
            var matchingRequisiciones = await _context.Requisiciones
                .Where(requisicion =>
                    requisicion.Descripcion.Contains(searchTerm) ||
                    requisicion.Estado.Contains(searchTerm))
                .ToListAsync();

            if (matchingRequisiciones.Count == 0)
            {
                return NotFound(new { Message = "No se encontraron requisiciones que coincidan con el término de búsqueda" });
            }

            return Ok(matchingRequisiciones);
        }


        //trae todas las requisiciones
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

        //trae rquisiciones de estado Creada
        [HttpGet("creadas")]
        public IActionResult ObtenerRequisicionesCreadas()
        {
            var requisicionesCreadas = _context.Requisiciones.Where(r => r.Estado == "Creada").ToList();
            foreach (var requisicion in requisicionesCreadas)
            {
                requisicion.Facturas = null;
            }
            return Ok(requisicionesCreadas);
        }

        //trae rquisiciones de estado Asignada
        [HttpGet("asignadas")]
        public IActionResult ObtenerRequisicionesAsignadas()
        {
            var requisicionesAsignadas = _context.Requisiciones.Where(r => r.Estado == "Asignada").ToList();
            foreach (var requisicion in requisicionesAsignadas)
            {
                requisicion.Facturas = null;
            }
            return Ok(requisicionesAsignadas);
        }

        //trae rquisiciones de estado Completada
        [HttpGet("completadas")]
        public IActionResult ObtenerRequisicionesCompletadas()
        {
            var requisicionesCompletadas = _context.Requisiciones.Where(r => r.Estado == "Completada").ToList();
            foreach (var requisicion in requisicionesCompletadas)
            {
                requisicion.Facturas = null;
            }
            return Ok(requisicionesCompletadas);
        }

        //trae rquisiciones de estado Creada y Asignada
        [HttpGet("creadas-asignadas")]
        public IActionResult ObtenerRequisicionesCreadasAsignadas()
        {
            var requisicionesCreadasAsignadas = _context.Requisiciones.Where(r => r.Estado == "Creada" || r.Estado == "Asignada").ToList();
            foreach (var requisicion in requisicionesCreadasAsignadas)
            {
                requisicion.Facturas = null;
            }
            return Ok(requisicionesCreadasAsignadas);
        }


        [HttpPost("{requisicionId}/asignar-usuario/{usuarioId}")]
        public IActionResult AsignarRequisicionAUsuario(int requisicionId, int usuarioId)
        {
            var requisicion = _context.Requisiciones.Find(requisicionId);
            if (requisicion == null)
            {
                return NotFound("Requisición no encontrada.");
            }

            if (requisicion.Estado == "Completada")
            {
                return BadRequest("La requisición está completada y no se puede asignar.");
            }

            if (requisicion.Estado != "Creada" && requisicion.Estado != "Asignada")
            {
                return BadRequest("La requisición no se puede asignar porque no está en estado 'Creada' o 'Asignada'.");
            }

            var usuario = _context.Users.Find(usuarioId);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Verificar si el usuario tiene el rol de "Distribuidor"
            if (usuario.Role != "Distribuidor")
            {
                return BadRequest("El usuario no tiene permiso para ser asignado a esta requisición.");
            }

            // Si la requisición estaba en estado "Creada", cambiar a "Asignada"
            if (requisicion.Estado == "Creada")
            {
                requisicion.Estado = "Asignada";
            }

            // Actualizar UltimoCambio a la fecha y hora actual
            requisicion.UltimoCambio = DateTime.Now;

            // Asignar la requisición al usuario
            requisicion.UserId = usuarioId;

            _context.SaveChanges();

            return Ok("Requisición asignada al usuario correctamente.");
        }


    }
}
