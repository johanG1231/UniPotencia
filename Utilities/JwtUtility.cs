using Microsoft.IdentityModel.Tokens;
using Backend_PotenciaPC.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend_PotenciaPC.Utilities
{
    /// <summary>JwtHelper</summary>
    public static class JwtUtility
    {
        /// <summary></summary>
        public static ResponseInicionSesionDto? GenTokenkey(ResponseInicionSesionDto userToken, JwtSettingsDto jwtSettings)
        {
            try
            {
                if (userToken == null) throw new ArgumentException(nameof(userToken));

                var clave = jwtSettings.IssuerSigningKey;

                if (string.IsNullOrWhiteSpace(clave))
                {
                    throw new ArgumentException("La clave secreta IssuerSigningKey no puede estar vacía.");
                }

                var key = System.Text.Encoding.ASCII.GetBytes(clave);


                DateTime expireTime = DateTime.Now;

                if (jwtSettings.FlagExpirationTimeHours)
                {
                    expireTime = DateTime.Now.AddHours(jwtSettings.ExpirationTimeHours);
                }
                else if (jwtSettings.FlagExpirationTimeMinutes)
                {
                    expireTime = DateTime.Now.AddMinutes(jwtSettings.ExpirationTimeMinutes);
                }
                else
                {
                    userToken.Respuesta = 0;
                    userToken.Mensaje = "Error en la configuración del tiempo de expiración del token.";
                    return userToken;
                }

                IEnumerable<Claim> claims = new Claim[] {
                    new Claim("UsuarioId", userToken.Usuario.Id.ToString()),
                    new Claim("Correo", userToken.Usuario.Correo),
                    new Claim("Rol", userToken.Usuario.Rol),
                    new Claim("TiempoExpiracion", expireTime.ToString("yyyy-MM-dd HH:mm:ss"))
                };


                var JWToken = new JwtSecurityToken(
                    issuer: jwtSettings.ValidIssuer,
                    audience: jwtSettings.ValidAudience,
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: expireTime,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

                userToken.Token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                Console.WriteLine("Token generado: " + userToken.Token);

                userToken.TiempoExpiracion = expireTime;
                return userToken;
            }
            catch (Exception ex)
            {
                // Puedes usar un logger o simplemente imprimir en consola (para debugging)
                Console.WriteLine($"Error al generar token JWT: {ex.Message}");
                if (userToken == null)
                {
                    return new ResponseInicionSesionDto
                    {
                        Respuesta = 0,
                        Mensaje = $"Error al generar el token: {ex.Message}"
                    };
                }
                else
                {
                    userToken.Respuesta = 0;
                    userToken.Mensaje = $"Error al generar el token: {ex.Message}";
                    return userToken;
                }
            }


        }

    }
}
