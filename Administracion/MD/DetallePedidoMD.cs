using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using Administracion.Datos;
using Administracion.DP;

namespace Administracion.MD
{
    public class DetallePedidoMD
    {
        public List<DetallePedidoDP> ConsultarAllMD()
        {
            List<DetallePedidoDP> lista = new List<DetallePedidoDP>();
            string query = "SELECT * FROM DETALLE_PEDIDO";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(query, conn);
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new DetallePedidoDP
                        {
                            ProCodigo = reader["PRO_CODIGO"].ToString(),
                            PddCodigo = reader["PDD_CODIGO"].ToString(),
                            DppCantidad = Convert.ToInt32(reader["DPP_CANTIDAD"]),
                            DppMonto = Convert.ToDouble(reader["DPP_MONTO"])
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

        public List<DetallePedidoDP> ConsultarByCodMD(string criterio)
        {
            List<DetallePedidoDP> lista = new List<DetallePedidoDP>();
            string sql = "SELECT PRO_CODIGO, PDD_CODIGO, DPP_CANTIDAD, DPP_MONTO " +
                         "FROM DETALLE_PEDIDO " +
                         "WHERE PRO_CODIGO LIKE :criterio " +
                         "OR PDD_CODIGO LIKE :criterio";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        string filtro = "%" + criterio + "%";
                        cmd.Parameters.Add(":criterio", filtro);
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new DetallePedidoDP
                                {
                                    ProCodigo = dr.GetString(0),
                                    PddCodigo = dr.GetString(1),
                                    DppCantidad = dr.GetInt32(2),
                                    DppMonto = dr.GetDouble(3)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"{OracleDB.GetConfig("error.general")} (ConsultarByCodMD): {ex.Message}");
                }
            }
            return lista;
        }

        public int IngresarMD(DetallePedidoDP dp)
        {
            string sql = "INSERT INTO DETALLE_PEDIDO (PRO_CODIGO, PDD_CODIGO, DPP_CANTIDAD, DPP_MONTO) VALUES (:pro, :pdd, :can, :mon)";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.Parameters.Add(new OracleParameter("pro", dp.ProCodigo));
                    cmd.Parameters.Add(new OracleParameter("pdd", dp.PddCodigo));
                    cmd.Parameters.Add(new OracleParameter("can", dp.DppCantidad));
                    cmd.Parameters.Add(new OracleParameter("mon", dp.DppMonto));
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { throw new Exception($"{OracleDB.GetConfig("error.general")} (IngresarMD): {ex.Message}"); }
            }
        }

        public int ActualizarMD(DetallePedidoDP dp)
        {
            string sql = "UPDATE DETALLE_PEDIDO SET DPP_CANTIDAD = :can, DPP_MONTO = :mon WHERE PRO_CODIGO = :pro AND PDD_CODIGO = :pdd";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.Parameters.Add(new OracleParameter("can", dp.DppCantidad));
                    cmd.Parameters.Add(new OracleParameter("mon", dp.DppMonto));
                    cmd.Parameters.Add(new OracleParameter("pro", dp.ProCodigo));
                    cmd.Parameters.Add(new OracleParameter("pdd", dp.PddCodigo));
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { throw new Exception($"{OracleDB.GetConfig("error.general")} (ActualizarMD): {ex.Message}"); }
            }
        }

        public int EliminarMD(string pro, string pdd)
        {
            string sql = "DELETE FROM DETALLE_PEDIDO WHERE PRO_CODIGO = :pro AND PDD_CODIGO = :pdd";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.Parameters.Add(new OracleParameter("pro", pro));
                    cmd.Parameters.Add(new OracleParameter("pdd", pdd));
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { throw new Exception($"{OracleDB.GetConfig("error.general")} (EliminarMD): {ex.Message}"); }
            }
        }
    }
}