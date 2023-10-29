using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Models
{
    public class Vehiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid IdVehiculo { get; set; }

        public string NumeroPlaca { get; set; }

        public long Kilometraje { get; set; }

        public string TipoVehiculo { get; set; }

        public string EstadoVehiculo { get; set; }
    }
}
