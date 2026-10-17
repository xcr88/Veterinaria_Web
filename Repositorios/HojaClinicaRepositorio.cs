using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Veterinaria_Web.Modelos;

namespace Veterinaria_Web.Repositorios
{
    public class HojaClinicaRepositorio
    {
        // Guarda la cadena de conexión obtenida de Web.config.
        private readonly string _cadenaConexion;

        public HojaClinicaRepositorio()
        {
            _cadenaConexion = ConfigurationManager
                .ConnectionStrings["Conexion"]
                .ConnectionString;
        }

        // =====================================================
        // READ: CONSULTAR TODAS LAS HOJAS CLÍNICAS
        // =====================================================

        public List<HojaClinica> ConsultarTodos()
        {
            List<HojaClinica> lista =
                new List<HojaClinica>();

            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand comando =
                    new SqlCommand(
                        "sp_ConsultarHojasClinicas",
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
                            lista.Add(MapearHojaClinica(lector));
                        }
                    }
                }
            }

            return lista;
        }

        // =====================================================
        // READ: CONSULTAR HOJA CLÍNICA POR IDENTIFICADOR
        // =====================================================

        public HojaClinica ConsultarPorId(int idHoja)
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand comando =
                    new SqlCommand(
                        "sp_ConsultarHojaClinicaPorId",
                        conexion))
                {
                    comando.CommandType =
                        CommandType.StoredProcedure;

                    comando.Parameters.Add(
                        "@ID_HOJA",
                        SqlDbType.Int
                    ).Value = idHoja;

                    conexion.Open();

                    using (SqlDataReader lector =
                        comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return MapearHojaClinica(lector);
                        }
                    }
                }
            }

            // No existe una atención con ese identificador.
            return null;
        }

        // =====================================================
        // AUXILIAR: CONVERTIR UNA FILA SQL EN UNA HOJA CLÍNICA
        // =====================================================

        private HojaClinica MapearHojaClinica(
            SqlDataReader lector)
        {
            return new HojaClinica
            {
                ID_HOJA =
                    Convert.ToInt32(lector["ID_HOJA"]),

                FECHA_ATENCION =
                    Convert.ToDateTime(lector["FECHA_ATENCION"]),

                SINTOMAS =
                    Convert.ToString(lector["SINTOMAS"]),

                DIAGNOSTICO =
                    Convert.ToString(lector["DIAGNOSTICO"]),

                TRATAMIENTO =
                    Convert.ToString(lector["TRATAMIENTO"]),

                ID_MASCOTA =
                    Convert.ToInt32(lector["ID_MASCOTA"]),

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