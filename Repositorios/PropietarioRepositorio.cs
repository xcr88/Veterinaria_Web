using System.Configuration;
using System.Data.SqlClient;

namespace Veterinaria_Web.Repositorios
{
    public class PropietarioRepositorio
    {
        // Guarda la cadena de conexión durante el uso del repositorio.
        private readonly string _cadenaConexion;

        // Constructor: se ejecuta al crear un objeto del repositorio.
        public PropietarioRepositorio()
        {
            _cadenaConexion = ConfigurationManager
                .ConnectionStrings["Conexion"]
                .ConnectionString;
        }
        // Comprueba si podemos abrir la conexión con SQL Server.
        public bool ProbarConexion()
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                return true;
            }
        }
    }
}