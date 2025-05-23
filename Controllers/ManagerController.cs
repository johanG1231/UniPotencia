using Microsoft.AspNetCore.Mvc;
using Backend_PotenciaPC.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using SelectPdf;

namespace Backend_PotenciaPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly TestContext _context;

        public ManagerController(TestContext context)
        {
            _context = context;
        }

        [HttpGet("ReporteDescargasUsuarios")]
        public async Task<IActionResult> ReporteDescargasUsuarios()
        {
            var datos = await _context.Usuarios
                .Select(u => new
                {
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    CantidadDescargas = _context.Descargas.Count(d => d.UsuarioId == u.Id)
                })
                .ToListAsync();

            // Crear documento PDF
            var doc = new PdfDocument();
            var page = doc.AddPage();

            var html = @"
<html>
<head>
    <style>
        html, body {
        height: 100%;
        margin: 0;
        padding: 0;
        font-family: Arial, sans-serif;
        background: linear-gradient(135deg, #0f2027, #203a43, #2c5364);
        color: white;
    }

    .container {
        padding: 40px;
    }

    .header {
        text-align: center;
        margin-bottom: 20px;
    }

    .logo {
        width: 150px;
        margin-bottom: 10px;
    }

    h2 {
        margin-top: 0;
        color: #ffffff;
    }

    table {
        width: 100%;
        border-collapse: collapse;
        margin-top: 20px;
        background-color: rgba(255, 255, 255, 0.1);
    }

    th, td {
        border: 1px solid #ccc;
        padding: 8px;
        text-align: center;
        color: #ffffff;
    }

    th {
        background-color: rgba(0, 0, 0, 0.5);
    }
    </style>
</head>
<body>
    <div class='header'>
        <img class='logo' src='https://drive.google.com/uc?export=view&id=1v0_ZRgQwcXKnyNP1ZmiUAUXIipqtmJ8F' />
        <h2>Reporte de Descargas por Usuario</h2>
    </div>
    <table>
        <thead>
            <tr>
                <th>Nombre</th>
                <th>Correo</th>
                <th>Total Descargas</th>
            </tr>
        </thead>
        <tbody>";



            foreach (var d in datos)
            {
                html += $"<tr><td>{d.Nombre}</td><td>{d.Correo}</td><td>{d.CantidadDescargas}</td></tr>";
            }

            html += @"
                    </tbody>
                </table>
                <p style='text-align:right;'>Generado: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "</p>";

            // Convertir HTML a PDF
            var converter = new HtmlToPdf();
            var pdf = converter.ConvertHtmlString(html);

            var pdfBytes = pdf.Save();
            pdf.Close();

            return File(pdfBytes, "application/pdf", "ReporteDescargasUsuarios.pdf");
        }
    }
}
