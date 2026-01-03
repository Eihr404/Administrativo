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
    internal class ClasificacionMD
    {
        /* Consultar todas las clasificaciones existentes en la base de datos */
        internal List<ClasificacionDP> ConsultarAllMD()
        {
            List<ClasificacionDP> lista = new List<ClasificacionDP>();

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                string sql = "SELECT CLA_CODIGO, CLA_NOMBRE FROM CLASIFICACION ORDER BY CLA_CODIGO ASC";
                OracleCommand cmd = new OracleCommand(sql, conn);

                try
                {
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ClasificacionDP
                            {
                                Codigo = reader["CLA_CODIGO"].ToString(),
                                Nombre = reader["CLA_NOMBRE"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error en ClasificacionMD: " + ex.Message);
                }
            }
            return lista;
        }
    }
}
