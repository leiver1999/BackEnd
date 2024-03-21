using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Context;
using ProjectAPI.Models;

namespace ProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculosController : Controller
    {
        private readonly AppDbContext _backendDbContext;
        public VehiculosController(AppDbContext backendDbContext)
        {
            _backendDbContext = backendDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehiculos()
        {
            var vehiculos = await _backendDbContext.Vehiculos.Where(v => v.IsActive == true).ToListAsync();

            return Ok(vehiculos);
        }

        [HttpPost]
        public async Task<IActionResult> AddVehiculo([FromBody] Vehiculo vehiculoRequest)
        {
            if (vehiculoRequest == null)
                return BadRequest();

            vehiculoRequest.IsActive = true;
            await _backendDbContext.Vehiculos.AddAsync(vehiculoRequest);
            await _backendDbContext.SaveChangesAsync();

            return Ok(vehiculoRequest);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetVehiculo([FromRoute] int id)
        {
            var vehiculo = await _backendDbContext.Vehiculos.FirstOrDefaultAsync(x => x.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }
            return Ok(vehiculo);
        }

        [HttpPut]
        [Route("{id}")]

        public async Task<IActionResult> UpdateVehiculo([FromRoute] int id, Vehiculo updateVehiculoRequest)
        {
            var vehiculo = await _backendDbContext.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            vehiculo.NumeroPlaca = updateVehiculoRequest.NumeroPlaca;
            vehiculo.Kilometraje = updateVehiculoRequest.Kilometraje;
            vehiculo.TipoVehiculo = updateVehiculoRequest.TipoVehiculo;
            vehiculo.EstadoVehiculo = updateVehiculoRequest.EstadoVehiculo;
            vehiculo.EstadoRtv = updateVehiculoRequest.EstadoRtv;

            await _backendDbContext.SaveChangesAsync();

            return Ok(vehiculo);
        }

        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> DeleteVehiculo([FromRoute] int id)
        {
            var vehiculo = await _backendDbContext.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            _backendDbContext.Vehiculos.Remove(vehiculo);
            await _backendDbContext.SaveChangesAsync();

            return Ok(vehiculo);
        }

        [HttpPut]
        [Route("deactivate/{id}")]

        public async Task<IActionResult> DeactivateVehiculo([FromRoute] int id)
        {
            var vehiculo = await _backendDbContext.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            vehiculo.IsActive = false; // Desactiva el vehículo

            await _backendDbContext.SaveChangesAsync();

            return Ok(vehiculo);
        }

        // Agrega este método al controlador VehiculosController
        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<List<Vehiculo>>> SearchVehiculos([FromQuery] string numeroPlaca, [FromQuery] string estadoRtv)
        {
            if (string.IsNullOrWhiteSpace(numeroPlaca))
            {
                return BadRequest(new { Message = "El número de placa no puede estar vacío" });
            }

            IQueryable<Vehiculo> query = _backendDbContext.Vehiculos
                                            .Where(v => v.IsActive && v.NumeroPlaca.Contains(numeroPlaca));

            if (!string.IsNullOrWhiteSpace(estadoRtv))
            {
                // Filtrar por EstadoRtv (ignorando mayúsculas y minúsculas)
                query = query.Where(v => v.EstadoRtv.Equals(estadoRtv, StringComparison.OrdinalIgnoreCase));
            }

            var vehiculos = await query.ToListAsync();

            if (vehiculos.Count == 0)
            {
                return NotFound(new { Message = "No se encontraron vehículos con el número de placa y estado RTV proporcionados" });
            }

            return Ok(vehiculos);
        }




    }
}
