using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend_PotenciaPC.Dtos;
using Backend_PotenciaPC.Services;
using Microsoft.EntityFrameworkCore;
using Backend_PotenciaPC.Repositories.Models;

namespace Backend_PotenciaPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TestContext _context;

        public UserController(UserService userService, TestContext context)
        {
            _userService = userService;
            _context = context;
        }
        [HttpGet("ConfirmarCuenta")]
        public async Task<IActionResult> ConfirmarCuenta(string token)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.TokenConfirmacion == token);

            if (usuario == null)
            {
                return NotFound("Token inválido.");
            }

            if (usuario.Confirmado)
            {
                return BadRequest("La cuenta ya fue confirmada.");
            }

            usuario.Confirmado = true;
            usuario.TokenConfirmacion = null;

            await _context.SaveChangesAsync();

            return Ok("Cuenta confirmada.");
        }


        [HttpGet]
        [Route("/api/[Controller]/EstadoApi")]
        public async Task<IActionResult> EstadoApi()
        {
            ResponseGeneralDto resposeGeneralDto = new()
            {
                Respuesta = 200,
                Mensaje = "API EN EJECUCION CORRECTA"
            };
            return Ok(resposeGeneralDto);
        }

        [HttpPost]
        [Route("InicioSesion")]
        [AllowAnonymous]
        public async Task<IActionResult> PostIniciarSesion([FromBody] InicioSesionDto requestInicioSesionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _userService.InicioSesion(requestInicioSesionDto));
        }

        [HttpPost]
        [Route("/api/[Controller]/CrearUsuario")]
        public async Task<IActionResult> CrearUsuarios([FromBody] RequestUserDto requestUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResponseGeneralDto responseGeneralDto = await _userService.CrearUsuario(requestUserDto);

            return Ok(responseGeneralDto);
        }

        [HttpPost]
        [Route("/api/[Controller]/ObtenerUsuarios")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUsuarios()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _userService.ObtenerUsuarios());
        }

        [HttpGet]
        [Route("/api/[Controller]/ReportePDF64")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReportePDF64()
        {
            ResponseGeneralDto resposeGeneralDto = new();
            resposeGeneralDto.Respuesta = 201;
            resposeGeneralDto.Mensaje = await _userService.ObtenerReporte();
            return Ok(resposeGeneralDto);
        }

        [HttpGet]
        [Route("/api/[Controller]/ReporteAspose")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReporteAspose()
        {
            ResponseGeneralDto resposeGeneralDto = new();
            resposeGeneralDto.Respuesta = 201;
            resposeGeneralDto.Mensaje = await _userService.ObtenerReporteAspose();
            return Ok(resposeGeneralDto);
        }

        [HttpGet("ConfirmarCorreo")]
        public async Task<IActionResult> ConfirmarCorreo(string token)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.TokenConfirmacion == token);

            if (usuario == null)
                return BadRequest("Token inválido o ya fue confirmado.");

            usuario.Confirmado = true;
            usuario.TokenConfirmacion = null;

            await _context.SaveChangesAsync();

            return Ok("Cuenta confirmada. Ya puedes iniciar sesión.");
        }



    }
}
