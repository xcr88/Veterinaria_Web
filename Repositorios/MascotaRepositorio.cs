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

            return null;
        }

        // =====================================================
        // CREATE: CREAR MASCOTA
        // =====================================================

        public int Crear(Mascota mascota)
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand comando =
                    new SqlCommand(
                        "sp_CrearMascota",
                        conexion))
                {
                    comando.CommandType =
                        CommandType.StoredProcedure;

                    AgregarDatos(comando, mascota);

                    comando.Parameters.Add(
                        "@ADICIONADO_POR",
                        SqlDbType.NVarChar,
                        50
                    ).Value = mascota.ADICIONADO_POR;

                    conexion.Open();

                    // El procedimiento devuelve el ID generado.
                    return Convert.ToInt32(
                        comando.ExecuteScalar());
                }
            }
        }

        // =====================================================
        // UPDATE: ACTUALIZAR MASCOTA
        // =====================================================

        public bool Actualizar(Mascota mascota)
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand comando =
                    new SqlCommand(
                        "sp_ActualizarMascota",
                        conexion))
                {
                    comando.CommandType =
                        CommandType.StoredProcedure;

                    // Identifica la mascota que se modificará.
                    comando.Parameters.Add(
                        "@ID_MASCOTA",
                        SqlDbType.Int
                    ).Value = mascota.ID_MASCOTA;

                    AgregarDatos(comando, mascota);

                    comando.Parameters.Add(
                        "@MODIFICADO_POR",
                        SqlDbType.NVarChar,
                        50
                    ).Value = mascota.MODIFICADO_POR;

                    conexion.Open();

                    // Lee FILAS_AFECTADAS del procedimiento.
                    int filasAfectadas =
                        Convert.ToInt32(comando.ExecuteScalar());

                    return filasAfectadas > 0;
                }
            }
        }

        // =====================================================
        // AUXILIAR: PARÁMETROS COMUNES DE CREAR Y ACTUALIZAR
        // =====================================================

        private void AgregarDatos(
            SqlCommand comando,
            Mascota mascota)
        {
            comando.Parameters.Add(
                "@NOMBRE",
                SqlDbType.NVarChar,
                15
            ).Value = mascota.NOMBRE;

            comando.Parameters.Add(
                "@FECHA_NACIMIENTO",
                SqlDbType.Date
            ).Value = mascota.FECHA_NACIMIENTO.Date;

            comando.Parameters.Add(
                "@SEXO",
                SqlDbType.NVarChar,
                1
            ).Value = mascota.SEXO;

            // Coincide con DECIMAL(5,2) de SQL Server.
            SqlParameter parametroPeso =
                comando.Parameters.Add(
                    "@PESO",
                    SqlDbType.Decimal);

            parametroPeso.Precision = 5;
            parametroPeso.Scale = 2;
            parametroPeso.Value = mascota.PESO;

            // Convierte las alergias no informadas en NULL.
            comando.Parameters.Add(
                "@ALERGIAS",
                SqlDbType.NVarChar,
                150
            ).Value =
                string.IsNullOrWhiteSpace(mascota.ALERGIAS)
                    ? (object)DBNull.Value
                    : mascota.ALERGIAS;

            comando.Parameters.Add(
                "@ID_PROPIETARIO",
                SqlDbType.Int
            ).Value = mascota.ID_PROPIETARIO;
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
                        : Convert.ToString(
                            lector["MODIFICADO_POR"])
            };
        }
    }
}