using Administracion.Datos;
using Administracion.DP;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Administracion.MD
{
    public class MateriaPrimaMD
    {
        /* Método par colsulta general */
        public List<MateriaPrimaDP> ConsultarAllMD()
        {
            List<MateriaPrimaDP> lista = new List<MateriaPrimaDP>();
            string query = @"
                    SELECT 
                        MTP_CODIGO,
                        UME_CODIGO,
                        BOD_CODIGO,
                        MTP_NOMBRE,
                        MTP_DESCRIPCION,
                        MTP_PRECIO__COMPRA_ANT,
                        MTP_PRECIO_COMPRA,
                        MTP_EXISTENCIA
                    FROM MATERIA_PRIMA";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(query, conn);
                try
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new MateriaPrimaDP
                        {
                            MtpCodigo = reader["MTP_CODIGO"].ToString(),
                            UmeCodigo = reader["UME_CODIGO"].ToString(),
                            BodCodigo = reader["BOD_CODIGO"].ToString(),

                            MtpNombre = reader["MTP_NOMBRE"]?.ToString() ?? "",
                            MtpDescripcion = reader["MTP_DESCRIPCION"]?.ToString() ?? "",

                            MtpPrecioCompraAnt = reader.IsDBNull(reader.GetOrdinal("MTP_PRECIO__COMPRA_ANT")) ? 0 : Convert.ToDouble(reader["MTP_PRECIO__COMPRA_ANT"]),

                            MtpPrecioCompra = reader.IsDBNull(reader.GetOrdinal("MTP_PRECIO_COMPRA")) ? 0: Convert.ToDouble(reader["MTP_PRECIO_COMPRA"]),

                            MtpExistencia = reader.IsDBNull(reader.GetOrdinal("MTP_EXISTENCIA"))? 0 : Convert.ToInt32(reader["MTP_EXISTENCIA"])});
                    }
                }
                catch (Exception ex)
                {
                    // error.general
                    throw new Exception($"{OracleDB.GetConfig("error.general")} (ConsultarAllMD): {ex.Message}");
                }
            }
            return lista;
        }

        /* Método para consulta por parámetro (código) */
        public List<MateriaPrimaDP> ConsultarByCodMD(string codigo)
        {
            List<MateriaPrimaDP> lista = new List<MateriaPrimaDP>();
            string query = "SELECT * FROM MATERIA_PRIMA WHERE MTP_CODIGO = :codigo";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter("cod", codigo));
                try
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new MateriaPrimaDP
                        {
                            MtpCodigo = reader["MTP_CODIGO"].ToString(),
                            UmeCodigo = reader["UME_CODIGO"].ToString(),
                            BodCodigo = reader["BOD_CODIGO"].ToString(),

                            MtpNombre = reader["MTP_NOMBRE"]?.ToString() ?? "",
                            MtpDescripcion = reader["MTP_DESCRIPCION"]?.ToString() ?? "",

                            MtpPrecioCompraAnt = reader.IsDBNull(reader.GetOrdinal("MTP_PRECIO__COMPRA_ANT")) ? 0 : Convert.ToDouble(reader["MTP_PRECIO__COMPRA_ANT"]),

                            MtpPrecioCompra = reader.IsDBNull(reader.GetOrdinal("MTP_PRECIO_COMPRA")) ? 0 : Convert.ToDouble(reader["MTP_PRECIO_COMPRA"]),

                            MtpExistencia = reader.IsDBNull(reader.GetOrdinal("MTP_EXISTENCIA")) ? 0 : Convert.ToInt32(reader["MTP_EXISTENCIA"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    // error.general
                    throw new Exception($"{OracleDB.GetConfig("error.general")} (ConsultarByCodMD): {ex.Message}");
                }
            }
            return lista;
        }

        /* Método para ingresar nueva materia prima */
        public int IngresarMD(MateriaPrimaDP dp)
        {
            string sql = @"
                    INSERT INTO MATERIA_PRIMA
                    (
                        MTP_CODIGO,
                        UME_CODIGO,
                        BOD_CODIGO,
                        MTP_NOMBRE,
                        MTP_DESCRIPCION,
                        MTP_PRECIO__COMPRA_ANT,
                        MTP_PRECIO_COMPRA,
                        MTP_EXISTENCIA
                    )
                    VALUES
                    (
                        :cod,
                        :ume,
                        :bod,
                        :nom,
                        :des,
                        :pant,
                        :pact,
                        :exi
                    )";

            using OracleConnection conn = OracleDB.CrearConexion();
            try
            {
                using OracleCommand cmd = new OracleCommand(sql, conn);

                cmd.Parameters.Add(":cod", dp.MtpCodigo);
                cmd.Parameters.Add(":ume", dp.UmeCodigo);
                cmd.Parameters.Add(":bod", dp.BodCodigo);
                cmd.Parameters.Add(":nom", dp.MtpNombre);
                cmd.Parameters.Add(":des", dp.MtpDescripcion);
                cmd.Parameters.Add(":pant", dp.MtpPrecioCompraAnt);
                cmd.Parameters.Add(":pact", dp.MtpPrecioCompra);
                cmd.Parameters.Add(":exi", dp.MtpExistencia);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"{OracleDB.GetConfig("error.general")} (IngresarMD): {ex.Message}");
            }
        }


        /* Método para actualizar materia prima */
        public int ActualizarMD(MateriaPrimaDP dp)
        {
            string sql = @"
                    UPDATE MATERIA_PRIMA
                    SET
                        UME_CODIGO = :ume,
                        BOD_CODIGO = :bod,
                        MTP_NOMBRE = :nom,
                        MTP_DESCRIPCION = :des,
                        MTP_PRECIO__COMPRA_ANT = :pant,
                        MTP_PRECIO_COMPRA = :pact,
                        MTP_EXISTENCIA = :exi
                    WHERE MTP_CODIGO = :cod";

            using OracleConnection conn = OracleDB.CrearConexion();
            try
            {
                using OracleCommand cmd = new OracleCommand(sql, conn);

                cmd.Parameters.Add(":ume", dp.UmeCodigo);
                cmd.Parameters.Add(":bod", dp.BodCodigo);
                cmd.Parameters.Add(":nom", dp.MtpNombre);
                cmd.Parameters.Add(":des", dp.MtpDescripcion);
                cmd.Parameters.Add(":pant", dp.MtpPrecioCompraAnt);
                cmd.Parameters.Add(":pact", dp.MtpPrecioCompra);
                cmd.Parameters.Add(":exi", dp.MtpExistencia);
                cmd.Parameters.Add(":cod", dp.MtpCodigo);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"{OracleDB.GetConfig("error.general")} (ActualizarMD): {ex.Message}");
            }
        }


        /* Método para eliminar materia prima */
        public int EliminarMD(string codigo)
        {
            string sql = "DELETE FROM MATERIA_PRIMA WHERE MTP_CODIGO = :cod";

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
                    // error.general
                    throw new Exception($"{OracleDB.GetConfig("error.general")} (EliminarMD): {ex.Message}");
                }
            }
        }
    }
}