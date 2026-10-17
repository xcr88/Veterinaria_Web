using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Veterinaria_Web.Modelos;

namespace Veterinaria_Web.Repositorios
{
    public class PropietarioRepositorio
    {
        // Guarda la cadena de conexión obtenida de Web.config.
        private readonly string _cadenaConexion;

        public PropietarioRepositorio()
        {
            _cadenaConexion = ConfigurationManager
                .ConnectionStrings["Conexion"]
                .ConnectionString;
        }

        // =====================================================
        // PRUEBA DE CONEXIÓN
        // =====================================================

        public bool ProbarConexion()
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                return true;
            }
        }

        // =====================================================
        // READ: CONSULTAR TODOS LOS PROPIETARIOS
        // =====================================================

        public List<Propietario> ConsultarTodos()
        {
            List<Propietario> lista =
                new List<Propietario>();

            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand comando =
                    new SqlCommand(
                        "sp_ConsultarPropietarios",
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
                            Propietario propietario =
                                new Propietario
                                {
                                    ID_PROPIETARIO =
                                        Convert.ToInt32(
                                            lector["ID_PROPIETARIO"]),

                                    IDENTIFICACION =
                                        Convert.ToString(
                                            lector["IDENTIFICACION"]),

                                    PRIMER_NOMBRE =
                                        Convert.ToString(
                                            lector["PRIMER_NOMBRE"]),

                                    SEGUNDO_NOMBRE =
                                        lector["SEGUNDO_NOMBRE"] == DBNull.Value
                                            ? null
                                            : Convert.ToString(
                                                lector["SEGUNDO_NOMBRE"]),

                                    PRIMER_APELLIDO =
                                        Convert.ToString(
                                            lector["PRIMER_APELLIDO"]),

                                    SEGUNDO_APELLIDO =
                                        lector["SEGUNDO_APELLIDO"] == DBNull.Value
                                            ? null
                                            : Convert.ToString(
                                                lector["SEGUNDO_APELLIDO"]),

                                    TELEFONO =
                                        Convert.ToString(
                                            lector["TELEFONO"]),

                                    CORREO =
                                        Convert.ToString(
                                            lector["CORREO"]),

                                    FECHA_ADICION =
                                        Convert.ToDateTime(
                                            lector["FECHA_ADICION"]),

                                    ADICIONADO_POR =
                                        Convert.ToString(
                                            lector["ADICIONADO_POR"]),

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

                            lista.Add(propietario);
                        }
                    }
                }
            }

            return lista;
        }

        // =====================================================
        // READ: CONSULTAR PROPIETARIO POR IDENTIFICADOR
        // =====================================================

        public Propietario ConsultarPorId(int idPropietario)
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand comando =
                    new SqlCommand(
                        "sp_ConsultarPropietarioPorId",
                        conexion))
                {
                    comando.CommandType =
                        CommandType.StoredProcedure;

                    comando.Parameters.Add(
                        "@ID_PROPIETARIO",
                        SqlDbType.Int
                    ).Value = idPropietario;

                    conexion.Open();

                    using (SqlDataReader lector =
                        comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return new Propietario
                            {
                                ID_PROPIETARIO =
                                    Convert.ToInt32(
                                        lector["ID_PROPIETARIO"]),

                                IDENTIFICACION =
                                    Convert.ToString(
                                        lector["IDENTIFICACION"]),

                                PRIMER_NOMBRE =
                                    Convert.ToString(
                                        lector["PRIMER_NOMBRE"]),

                                SEGUNDO_NOMBRE =
                                    lector["SEGUNDO_NOMBRE"] == DBNull.Value
                                        ? null
                                        : Convert.ToString(
                                            lector["SEGUNDO_NOMBRE"]),

                                PRIMER_APELLIDO =
                                    Convert.ToString(
                                        lector["PRIMER_APELLIDO"]),

                                SEGUNDO_APELLIDO =
                                    lector["SEGUNDO_APELLIDO"] == DBNull.Value
                                        ? null
                                        : Convert.ToString(
                                            lector["SEGUNDO_APELLIDO"]),

                                TELEFONO =
                                    Convert.ToString(
                                        lector["TELEFONO"]),

                                CORREO =
                                    Convert.ToString(
                                        lector["CORREO"]),

                                FECHA_ADICION =
                                    Convert.ToDateTime(
                                        lector["FECHA_ADICION"]),

                                ADICIONADO_POR =
                                    Convert.ToString(
                                        lector["ADICIONADO_POR"]),

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
            }

            // El identificador no existe.
            return null;
        }

        // =====================================================
        // CREATE: CREAR PROPIETARIO
        // =====================================================

        public int Crear(Propietario propietario)
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand comando =
                    new SqlCommand(
                        "sp_CrearPropietario",
                        conexion))
                {
                    comando.CommandType =
                        CommandType.StoredProcedure;

                    comando.Parameters.Add(
                        "@IDENTIFICACION",
                        SqlDbType.NVarChar,
                        15
                    ).Value = propietario.IDENTIFICACION;

                    comando.Parameters.Add(
                        "@PRIMER_NOMBRE",
                        SqlDbType.NVarChar,
                        25
                    ).Value = propietario.PRIMER_NOMBRE;

                    comando.Parameters.Add(
                        "@SEGUNDO_NOMBRE",
                        SqlDbType.NVarChar,
                        25
                    ).Value =
                        string.IsNullOrWhiteSpace(
                            propietario.SEGUNDO_NOMBRE)
                            ? (object)DBNull.Value
                            : propietario.SEGUNDO_NOMBRE;

                    comando.Parameters.Add(
                        "@PRIMER_APELLIDO",
                        SqlDbType.NVarChar,
                        25
                    ).Value = propietario.PRIMER_APELLIDO;

                    comando.Parameters.Add(
                        "@SEGUNDO_APELLIDO",
                        SqlDbType.NVarChar,
                        25
                    ).Value =
                        string.IsNullOrWhiteSpace(
                            propietario.SEGUNDO_APELLIDO)
                            ? (object)DBNull.Value
                            : propietario.SEGUNDO_APELLIDO;

                    comando.Parameters.Add(
                        "@TELEFONO",
                        SqlDbType.NVarChar,
                        8
                    ).Value = propietario.TELEFONO;

                    comando.Parameters.Add(
                        "@CORREO",
                        SqlDbType.NVarChar,
                        150
                    ).Value = propietario.CORREO;

                    comando.Parameters.Add(
                        "@ADICIONADO_POR",
                        SqlDbType.NVarChar,
                        50
                    ).Value = propietario.ADICIONADO_POR;

                    conexion.Open();

                    // Obtiene el ID generado por el procedimiento.
                    object resultado = comando.ExecuteScalar();

                    return Convert.ToInt32(resultado);
                }
            }
        }

        // =====================================================
        // UPDATE: ACTUALIZAR PROPIETARIO
        // =====================================================

        public bool Actualizar(Propietario propietario)
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand comando =
                    new SqlCommand(
                        "sp_ActualizarPropietario",
                        conexion))
                {
                    comando.CommandType =
                        CommandType.StoredProcedure;

                    comando.Parameters.Add(
                        "@ID_PROPIETARIO",
                        SqlDbType.Int
                    ).Value = propietario.ID_PROPIETARIO;

                    comando.Parameters.Add(
                        "@IDENTIFICACION",
                        SqlDbType.NVarChar,
                        15
                    ).Value = propietario.IDENTIFICACION;

                    comando.Parameters.Add(
                        "@PRIMER_NOMBRE",
                        SqlDbType.NVarChar,
                        25
                    ).Value = propietario.PRIMER_NOMBRE;

                    comando.Parameters.Add(
                        "@SEGUNDO_NOMBRE",
                        SqlDbType.NVarChar,
                        25
                    ).Value =
                        string.IsNullOrWhiteSpace(
                            propietario.SEGUNDO_NOMBRE)
                            ? (object)DBNull.Value
                            : propietario.SEGUNDO_NOMBRE;

                    comando.Parameters.Add(
                        "@PRIMER_APELLIDO",
                        SqlDbType.NVarChar,
                        25
                    ).Value = propietario.PRIMER_APELLIDO;

                    comando.Parameters.Add(
                        "@SEGUNDO_APELLIDO",
                        SqlDbType.NVarChar,
                        25
                    ).Value =
                        string.IsNullOrWhiteSpace(
                            propietario.SEGUNDO_APELLIDO)
                            ? (object)DBNull.Value
                            : propietario.SEGUNDO_APELLIDO;

                    comando.Parameters.Add(
                        "@TELEFONO",
                        SqlDbType.NVarChar,
                        8
                    ).Value = propietario.TELEFONO;

                    comando.Parameters.Add(
                        "@CORREO",
                        SqlDbType.NVarChar,
                        150
                    ).Value = propietario.CORREO;

                    comando.Parameters.Add(
                        "@MODIFICADO_POR",
                        SqlDbType.NVarChar,
                        50
                    ).Value = propietario.MODIFICADO_POR;

                    conexion.Open();

                    // Lee el SELECT @@ROWCOUNT del procedimiento.
                    int filasAfectadas =
                        Convert.ToInt32(comando.ExecuteScalar());

                    return filasAfectadas > 0;
                }
            }
        }
    }
}