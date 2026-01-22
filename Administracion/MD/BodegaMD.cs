using Administracion.Datos;
using Administracion.DP;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracion.MD
{
    internal class BodegaMD
    {
        public BodegaMD() { }

        /* Consulta todas las bodegas existentes en la base de datos */
        internal List<BodegaDP> ConsultarAllMD()
        {
            List<BodegaDP> lista = new List<BodegaDP>();

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                // Seleccionamos los campos según tu estructura de tabla
                string sql = @"SELECT BOD_CODIGO, SUC_CODIGO, BOD_DIRECCION, 
                                      BOD_DESCRIPCION, BOD_CAPACIDAD 
                               FROM BODEGA 
                               ORDER BY BOD_CODIGO ASC";

                OracleCommand cmd = new OracleCommand(sql, conn);

                try
                {
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new BodegaDP
                            {
                                // Asignamos los valores al objeto DP
                                Codigo = reader["BOD_CODIGO"].ToString(),
                                SucursalCodigo = reader["SUC_CODIGO"].ToString(),
                                Direccion = reader["BOD_DIRECCION"] != DBNull.Value ? reader["BOD_DIRECCION"].ToString() : "",
                                Nombre = reader["BOD_DESCRIPCION"] != DBNull.Value ? reader["BOD_DESCRIPCION"].ToString() : "",
                                Capacidad = reader["BOD_CAPACIDAD"] != DBNull.Value ? Convert.ToDouble(reader["BOD_CAPACIDAD"]) : 0.0
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Usamos la configuración de error si la tienes, o un mensaje genérico
                    throw new Exception("Error en BodegaMD (ConsultarAllMD): " + ex.Message);
                }
            }
            return lista;
        }
    }
}
