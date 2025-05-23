using System;
using System.Data.SqlClient;

namespace Backend_PotenciaPC.Utilities
{
    public class DBContextUtility
    {
        private readonly string connectionString;
        private SqlConnection connection;

        // Constructor que recibe la cadena de conexión
        public DBContextUtility(string connectionString)
        {
            this.connectionString = connectionString;
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
