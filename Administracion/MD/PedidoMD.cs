using Administracion.Datos;
using Administracion.DP;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Administracion.MD
{
    public class PedidoMD
    {
        /* Método para consulta general */
        public List<PedidoDP> ConsultarAllMD()
        {
            List<PedidoDP> lista = new List<PedidoDP>();
            string query = @"
                    SELECT 
                        PDD_CODIGO,
                        CLI_CEDULA,
                        PDD_COMENTARIO,
                        PDD_ESTADO,
                        PDD_UBICACION,
                        PDD_MONTO_TOTAL,
                        PDD_ABONO
                    FROM PEDIDO";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(query, conn);
                try
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new PedidoDP
                        {
                            PddCodigo = reader["PDD_CODIGO"].ToString(),
                            CliCedula = reader["CLI_CEDULA"].ToString(),
                            PddComentario = reader["PDD_COMENTARIO"]?.ToString() ?? "",
                            PddEstado = reader["PDD_ESTADO"]?.ToString() ?? "",
                            PddUbicacion = reader["PDD_UBICACION"]?.ToString() ?? "",

                            PddMontoTotal = reader.IsDBNull(reader.GetOrdinal("PDD_MONTO_TOTAL")) ? 0 : Convert.ToDouble(reader["PDD_MONTO_TOTAL"]),
                            PddAbono = reader.IsDBNull(reader.GetOrdinal("PDD_ABONO")) ? 0 : Convert.ToDouble(reader["PDD_ABONO"])
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

        /* Método para consulta por parámetro (código) */
        public List<PedidoDP> ConsultarByCodMD(string codigo)
        {
            List<PedidoDP> lista = new List<PedidoDP>();
            string query = "SELECT * FROM PEDIDO WHERE PDD_CODIGO = :codigo";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter("codigo", codigo));
                try
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new PedidoDP
                        {
                            PddCodigo = reader["PDD_CODIGO"].ToString(),
                            CliCedula = reader["CLI_CEDULA"].ToString(),
                            PddComentario = reader["PDD_COMENTARIO"]?.ToString() ?? "",
                            PddEstado = reader["PDD_ESTADO"]?.ToString() ?? "",
                            PddUbicacion = reader["PDD_UBICACION"]?.ToString() ?? "",

                            PddMontoTotal = reader.IsDBNull(reader.GetOrdinal("PDD_MONTO_TOTAL")) ? 0 : Convert.ToDouble(reader["PDD_MONTO_TOTAL"]),
                            PddAbono = reader.IsDBNull(reader.GetOrdinal("PDD_ABONO")) ? 0 : Convert.ToDouble(reader["PDD_ABONO"])
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

        /* Método para ingresar nuevo pedido */
        public int IngresarMD(PedidoDP dp)
        {
            string sql = @"
                    INSERT INTO PEDIDO
                    (
                        PDD_CODIGO,
                        CLI_CEDULA,
                        PDD_COMENTARIO,
                        PDD_ESTADO,
                        PDD_UBICACION,
                        PDD_MONTO_TOTAL,
                        PDD_ABONO
                    )
                    VALUES
                    (
                        :cod,
                        :ced,
                        :com,
                        :est,
                        :ubi,
                        :tot,
                        :abo
                    )";

            using OracleConnection conn = OracleDB.CrearConexion();
            try
            {
                using OracleCommand cmd = new OracleCommand(sql, conn);

                cmd.Parameters.Add(":cod", dp.PddCodigo);
                cmd.Parameters.Add(":ced", dp.CliCedula);
                cmd.Parameters.Add(":com", dp.PddComentario);
                cmd.Parameters.Add(":est", dp.PddEstado);
                cmd.Parameters.Add(":ubi", dp.PddUbicacion);
                cmd.Parameters.Add(":tot", dp.PddMontoTotal);
                cmd.Parameters.Add(":abo", dp.PddAbono);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"{OracleDB.GetConfig("error.general")} (IngresarMD): {ex.Message}");
            }
        }

        /* Método para actualizar pedido */
        public int ActualizarMD(PedidoDP dp)
        {
            string sql = @"
                    UPDATE PEDIDO
                    SET
                        CLI_CEDULA = :ced,
                        PDD_COMENTARIO = :com,
                        PDD_ESTADO = :est,
                        PDD_UBICACION = :ubi,
                        PDD_MONTO_TOTAL = :tot,
                        PDD_ABONO = :abo
                    WHERE PDD_CODIGO = :cod";

            using OracleConnection conn = OracleDB.CrearConexion();
            try
            {
                using OracleCommand cmd = new OracleCommand(sql, conn);

                cmd.Parameters.Add(":ced", dp.CliCedula);
                cmd.Parameters.Add(":com", dp.PddComentario);
                cmd.Parameters.Add(":est", dp.PddEstado);
                cmd.Parameters.Add(":ubi", dp.PddUbicacion);
                cmd.Parameters.Add(":tot", dp.PddMontoTotal);
                cmd.Parameters.Add(":abo", dp.PddAbono);
                cmd.Parameters.Add(":cod", dp.PddCodigo);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"{OracleDB.GetConfig("error.general")} (ActualizarMD): {ex.Message}");
            }
        }

        /* Método para eliminar pedido */
        public int EliminarMD(string codigo)
        {
            string sql = "DELETE FROM PEDIDO WHERE PDD_CODIGO = :cod";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.Parameters.Add(new OracleParameter("cod", codigo));
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"{OracleDB.GetConfig("error.general")} (EliminarMD): {ex.Message}");
                }
            }
        }
    }
}