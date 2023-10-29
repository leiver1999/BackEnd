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
            var vehiculos = await _backendDbContext.Vehiculos.ToListAsync();

            return Ok(vehiculos);
        }

        [HttpPost]
        public async Task<IActionResult> AddVehiculo([FromBody] Vehiculo vehiculoRequest)
        {
            vehiculoRequest.IdVehiculo = Guid.NewGuid();

            await _backendDbContext.Vehiculos.AddAsync(vehiculoRequest);
            await _backendDbContext.SaveChangesAsync();

            return Ok(vehiculoRequest);
        }

        [HttpGet]

        [Route("{idVehiculo:Guid}")]

        public async Task<IActionResult> GetVehiculo([FromRoute] Guid idVehiculo)
        {
            var vehiculo = await _backendDbContext.Vehiculos.FirstOrDefaultAsync(x => x.IdVehiculo == idVehiculo);
            if (vehiculo == null)
            {
                return NotFound();
            }
            return Ok(vehiculo);
        }

        [HttpPut]
        [Route("{idVehiculo:Guid}")]

        public async Task<IActionResult> UpdateVehiculo([FromRoute] Guid idVehiculo, Vehiculo updateVehiculoRequest)
        {
            var vehiculo = await _backendDbContext.Vehiculos.FindAsync(idVehiculo);
            if (vehiculo == null)
            {
                return NotFound();
            }

            vehiculo.NumeroPlaca = updateVehiculoRequest.NumeroPlaca;
            vehiculo.Kilometraje = updateVehiculoRequest.Kilometraje;
            vehiculo.TipoVehiculo = updateVehiculoRequest.TipoVehiculo;
            vehiculo.EstadoVehiculo = updateVehiculoRequest.EstadoVehiculo;

            await _backendDbContext.SaveChangesAsync();

            return Ok(vehiculo);
        }

        [HttpDelete]
        [Route("{idVehiculo:Guid}")]

        public async Task<IActionResult> DeleteVehiculo([FromRoute] Guid idVehiculo)
        {
            var vehiculo = await _backendDbContext.Vehiculos.FindAsync(idVehiculo);
            if (vehiculo == null)
            {
                return NotFound();
            }

            _backendDbContext.Vehiculos.Remove(vehiculo);
            await _backendDbContext.SaveChangesAsync();

            return Ok(vehiculo);
        }
    }
}
