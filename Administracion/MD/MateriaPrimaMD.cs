using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client; // Asegúrate de tener la referencia
using Administracion.DP;

namespace Administracion.MD
{
    public class MateriaPrimaMD
    {
        // Cadena de conexión (Ajustar según tu instancia)
        private string connectionString = "User Id=a_prueba;Password=lticPUCE24;Data Source=192.168.5.125:1521/PRUEBA";

        // MÉTODO PARA CONSULTAR
        public List<MateriaPrimaDP> Consultar(string codigoFiltro = "")
        {
            List<MateriaPrimaDP> lista = new List<MateriaPrimaDP>();
            string query = "SELECT * FROM MATERIA_PRIMA";

            // Ajustamos el nombre de la columna a MTP_CODIGO
            if (!string.IsNullOrEmpty(codigoFiltro))
                query += " WHERE MTP_CODIGO = :cod";

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand(query, conn);
                if (!string.IsNullOrEmpty(codigoFiltro))
                    cmd.Parameters.Add(new OracleParameter("cod", codigoFiltro));

                try
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MateriaPrimaDP materia = new MateriaPrimaDP
                        {
                            MtpCodigo = reader["MTP_CODIGO"].ToString(),
                            UmeCodigo = reader["UME_CODIGO"].ToString(),
                            MtpNombre = reader["MTP_NOMBRE"].ToString(),
                            MtpDescripcion = reader["MTP_DESCRIPCION"].ToString(),
                            MtpPrecioCompraAnt = Convert.ToDouble(reader["MTP_PRECIO_COMPRA_ANT"]),
                            MtpPrecioCompra = Convert.ToDouble(reader["MTP_PRECIO_COMPRA"])
                        };
                        lista.Add(materia);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al consultar en MD: " + ex.Message);
                }
            }
            return lista;
        }

        // MÉTODO PARA INSERTAR (Recibe un DP)
        public int Insertar(MateriaPrimaDP dp)
        {
            string sql = "INSERT INTO MATERIA_PRIMA (MTP_CODIGO, UME_CODIGO, MTP_NOMBRE, MTP_DESCRIPCION, MTP_PRECIO_COMPRA_ANT, MTP_PRECIO_COMPRA) " +
                 "VALUES (:cod, :uni, :nom, :des, :pant, :pact)";

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("cod", dp.MtpCodigo));
                cmd.Parameters.Add(new OracleParameter("uni", dp.UmeCodigo));
                cmd.Parameters.Add(new OracleParameter("nom", dp.MtpNombre));
                cmd.Parameters.Add(new OracleParameter("des", dp.MtpDescripcion));
                cmd.Parameters.Add(new OracleParameter("pant", dp.MtpPrecioCompraAnt));
                cmd.Parameters.Add(new OracleParameter("pact", dp.MtpPrecioCompra));

                conn.Open();
                return cmd.ExecuteNonQuery(); // Retorna filas afectadas
            }
        }

        // MÉTODO PARA ELIMINAR
        public int Eliminar(string codigo)
        {
            string sql = "DELETE FROM MATERIA_PRIMA WHERE MTP_CODIGO = :cod";

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("cod", codigo));
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
