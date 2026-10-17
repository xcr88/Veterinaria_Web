using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Veterinaria_Web.Modelos;

namespace Veterinaria_Web.Repositorios
{
    public class UsuarioRepositorio
    {
        private readonly string _cadenaConexion;

        // Lee la conexión configurada en Web.config.
        public UsuarioRepositorio()
        {
            _cadenaConexion = ConfigurationManager
                .ConnectionStrings["Conexion"]
                .ConnectionString;
        }

        // =====================================================
        // CONSULTAR USUARIO PARA VALIDAR EL INGRESO
        // =====================================================

        public Usuario ValidarIngreso(string usuario, string clave)
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand comando =
                    new SqlCommand(
                        "sp_ConsultarUsuario",
                        conexion))
                {
                    comando.CommandType =
                        CommandType.StoredProcedure;

                    comando.Parameters.Add(
                        "@USUARIO",
                        SqlDbType.NVarChar,
                        50
                    ).Value = usuario;

                    comando.Parameters.Add(
                        "@CLAVE",
                        SqlDbType.NVarChar,
                        10
                    ).Value = clave;

                    conexion.Open();

                    using (SqlDataReader lector =
                        comando.ExecuteReader())
                    {
                        // El procedimiento solo devuelve
                        // cuentas activas con datos coincidentes.
                        if (lector.Read())
                        {
                            return new Usuario
                            {
                                ID_USUARIO =
                                    Convert.ToInt32(
                                        lector["ID_USUARIO"]),

                                USUARIO =
                                    Convert.ToString(
                                        lector["USUARIO"])
                            };
                        }
                    }
                }
            }

            // No encontró una cuenta activa con esas credenciales.
            return null;
        }
    }
}