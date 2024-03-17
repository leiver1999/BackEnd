using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }
        public string Cedula { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("VehiculoId")]
        public int? VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime ResetPasswordExpiry { get; set; }
    }
}
