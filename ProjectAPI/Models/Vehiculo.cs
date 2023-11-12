using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Models
{
    public class Vehiculo
    {
        [Key]
        public int Id {  get; set; }
        public string NumeroPlaca { get; set; }

        public long Kilometraje { get; set; }

        public string TipoVehiculo { get; set; }

        public string EstadoVehiculo { get; set; }
        public bool IsActive { get; set; }

    }
}
