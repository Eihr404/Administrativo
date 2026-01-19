using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using Administracion.Datos;
using Administracion.DP;

namespace Administracion.MD
{
    public class EstandarProduccionMD
    {
        public List<EstandarProduccionDP> ConsultarAllMD()
        {
            List<EstandarProduccionDP> lista = new List<EstandarProduccionDP>();
            string query = @"SELECT * FROM ESTANDAR_PRODUCCION";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(query, conn);
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new EstandarProduccionDP
                        {
                            MtpCodigo = reader["MTP_CODIGO"].ToString(),
                            ProCodigo = reader["PRO_CODIGO"].ToString(),
                            EdpCantidad = Convert.ToDouble(reader["EDP_CANTIDAD"])
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

        public List<EstandarProduccionDP> ConsultarByCodMD(string criterio)
        {
            List<EstandarProduccionDP> lista = new List<EstandarProduccionDP>();
            string sql = "SELECT MTP_Codigo, PRO_Codigo, EDP_Cantidad " +
                         "FROM ESTANDAR_PRODUCCION " +
                         "WHERE MTP_Codigo LIKE :criterio " +
                         "OR PRO_Codigo LIKE :criterio";

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
                                lista.Add(new EstandarProduccionDP
                                {
                                    MtpCodigo = dr.GetString(0),
                                    ProCodigo = dr.GetString(1),
                                    EdpCantidad = dr.GetDouble(3)
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

        public int IngresarMD(EstandarProduccionDP dp)
        {
            string sql = "INSERT INTO ESTANDAR_PRODUCCION (MTP_CODIGO, PRO_CODIGO, EDP_CANTIDAD) VALUES (:mtp, :pro, :can)";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.Parameters.Add(new OracleParameter("mtp", dp.MtpCodigo));
                    cmd.Parameters.Add(new OracleParameter("pro", dp.ProCodigo));
                    cmd.Parameters.Add(new OracleParameter("can", dp.EdpCantidad));
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"{OracleDB.GetConfig("error.general")} (IngresarMD): {ex.Message}");
                }
            }
        }

        public int ActualizarMD(EstandarProduccionDP dp)
        {
            string sql = "UPDATE ESTANDAR_PRODUCCION SET EDP_CANTIDAD = :can WHERE MTP_CODIGO = :mtp AND PRO_CODIGO = :pro";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.Parameters.Add(new OracleParameter("can", dp.EdpCantidad));
                    cmd.Parameters.Add(new OracleParameter("mtp", dp.MtpCodigo));
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

        public int EliminarMD(string mtp, string pro)
        {
            string sql = "DELETE FROM ESTANDAR_PRODUCCION WHERE MTP_CODIGO = :mtp AND PRO_CODIGO = :pro";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.Parameters.Add(new OracleParameter("mtp", mtp));
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