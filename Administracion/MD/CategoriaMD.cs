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
    internal class CategoriaMD
    {
        internal List<CategoriaDP> ConsultarTodosMD()
        {
            List<CategoriaDP> lista = new List<CategoriaDP>();

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                string sql = "SELECT CAT_CODIGO, CAT_NOMBRE FROM CATEGORIA ORDER BY CAT_CODIGO ASC";
                OracleCommand cmd = new OracleCommand(sql, conn);

                try
                {
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new CategoriaDP
                            {
                                Codigo = reader["CAT_CODIGO"].ToString(),
                                Descripcion = reader["CAT_NOMBRE"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error en CategoriaMD: " + ex.Message);
                }
            }
            return lista;
        }
    }
}
