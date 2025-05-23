using Microsoft.EntityFrameworkCore;
using Backend_PotenciaPC.Dtos;
using Backend_PotenciaPC.Repositories.Models;
using System.Data.SqlClient;
using Backend_PotenciaPC.Utilities;

namespace Backend_PotenciaPC.Repositories
{
    public class UserRepository
    {
        private readonly TestContext _context;

        public UserRepository(TestContext context)
        {
            _context = context;
        }

        public void CrearUsuario(User user)
        {
            var parameters = new[]
            {
                new SqlParameter("@Nombre", user.Nombre),
                new SqlParameter("@Correo", user.Correo),
                new SqlParameter("@Contraseña", user.Contraseña),
                new SqlParameter("@RolId", user.RolId)
            };
            _context.Database.ExecuteSqlRaw("EXEC sp_CrearUsuario @Nombre, @Correo, @Contraseña, @RolId", parameters);
        }

        public async Task<int> CrearUsuario(RequestUserDto requestUserDto)
        {
            var usuario = new User
            {
                Nombre = requestUserDto.Name,
                Correo = requestUserDto.Username,
                Contraseña = requestUserDto.Password,
                RolId = 1, // Rol por defecto: usuario común
                FechaCreacion = DateTime.Now
            };

            // ⬇️ Aquí generas y asignas el token y el estado confirmado en falso
            var token = Guid.NewGuid().ToString();
            usuario.TokenConfirmacion = token;
            usuario.Confirmado = false;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            

            // ⬇️ Aquí puedes enviar el correo con el link de confirmación
            string urlConfirmacion = $"https://localhost:7158/confirmar.html?token={token}";

            string rutaHtml = Path.Combine(Directory.GetCurrentDirectory(), "Utilities", "Templates", "ConfirmacionCorreo.html");

            // ✅ Leer contenido y reemplazar los valores
            string plantilla = await File.ReadAllTextAsync(rutaHtml);
            string mensaje = plantilla
                .Replace("{{Nombre}}", usuario.Nombre)
                .Replace("{{UrlConfirmacion}}", urlConfirmacion);

            // ✅ Enviar el correo
            EmailManager correo = new EmailManager();
            correo.EnviarCorreo(usuario.Correo, "Confirma tu cuenta", mensaje, true);

            return 1;
        }
        public async Task<User?> ObtenerEntidadUsuarioPorEmail(string email)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == email);
        }

        public async Task<User?> ObtenerDtoUsuarioPorEmail(string email)
        {
            return await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Correo == email);
        }


        public async Task<ResponseUserDto> Login(InicioSesionDto dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u =>
                    u.Correo == dto.NombreUsuario &&
                    u.Contraseña == dto.Contrasena
                );

            if (usuario == null)
                return new ResponseUserDto(); // devuelve uno vacío y Confirmado = false por defecto

            return new ResponseUserDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = usuario.Rol.Nombre,
                Confirmado = usuario.Confirmado // 🔴 Esto es fundamental
            };
        }


        public async Task<List<ResponseUserDto>> ObtenerUsuarios()
        {
            var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();

            return usuarios.Select(u => new ResponseUserDto
            {
                Id = u.Id,
                Rol = u.Rol.Nombre,
                Nombre = u.Nombre,
                Correo = u.Correo,
            }).ToList();
        }
    }
}
