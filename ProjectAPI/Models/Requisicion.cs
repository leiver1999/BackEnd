using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Models
{
    public class Requisicion
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }

        // Lista de facturas asignadas a esta requisición
        public List<Factura> Facturas { get; set; }
    }
}
