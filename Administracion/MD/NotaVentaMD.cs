using Administracion.Datos;
using Administracion.DP;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Administracion.MD
{
    public class NotaVentaMD
    {
        /* Método para consulta general */
        public List<NotaVentaDP> ConsultarAllMD()
        {
            List<NotaVentaDP> lista = new List<NotaVentaDP>();
            string query = @"
                    SELECT 
                        NDV_NUMERO, SUC_CODIGO, PDD_CODIGO, 
                        NDV_FECHA_EMISION, NDV_MONTO_TOTAL, 
                        NDV_RESPONSABLE, NDV_DESCRIPCION
                    FROM NOTA_VENTA";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(query, conn);
                try
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new NotaVentaDP
                        {
                            NdvNumero = reader["NDV_NUMERO"].ToString(),
                            SucCodigo = reader["SUC_CODIGO"].ToString(),
                            PddCodigo = reader["PDD_CODIGO"].ToString(),
                            NdvFechaEmision = reader.IsDBNull(reader.GetOrdinal("NDV_FECHA_EMISION")) ? DateTime.Now : Convert.ToDateTime(reader["NDV_FECHA_EMISION"]),
                            NdvMontoTotal = reader.IsDBNull(reader.GetOrdinal("NDV_MONTO_TOTAL")) ? 0 : Convert.ToDouble(reader["NDV_MONTO_TOTAL"]),
                            NdvResponsable = reader["NDV_RESPONSABLE"]?.ToString() ?? "",
                            NdvDescripcion = reader["NDV_DESCRIPCION"]?.ToString() ?? ""
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"{OracleDB.GetConfig("error.general")} (ConsultarAllMD): {ex.Message}");
                }
            }
            return lista;
        }

        /* Método para consulta por parámetro (número) */
        public List<NotaVentaDP> ConsultarByCodMD(string numero)
        {
            List<NotaVentaDP> lista = new List<NotaVentaDP>();
            string query = "SELECT * FROM NOTA_VENTA WHERE NDV_NUMERO = :num";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter("num", numero));
                try
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new NotaVentaDP
                        {
                            NdvNumero = reader["NDV_NUMERO"].ToString(),
                            SucCodigo = reader["SUC_CODIGO"].ToString(),
                            PddCodigo = reader["PDD_CODIGO"].ToString(),
                            NdvFechaEmision = Convert.ToDateTime(reader["NDV_FECHA_EMISION"]),
                            NdvMontoTotal = Convert.ToDouble(reader["NDV_MONTO_TOTAL"]),
                            NdvResponsable = reader["NDV_RESPONSABLE"].ToString(),
                            NdvDescripcion = reader["NDV_DESCRIPCION"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"{OracleDB.GetConfig("error.general")} (ConsultarByCodMD): {ex.Message}");
                }
            }
            return lista;
        }

        /* Método para Insertar */
        public int IngresarMD(NotaVentaDP dp)
        {
            string sql = @"
                INSERT INTO NOTA_VENTA (NDV_NUMERO, SUC_CODIGO, PDD_CODIGO, NDV_FECHA_EMISION, NDV_MONTO_TOTAL, NDV_RESPONSABLE, NDV_DESCRIPCION)
                VALUES (:num, :suc, :pdd, :fec, :mon, :res, :des)";

            using OracleConnection conn = OracleDB.CrearConexion();
            try
            {
                using OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":num", dp.NdvNumero);
                cmd.Parameters.Add(":suc", dp.SucCodigo);
                cmd.Parameters.Add(":pdd", dp.PddCodigo);
                cmd.Parameters.Add(":fec", dp.NdvFechaEmision);
                cmd.Parameters.Add(":mon", dp.NdvMontoTotal);
                cmd.Parameters.Add(":res", dp.NdvResponsable);
                cmd.Parameters.Add(":des", dp.NdvDescripcion);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { throw new Exception($"{OracleDB.GetConfig("error.general")} (IngresarMD): {ex.Message}"); }
        }

        /* Método para Actualizar */
        public int ActualizarMD(NotaVentaDP dp)
        {
            string sql = @"
                UPDATE NOTA_VENTA SET 
                    SUC_CODIGO = :suc, PDD_CODIGO = :pdd, NDV_FECHA_EMISION = :fec, 
                    NDV_MONTO_TOTAL = :mon, NDV_RESPONSABLE = :res, NDV_DESCRIPCION = :des
                WHERE NDV_NUMERO = :num";

            using OracleConnection conn = OracleDB.CrearConexion();
            try
            {
                using OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":suc", dp.SucCodigo);
                cmd.Parameters.Add(":pdd", dp.PddCodigo);
                cmd.Parameters.Add(":fec", dp.NdvFechaEmision);
                cmd.Parameters.Add(":mon", dp.NdvMontoTotal);
                cmd.Parameters.Add(":res", dp.NdvResponsable);
                cmd.Parameters.Add(":des", dp.NdvDescripcion);
                cmd.Parameters.Add(":num", dp.NdvNumero);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { throw new Exception($"{OracleDB.GetConfig("error.general")} (ActualizarMD): {ex.Message}"); }
        }

        /* Método para Eliminar */
        public int EliminarMD(string numero)
        {
            string sql = "DELETE FROM NOTA_VENTA WHERE NDV_NUMERO = :num";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.Parameters.Add(new OracleParameter("num", numero));
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { throw new Exception($"{OracleDB.GetConfig("error.general")} (EliminarMD): {ex.Message}"); }
            }
        }
    }
}