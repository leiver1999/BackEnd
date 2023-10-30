using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Context;
using ProjectAPI.Helper;
using ProjectAPI.Models;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        public UserController(AppDbContext appDbContext) {

            _authContext = appDbContext;
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

            //verificar usuario
            if(await CheckUserNameExist(userObj.Username))
            {
                return BadRequest(new { Message = "¡El nombre de usuario ya existe!" });
            }

            if (!IsValidEmail(userObj.Email))
            {
                return BadRequest(new { Message = "El formato del correo electrónico no es válido." });
            }

            //verificar email
            if (await CheckEmailExist(userObj.Email))
            {
                return BadRequest(new { Message = "¡El correo ya existe!" });
            }


            //comprobar la seguridad de la contraseña 
            var pass = CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest(new { Message = pass.ToString() });


            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            //userObj.Role = "Administrador";
            userObj.Token = "";

            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Usuario registrado"
            });
        }

        private Task<bool> CheckUserNameExist(string username)
            => _authContext.Users.AnyAsync(x => x.Username == username);


        private bool IsValidEmail(string email)
        {
            // Utiliza una expresión regular para validar el formato del correo electrónico.
            // Puedes ajustar la expresión regular según tus requisitos específicos.
            string emailPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            return Regex.IsMatch(email, emailPattern);
        }
        private Task<bool> CheckEmailExist(string email)
            => _authContext.Users.AnyAsync(x => x.Email == email);



        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < 8)
                sb.Append("La longitud mínima de la contraseña debe ser 8" + Environment.NewLine);
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]")
                && Regex.IsMatch(password, "[0-9]")))
                sb.Append("La contraseña debe ser alfanumérica" + Environment.NewLine);
            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("La contraseña debe contener un carácter especial." + Environment.NewLine);
            return sb.ToString();
        }

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
        public async Task<ActionResult<User>>GetAllUsers()
        {
            return Ok(await _authContext.Users.ToListAsync());
        }

        //retorna la informacion de un distribuidor basado en su id
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Id == id);

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

    }
}
