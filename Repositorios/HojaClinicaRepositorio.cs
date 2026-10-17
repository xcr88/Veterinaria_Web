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
        private readonly string _cadenaConexion;

        public HojaClinicaRepositorio()
        {
            _cadenaConexion = ConfigurationManager
                .ConnectionStrings["Conexion"]
                .ConnectionString;
        }

        // Obtiene todas las atenciones registradas.
        public List<HojaClinica> ConsultarTodos()
        {
            List<HojaClinica> hojas = new List<HojaClinica>();

            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            using (SqlCommand comando =
                new SqlCommand("sp_ConsultarHojasClinicas", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        hojas.Add(MapearHojaClinica(lector));
                    }
                }
            }

            return hojas;
        }

        // Busca una atención por su identificador.
        public HojaClinica ConsultarPorId(int idHoja)
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            using (SqlCommand comando =
                new SqlCommand("sp_ConsultarHojaClinicaPorId", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add(
                    "@ID_HOJA",
                    SqlDbType.Int).Value = idHoja;

                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        return MapearHojaClinica(lector);
                    }
                }
            }

            return null;
        }

        // Registra una atención y devuelve su identificador.
        public int Crear(HojaClinica hoja)
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            using (SqlCommand comando =
                new SqlCommand("sp_CrearHojaClinica", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                AgregarDatos(comando, hoja);

                comando.Parameters.Add(
                    "@ADICIONADO_POR",
                    SqlDbType.NVarChar,
                    50).Value = hoja.ADICIONADO_POR;

                conexion.Open();

                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }

        // Actualiza una atención existente.
        public bool Actualizar(HojaClinica hoja)
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            using (SqlCommand comando =
                new SqlCommand("sp_ActualizarHojaClinica", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add(
                    "@ID_HOJA",
                    SqlDbType.Int).Value = hoja.ID_HOJA;

                AgregarDatos(comando, hoja);

                comando.Parameters.Add(
                    "@MODIFICADO_POR",
                    SqlDbType.NVarChar,
                    50).Value = hoja.MODIFICADO_POR;

                conexion.Open();

                int filasAfectadas =
                    Convert.ToInt32(comando.ExecuteScalar());

                return filasAfectadas > 0;
            }
        }

        // Elimina únicamente la atención indicada.
        // Devuelve false si el registro ya no existe.
        public bool Eliminar(int idHoja)
        {
            using (SqlConnection conexion =
                new SqlConnection(_cadenaConexion))
            using (SqlCommand comando =
                new SqlCommand("sp_EliminarHojaClinica", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add(
                    "@ID_HOJA",
                    SqlDbType.Int).Value = idHoja;

                conexion.Open();

                int filasAfectadas =
                    Convert.ToInt32(comando.ExecuteScalar());

                return filasAfectadas > 0;
            }
        }

        // Datos compartidos por la creación y la actualización.
        private void AgregarDatos(
            SqlCommand comando,
            HojaClinica hoja)
        {
            comando.Parameters.Add(
                "@FECHA_ATENCION",
                SqlDbType.Date).Value = hoja.FECHA_ATENCION.Date;

            comando.Parameters.Add(
                "@SINTOMAS",
                SqlDbType.NVarChar,
                150).Value = hoja.SINTOMAS;

            comando.Parameters.Add(
                "@DIAGNOSTICO",
                SqlDbType.NVarChar,
                150).Value = hoja.DIAGNOSTICO;

            comando.Parameters.Add(
                "@TRATAMIENTO",
                SqlDbType.NVarChar,
                150).Value = hoja.TRATAMIENTO;

            comando.Parameters.Add(
                "@ID_MASCOTA",
                SqlDbType.Int).Value = hoja.ID_MASCOTA;
        }

        // Convierte una fila de SQL Server en un objeto del modelo.
        private HojaClinica MapearHojaClinica(SqlDataReader lector)
        {
            return new HojaClinica
            {
                ID_HOJA = Convert.ToInt32(
                    lector["ID_HOJA"]),

                FECHA_ATENCION = Convert.ToDateTime(
                    lector["FECHA_ATENCION"]),

                SINTOMAS = Convert.ToString(
                    lector["SINTOMAS"]),

                DIAGNOSTICO = Convert.ToString(
                    lector["DIAGNOSTICO"]),

                TRATAMIENTO = Convert.ToString(
                    lector["TRATAMIENTO"]),

                ID_MASCOTA = Convert.ToInt32(
                    lector["ID_MASCOTA"]),

                FECHA_ADICION = Convert.ToDateTime(
                    lector["FECHA_ADICION"]),

                ADICIONADO_POR = Convert.ToString(
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