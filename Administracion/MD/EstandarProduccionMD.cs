using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string query = "SELECT * FROM ESTANDAR_PRODUCCION";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(query, conn);
                try
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new EstandarProduccionDP
                        {
                            MtpCodigo = reader["MTP_CODIGO"].ToString(),
                            ProCodigo = reader["PRO_CODIGO"].ToString(),
                            EdpDescripcion = reader["EDP_DESCRIPCION"].ToString(),
                            EdpCantidad = Convert.ToDouble(reader["EDP_CANTIDAD"])
                        });
                    }
                }
                catch (Exception ex) { throw new Exception("Error MD: " + ex.Message); }
            }
            return lista;
        }
        public EstandarProduccionDP ConsultarByCodMD(string mtpCodigo, string proCodigo)
        {
            EstandarProduccionDP estandar = null;
            string sql = "SELECT MTP_Codigo, PRO_Codigo, EDP_Descripcion, EDP_Cantidad " +
             "FROM ESTANDAR_PRODUCCION " +
             "WHERE MTP_Codigo = :mtp AND PRO_Codigo = :pro";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":mtp", mtpCodigo);
                    cmd.Parameters.Add(":pro", proCodigo);

                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            estandar = new EstandarProduccionDP
                            {
                                MtpCodigo = dr.GetString(0),
                                ProCodigo = dr.GetString(1),
                                EdpDescripcion = dr.IsDBNull(2) ? "" : dr.GetString(2),
                                EdpCantidad = dr.GetDouble(3)
                            };
                        }
                    }
                }
            }
            return estandar;
        }

        public int IngresarMD(EstandarProduccionDP dp)
        {
            string sql = "INSERT INTO ESTANDAR_PRODUCCION (MTP_CODIGO, PRO_CODIGO, EDP_DESCRIPCION, EDP_CANTIDAD) VALUES (:mtp, :pro, :des, :can)";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("mtp", dp.MtpCodigo));
                cmd.Parameters.Add(new OracleParameter("pro", dp.ProCodigo));
                cmd.Parameters.Add(new OracleParameter("des", dp.EdpDescripcion));
                cmd.Parameters.Add(new OracleParameter("can", dp.EdpCantidad));
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int ActualizarMD(EstandarProduccionDP dp)
        {
            string sql = "UPDATE ESTANDAR_PRODUCCION SET EDP_DESCRIPCION = :des, EDP_CANTIDAD = :can WHERE MTP_CODIGO = :mtp AND PRO_CODIGO = :pro";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("des", dp.EdpDescripcion));
                cmd.Parameters.Add(new OracleParameter("can", dp.EdpCantidad));
                cmd.Parameters.Add(new OracleParameter("mtp", dp.MtpCodigo));
                cmd.Parameters.Add(new OracleParameter("pro", dp.ProCodigo));
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int EliminarMD(string mtp, string pro)
        {
            string sql = "DELETE FROM ESTANDAR_PRODUCCION WHERE MTP_CODIGO = :mtp AND PRO_CODIGO = :pro";
            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("mtp", mtp));
                cmd.Parameters.Add(new OracleParameter("pro", pro));
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
