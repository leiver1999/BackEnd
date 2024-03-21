using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectAPI.Models
{
    public class Requisicion
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; } // Nuevo atributo para la fecha de creación
        public DateTime UltimoCambio { get; set; } // Nuevo atributo para el último cambio

        // Lista de facturas asignadas a esta requisición
        public List<Factura> Facturas { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public Requisicion()
        {
            FechaCreacion = DateTime.Now; // Se inicializa automáticamente al momento de la creación
            UltimoCambio = FechaCreacion; // Se inicia con la misma fecha de creación
        }
    }
}
