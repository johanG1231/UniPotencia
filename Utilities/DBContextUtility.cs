using System;
using System.Data.SqlClient;

namespace Backend_PotenciaPC.Utilities
{
    public class DBContextUtility
    {
        private static readonly string SERVER = "JOHAN\\SQLEXPRESS";
        private static readonly string DATABASE = "PotenciaPC";

        // Cadena de conexión usando autenticación de Windows
        private static readonly string connectionString = $"Server=JOHAN\\SQLEXPRESS;Database=PotenciaPC;Integrated Security=True;TrustServerCertificate=True;";

        private SqlConnection connection;

        public DBContextUtility()
        {
            connection = new SqlConnection(connectionString);
        }

        // Método para abrir la conexión
        public void Connect()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                    Console.WriteLine("Conexión abierta.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir la conexión: {ex.Message}");
            }
        }

        // Método para cerrar la conexión
        public void Disconnect()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Conexión cerrada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar la conexión: {ex.Message}");
            }
        }

        // Método para obtener la conexión actual
        public SqlConnection CONN()
        {
            return connection;
        }
    }
}
