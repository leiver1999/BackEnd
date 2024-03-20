using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectAPI.Models
{
    public class Factura
    {
        [Key]
        public int codigo { get; set; }
        public int codigoFactura { get; set; }
        public string NombreCliente { get; set; }
        public string Fecha { get; set; }
        public string Pedido { get; set; }
        public string NumFactura { get; set; }
        [JsonIgnore]
        public int? RequisicionId { get; set; }
        [JsonIgnore]// Ignorar esta propiedad al serializar
        public Requisicion Requisicion { get; set; }


    }
}
