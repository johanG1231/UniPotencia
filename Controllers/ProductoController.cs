using Backend_PotenciaPC.Repositories.Models;
using Backend_PotenciaPC.Services;
using Microsoft.AspNetCore.Mvc;
using Backend_PotenciaPC.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;

namespace Backend_PotenciaPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TestContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductoController(UserService userService, TestContext context, IWebHostEnvironment env)
        {
            _userService = userService;
            _context = context;
            _env = env;
        }

        [HttpPost("Subir")]
        public async Task<IActionResult> Subir([FromForm] ProductoDto dto, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest("Archivo no válido");

            var extension = Path.GetExtension(archivo.FileName).ToLower();
            if (extension != ".bat")
                return BadRequest("Solo se permiten archivos .bat");

            var carpeta = Path.Combine(_env.WebRootPath, "archivos");
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var ruta = Path.Combine(carpeta, nombreArchivo);

            using (var stream = new FileStream(ruta, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                Descripcion = dto.Descripcion,
                ArchivoUrl = $"/archivos/{nombreArchivo}",
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            try
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error guardando en base de datos: {ex.Message}");
            }
            Console.WriteLine("Archivo guardado en: " + ruta);
            Console.WriteLine("Archivo existe: " + System.IO.File.Exists(ruta));

            return Ok(new { mensaje = "Producto subido", producto });
        }


        [HttpPut("Editar/{id}")]
        public IActionResult Editar(int id, [FromBody] ProductoDto dto)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();

            producto.Nombre = dto.Nombre;
            producto.Precio = dto.Precio;
            producto.Descripcion = dto.Descripcion;

            _context.SaveChanges();
            return Ok(new { mensaje = "Producto modificado", producto });
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();

            producto.Activo = false;
            _context.SaveChanges();
            return Ok(new { mensaje = "Producto eliminado (inactivado)" });
        }

        [HttpGet("Todos")]
        public IActionResult ObtenerTodos()
        {
            var productos = _context.Productos.Where(p => p.Activo).ToList();
            return Ok(productos);
        }
        [Authorize]
        [HttpGet("Descargar/{nombreArchivo}")]
        public IActionResult Descargar(string nombreArchivo)
        {
            var carpeta = Path.Combine(_env.WebRootPath, "archivos");
            var rutaArchivo = Path.Combine(carpeta, nombreArchivo);

            if (!System.IO.File.Exists(rutaArchivo))
                return NotFound("Archivo no encontrado.");

            // Obtener el tipo MIME del archivo
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(rutaArchivo, out var contentType))
            {
                contentType = "application/octet-stream"; // Tipo genérico si no se detecta
            }

            var bytes = System.IO.File.ReadAllBytes(rutaArchivo);
            return File(bytes, contentType, nombreArchivo);
        }
    }
}
