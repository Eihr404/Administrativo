using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using Administracion.Datos;
using Administracion.DP;

namespace Administracion.MD
{
    public class DetalleNotaVentaMD
    {
        /* Consulta General */
        public List<DetalleNotaVentaDP> ConsultarAllMD()
        {
            List<DetalleNotaVentaDP> lista = new List<DetalleNotaVentaDP>();
            string query = @"SELECT NDV_NUMERO, PRO_CODIGO, DNV_CANTIDAD, DNV_MONTO FROM DETALLE_NOTA_VENTA";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(query, conn);
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new DetalleNotaVentaDP
                        {
                            NdvNumero = reader["NDV_NUMERO"].ToString(),
                            ProCodigo = reader["PRO_CODIGO"].ToString(),
                            DnvCantidad = Convert.ToInt32(reader["DNV_CANTIDAD"]),
                            DnvMonto = Convert.ToDouble(reader["DNV_MONTO"])
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

        /* Consulta por Criterio (Número de Nota o Código de Producto) */
        public List<DetalleNotaVentaDP> ConsultarByCodMD(string criterio)
        {
            List<DetalleNotaVentaDP> lista = new List<DetalleNotaVentaDP>();
            string sql = "SELECT NDV_NUMERO, PRO_CODIGO, DNV_CANTIDAD, DNV_MONTO " +
                         "FROM DETALLE_NOTA_VENTA " +
                         "WHERE NDV_NUMERO LIKE :criterio " +
                         "OR PRO_CODIGO LIKE :criterio";

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
                                lista.Add(new DetalleNotaVentaDP
                                {
                                    NdvNumero = dr.GetString(0),
                                    ProCodigo = dr.GetString(1),
                                    DnvCantidad = dr.GetInt32(2),
                                    DnvMonto = dr.GetDouble(3)
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

        /* Ingresar Detalle */
        public int IngresarMD(DetalleNotaVentaDP dp)
        {
            string sql = "INSERT INTO DETALLE_NOTA_VENTA (NDV_NUMERO, PRO_CODIGO, DNV_CANTIDAD, DNV_MONTO) VALUES (:ndv, :pro, :can, :mon)";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.Parameters.Add(new OracleParameter("ndv", dp.NdvNumero));
                    cmd.Parameters.Add(new OracleParameter("pro", dp.ProCodigo));
                    cmd.Parameters.Add(new OracleParameter("can", dp.DnvCantidad));
                    cmd.Parameters.Add(new OracleParameter("mon", dp.DnvMonto));
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"{OracleDB.GetConfig("error.general")} (IngresarMD): {ex.Message}");
                }
            }
        }

        /* Actualizar Detalle (Basado en la llave compuesta) */
        public int ActualizarMD(DetalleNotaVentaDP dp)
        {
            string sql = "UPDATE DETALLE_NOTA_VENTA SET DNV_CANTIDAD = :can, DNV_MONTO = :mon WHERE NDV_NUMERO = :ndv AND PRO_CODIGO = :pro";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.Parameters.Add(new OracleParameter("can", dp.DnvCantidad));
                    cmd.Parameters.Add(new OracleParameter("mon", dp.DnvMonto));
                    cmd.Parameters.Add(new OracleParameter("ndv", dp.NdvNumero));
                    cmd.Parameters.Add(new OracleParameter("pro", dp.ProCodigo));
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"{OracleDB.GetConfig("error.general")} (ActualizarMD): {ex.Message}");
                }
            }
        }

        /* Eliminar Detalle (Requiere ambos códigos por ser tabla dependiente) */
        public int EliminarMD(string ndv, string pro)
        {
            string sql = "DELETE FROM DETALLE_NOTA_VENTA WHERE NDV_NUMERO = :ndv AND PRO_CODIGO = :pro";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.Parameters.Add(new OracleParameter("ndv", ndv));
                    cmd.Parameters.Add(new OracleParameter("pro", pro));
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