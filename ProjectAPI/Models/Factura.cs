using System.ComponentModel.DataAnnotations;

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
<<<<<<< Updated upstream
=======
        //public string EmailVendedor { get; set; }
        [JsonIgnore]// Ignorar esta propiedad al serializar
        public int? RequisicionId { get; set; }
        [JsonIgnore]// Ignorar esta propiedad al serializar
        public Requisicion Requisicion { get; set; }
>>>>>>> Stashed changes

    }
}
