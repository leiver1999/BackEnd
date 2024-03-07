using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Context;
using ProjectAPI.Helper;
using ProjectAPI.Models;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using ProjectAPI.Models.Dto;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public UserController(AppDbContext appDbContext, IConfiguration configuration, IEmailService emailService)
        {

            _authContext = appDbContext;
            _configuration = configuration;
            _emailService = emailService;
        }

        //sirve para autenticar usuarios por su nombre de usuario y contraseña
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Username == userObj.Username);

            if (user == null)
                return NotFound(new { Message = "¡Usuario no encontrado!" });

            if (user.IsActive == false)
                return NotFound(new { Message = "¡Usuario no encontrado!" });
            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = "La contraseña es incorrecta" });
            }

            user.Token = CreateJwt(user);

            return Ok(new
            {
                Token = user.Token,
                Message = "Acceso exitoso"
            }) ;
        }

        //sirve para guardar nuevos usuarios
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            if (await CheckEmailExist(userObj.Email))
            {
                return BadRequest(new { Message = "¡El correo ya existe!" });
            }
            if (await CheckCedulaExist(userObj.Cedula))
            {
                return BadRequest(new { Message = "¡La cédula ya existe!" });
            }
            if (await CheckTelefonoExist(userObj.Telefono))
            {
                return BadRequest(new { Message = "¡El teléfono  ya existe!" });
            }
            if (await CheckUserNameExist(userObj.Username))
            {
                return BadRequest(new { Message = "¡El nombre de usuario ya existe!" });
            }

            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Token = "";
            userObj.IsActive = true;

            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Usuario registrado"
            });
        }

        private Task<bool> CheckUserNameExist(string username)
            => _authContext.Users.AnyAsync(x => x.Username == username);
        private Task<bool> CheckEmailExist(string email)
            => _authContext.Users.AnyAsync(x => x.Email == email);

        private Task<bool> CheckCedulaExist(string cedula)
            => _authContext.Users.AnyAsync(x => x.Cedula == cedula);

        private Task<bool> CheckTelefonoExist(int telefono)
            => _authContext.Users.AnyAsync(x => x.Telefono == telefono);

        private string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("muymuysecretosecretomuymuysecreto");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")

            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllActiveUsers()
        {
            var activeUsers = await _authContext.Users.Where(user => user.IsActive).ToListAsync();
            return Ok(activeUsers);
        }


        //retorna la informacion de un distribuidor basado en su id
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetActiveUserById(int id)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if (user == null)
            {
                return NotFound(new { Message = "Usuario no encontrado" });
            }

            return Ok(user);
        }

        //borra un usuario por su id
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound(new { Message = "Usuario no encontrado" });
            }

            _authContext.Users.Remove(user);
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Usuario eliminado exitosamente" });
        }

        // Actualiza los datos de un usuario por su ID
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound(new { Message = "Usuario no encontrado" });
            }

            // Actualiza los campos permitidos
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Username = updatedUser.Username;
            user.Role = updatedUser.Role;
            user.Telefono = updatedUser.Telefono;
            user.Cedula = updatedUser.Cedula;


            // Guarda los cambios en la base de datos
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Datos de usuario actualizados exitosamente" });
        }

        // Desactiva un usuario por su ID
        [Authorize]
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {

            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound(new { Message = "Usuario no encontrado" });
            }

            // Desactiva el usuario
            user.IsActive = false;

            // Guarda los cambios en la base de datos
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Usuario desactivado exitosamente" });
        }

        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<User>>> SearchUsers([FromQuery] string searchTerm)
        {
            // Verifica si el término de búsqueda no está vacío
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest(new { Message = "El término de búsqueda no puede estar vacío" });
            }

            // Realiza la búsqueda en la base de datos
            var matchingUsers = await _authContext.Users
                .Where(user =>
                    user.IsActive &&
                    (user.FirstName.Contains(searchTerm) ||
                     user.LastName.Contains(searchTerm) ||
                     user.Cedula.Contains(searchTerm) ||
                     user.Username.Contains(searchTerm)))
                .ToListAsync();

            if (matchingUsers.Count == 0)
            {
                return NotFound(new { Message = "No se encontraron usuarios que coincidan con el término de búsqueda" });
            }

            return Ok(matchingUsers);
        }

        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(a=>a.Email == email);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "El email no existe en la base de datos"
                });
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            string from = _configuration["EmailSettings:EmailUsername"];
            var emailModel = new EmailDto(email, "Resetear Contraseña", RecoverPassEmailBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Se envió el correo"
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var user = await _authContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "El usuario no existe en la base de datos"
                });
            }
            var tokenCode = user.ResetPasswordToken;
            DateTime emailTokenExpiry = user.ResetPasswordExpiry;
            if(tokenCode != resetPasswordDto.EmailToken  || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "El link para cambiar contraseña es invalido"
                });
            }
            user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            _authContext.Entry(user).State |= EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Se cambio la contraseña de manera correcta."
            });

        }

        //[Authorize]
        [HttpPost("add-vehicle/{userId}")]
        public async Task<IActionResult> AddVehicleToUser(int userId, [FromBody] Vehiculo vehiculoRequest)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound(new { Message = "Usuario no encontrado" });
            }

            vehiculoRequest.IsActive = true;
            user.Vehiculo = vehiculoRequest;

            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Vehículo agregado al usuario exitosamente" });
        }
    }
}
