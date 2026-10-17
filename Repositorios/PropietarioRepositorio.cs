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

        // Se ejecuta al crear un objeto del repositorio.
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
            List<Propietario> lista = new List<Propietario>();

            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand comando =
                    new SqlCommand(
                        "sp_ConsultarPropietarios",
                        conexion))
                {
                    // Indica que ejecutaremos un procedimiento.
                    comando.CommandType =
                        CommandType.StoredProcedure;

                    conexion.Open();

                    using (SqlDataReader lector =
                        comando.ExecuteReader())
                    {
                        // Lee una fila en cada repetición.
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

                            // Agrega el propietario a la lista.
                            lista.Add(propietario);
                        }
                    }
                }
            }

            return lista;
        }
    }
}