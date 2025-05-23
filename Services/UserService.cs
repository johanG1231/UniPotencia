using Aspose.Pdf;
using Backend_PotenciaPC.Dtos;
using Backend_PotenciaPC.Repositories;
using Backend_PotenciaPC.Utilities;

namespace Backend_PotenciaPC.Services
{
    public class UserService
    {
        private readonly JwtSettingsDto _jwtSettings;
        private readonly UserRepository _userRepository;

        public UserService(JwtSettingsDto jwtSettings, UserRepository userRepository)
        {
            _jwtSettings = jwtSettings;
            _userRepository = userRepository;
        }

        public async Task<ResponseGeneralDto> CrearUsuario(RequestUserDto requestUserDto)
        {
            ResponseGeneralDto responseGeneralDto = new();

            //Validacion de usuario
            requestUserDto.Password = Encrypt.HashPassword(requestUserDto.Password);
            var result = await _userRepository.CrearUsuario(requestUserDto);
            if (result != 0)
            {
                responseGeneralDto.Respuesta = 1;
                responseGeneralDto.Mensaje = "Ususario creado exitosamente.";
            }
            else
            {
                responseGeneralDto.Respuesta = 0;
                responseGeneralDto.Mensaje = "Usuario ya esxiste.";
            }

            return responseGeneralDto;
        }

        public async Task<ResponseInicionSesionDto> InicioSesion(InicioSesionDto requestInicioSesionDto)
        {
            ResponseInicionSesionDto responseInicionSesionDto = new();

            // Obtener entidad User (no DTO)
            var user = await _userRepository.ObtenerEntidadUsuarioPorEmail(requestInicioSesionDto.NombreUsuario);

            if (user == null)
            {
                responseInicionSesionDto.Respuesta = 0;
                responseInicionSesionDto.Mensaje = "Usuario y/o Contraseña están incorrectos.";
                return responseInicionSesionDto;
            }

            // Verificar contraseña hasheada
            bool esValida = Encrypt.VerifyPassword(requestInicioSesionDto.Contrasena, user.Contraseña);
            if (!esValida)
            {
                responseInicionSesionDto.Respuesta = 0;
                responseInicionSesionDto.Mensaje = "Usuario y/o Contraseña están incorrectos.";
                return responseInicionSesionDto;
            }

            // Verificar si está confirmado
            if (!user.Confirmado)
            {
                responseInicionSesionDto.Respuesta = 0;
                responseInicionSesionDto.Mensaje = "Debes confirmar tu cuenta antes de iniciar sesión.";
                return responseInicionSesionDto;
            }

            // Convertir a DTO
            var userDto = new ResponseUserDto
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Correo = user.Correo,
                Rol = user.Rol?.Nombre ?? "Sin rol",
                Confirmado = user.Confirmado
            };

            responseInicionSesionDto.Usuario = userDto;

            var tokenGenerado = JwtUtility.GenTokenkey(responseInicionSesionDto, _jwtSettings);

            if (tokenGenerado == null)
            {
                responseInicionSesionDto.Respuesta = 0;
                responseInicionSesionDto.Mensaje = "Error al generar el token.";
                return responseInicionSesionDto;
            }

            tokenGenerado.Respuesta = 1;
            tokenGenerado.Mensaje = "Inicio de Sesión Exitoso. Bienvenid@ " + user.Nombre;
            return tokenGenerado;
        }





        public async Task<List<ResponseUserDto>> ObtenerUsuarios()
        {

            return await _userRepository.ObtenerUsuarios();
        }

        public async Task<string> ObtenerReporte()
        {
            var htmlCode = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Utilities\\Templates\\template1.html");
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlCode);
            //doc.Save(AppDomain.CurrentDomain.BaseDirectory + "Template\\invoice1.pdf");
            byte[] data = doc.Save();
            var result = Convert.ToBase64String(data);
            doc.Close();
            return result;
        }

        public async Task<string> ObtenerReporteAspose()
        {
            string ruta = "C:\\Users\\cabal\\Desktop\\Reportes\\document-" + DateTime.Now.Second + ".pdf";
            // Inicializar objeto de documento
            Document document = new Document();
            // Añadir página
            Page page = document.Pages.Add();
            // Agregar texto a la nueva página
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("REPORTE CON ASPOSE"));

            // Inicializa una nueva instancia de la tabla.
            Aspose.Pdf.Table table = new Aspose.Pdf.Table();

            // Establezca el color del borde de la tabla como LightGray
            table.Border = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.FromRgb(System.Drawing.Color.LightGray));

            // Establecer el borde de las celdas de la tabla
            table.DefaultCellBorder = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.FromRgb(System.Drawing.Color.LightGray));

            // Crea un bucle para agregar 10 filas
            for (int row_count = 1; row_count < 10; row_count++)
            {
                // Agregar fila a la tabla
                Aspose.Pdf.Row row = table.Rows.Add();
                // Agregar celdas de tabla
                row.Cells.Add("Column (" + row_count + ", 1)");
                row.Cells.Add("Column (" + row_count + ", 2)");
                row.Cells.Add("Column (" + row_count + ", 3)");
            }

            // Agregar objeto de tabla a la primera página del documento de entrada
            document.Pages[1].Paragraphs.Add(table);

            // Guardar PDF 
            document.Save(ruta);
            return ruta;
        }

        private string ImageToBase64(string imagePath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }
    }
}
