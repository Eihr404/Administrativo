using Administracion.Datos;
using Administracion.DP;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace Administracion.MD
{
    internal class SucursalMD
    {
        public SucursalMD() { }

        /// <summary>
        /// Consulta todas las sucursales de la base de datos Oracle.
        /// </summary>
        internal List<SucursalDP> ConsultarAllMD()
        {
            List<SucursalDP> lista = new List<SucursalDP>();

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                // SQL basado en la descripción de columnas que proporcionaste
                string sql = @"SELECT SUC_CODIGO, EMP_CEDULA_RUC, SUC_NOMBRE, 
                                      SUC_DIRECCION, SUC_TELEFONO, SUC_CORREO, 
                                      SUC_REPRESENTANTE, SUC_CODIGO_SRI, SUC_NUM_FACTURA 
                               FROM SUCURSAL 
                               ORDER BY SUC_NOMBRE ASC";

                OracleCommand cmd = new OracleCommand(sql, conn);

                try
                {
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new SucursalDP
                            {
                                // Mapeo de columnas de la BD a propiedades del DP
                                SucCodigo = reader["SUC_CODIGO"].ToString(),
                                EmpCedulaRuc = reader["EMP_CEDULA_RUC"].ToString(),
                                SucNombre = reader["SUC_NOMBRE"] != DBNull.Value ? reader["SUC_NOMBRE"].ToString() : "",
                                SucDireccion = reader["SUC_DIRECCION"] != DBNull.Value ? reader["SUC_DIRECCION"].ToString() : "",
                                SucTelefono = reader["SUC_TELEFONO"] != DBNull.Value ? reader["SUC_TELEFONO"].ToString() : "",
                                SucCorreo = reader["SUC_CORREO"] != DBNull.Value ? reader["SUC_CORREO"].ToString() : "",
                                SucRepresentante = reader["SUC_REPRESENTANTE"] != DBNull.Value ? reader["SUC_REPRESENTANTE"].ToString() : "",
                                SucCodigoSri = reader["SUC_CODIGO_SRI"] != DBNull.Value ? reader["SUC_CODIGO_SRI"].ToString() : "",
                                SucNumFactura = reader["SUC_NUM_FACTURA"] != DBNull.Value ? reader["SUC_NUM_FACTURA"].ToString() : ""
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Lanza la excepción para ser capturada por la GUI
                    throw new Exception("Error en SucursalMD (ConsultarAllMD): " + ex.Message);
                }
            }
            return lista;
        }
    }
}