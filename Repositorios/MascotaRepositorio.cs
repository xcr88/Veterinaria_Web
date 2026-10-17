using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Veterinaria_Web.Modelos;

namespace Veterinaria_Web.Repositorios
{
    public class MascotaRepositorio
    {
        // Guarda la cadena de conexión obtenida de Web.config.
        private readonly string _cadenaConexion;

        public MascotaRepositorio()
        {
            _cadenaConexion = ConfigurationManager
                .ConnectionStrings["Conexion"]
                .ConnectionString;
        }

        // =====================================================
        // READ: CONSULTAR TODAS LAS MASCOTAS
        // =====================================================

        public List<Mascota> ConsultarTodos()
        {
            List<Mascota> lista = new List<Mascota>();

            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand comando =
                    new SqlCommand(
                        "sp_ConsultarMascotas",
                        conexion))
                {
                    comando.CommandType =
                        CommandType.StoredProcedure;

                    conexion.Open();

                    using (SqlDataReader lector =
                        comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(MapearMascota(lector));
                        }
                    }
                }
            }

            return lista;
        }

        // =====================================================
        // READ: CONSULTAR MASCOTA POR IDENTIFICADOR
        // =====================================================

        public Mascota ConsultarPorId(int idMascota)
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand comando =
                    new SqlCommand(
                        "sp_ConsultarMascotaPorId",
                        conexion))
                {
                    comando.CommandType =
                        CommandType.StoredProcedure;

                    comando.Parameters.Add(
                        "@ID_MASCOTA",
                        SqlDbType.Int
                    ).Value = idMascota;

                    conexion.Open();

                    using (SqlDataReader lector =
                        comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return MapearMascota(lector);
                        }
                    }
                }
            }

            // No existe una mascota con ese identificador.
            return null;
        }

        // =====================================================
        // AUXILIAR: CONVERTIR UNA FILA SQL EN UNA MASCOTA
        // =====================================================

        private Mascota MapearMascota(SqlDataReader lector)
        {
            return new Mascota
            {
                ID_MASCOTA =
                    Convert.ToInt32(lector["ID_MASCOTA"]),

                NOMBRE =
                    Convert.ToString(lector["NOMBRE"]),

                FECHA_NACIMIENTO =
                    Convert.ToDateTime(lector["FECHA_NACIMIENTO"]),

                SEXO =
                    Convert.ToString(lector["SEXO"]),

                PESO =
                    Convert.ToDecimal(lector["PESO"]),

                ALERGIAS =
                    lector["ALERGIAS"] == DBNull.Value
                        ? null
                        : Convert.ToString(lector["ALERGIAS"]),

                ID_PROPIETARIO =
                    Convert.ToInt32(lector["ID_PROPIETARIO"]),

                FECHA_ADICION =
                    Convert.ToDateTime(lector["FECHA_ADICION"]),

                ADICIONADO_POR =
                    Convert.ToString(lector["ADICIONADO_POR"]),

                FECHA_MODIFICACION =
                    lector["FECHA_MODIFICACION"] == DBNull.Value
                        ? (DateTime?)null
                        : Convert.ToDateTime(
                            lector["FECHA_MODIFICACION"]),

                MODIFICADO_POR =
                    lector["MODIFICADO_POR"] == DBNull.Value
                        ? null
                        : Convert.ToString(lector["MODIFICADO_POR"])
            };
        }
    }
}