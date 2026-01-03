using Administracion.Datos;
using Administracion.DP;
using Oracle.ManagedDataAccess.Client; // Asegúrate de tener la referencia
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
            string query = "SELECT * FROM MATERIA_PRIMA";

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
                            MtpNombre = reader["MTP_NOMBRE"].ToString(),
                            MtpDescripcion = reader["MTP_DESCRIPCION"].ToString(),
                            MtpPrecioCompraAnt = Convert.ToDouble(reader["MTP_PRECIO_COMPRA_ANT"]),
                            MtpPrecioCompra = Convert.ToDouble(reader["MTP_PRECIO_COMPRA"])
                        });
                    }
                }
                catch (Exception ex) { throw new Exception("Error en ConsultarAllMD: " + ex.Message); }
            }
            return lista;
        }
        /* Método para consulta por parámetro (código) */
        public List<MateriaPrimaDP> ConsultarByCodMD(string codigo)
        {
            List<MateriaPrimaDP> lista = new List<MateriaPrimaDP>();
            string query = "SELECT * FROM MATERIA_PRIMA WHERE MTP_CODIGO = :cod";

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
                            MtpNombre = reader["MTP_NOMBRE"].ToString(),
                            MtpDescripcion = reader["MTP_DESCRIPCION"].ToString(),
                            MtpPrecioCompraAnt = Convert.ToDouble(reader["MTP_PRECIO_COMPRA_ANT"]),
                            MtpPrecioCompra = Convert.ToDouble(reader["MTP_PRECIO_COMPRA"])
                        });
                    }
                }
                catch (Exception ex) { throw new Exception("Error en ConsultarPorCodigoMD: " + ex.Message); }
            }
            return lista;
        }

        /* Método para ingresar nueva materia prima */
        public int IngresarMD(MateriaPrimaDP dp)
        {
            string sql = "INSERT INTO MATERIA_PRIMA (MTP_CODIGO, UME_CODIGO, MTP_NOMBRE, MTP_DESCRIPCION, MTP_PRECIO_COMPRA_ANT, MTP_PRECIO_COMPRA) " +
                 "VALUES (:cod, :uni, :nom, :des, :pant, :pact)";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("cod", dp.MtpCodigo));
                cmd.Parameters.Add(new OracleParameter("uni", dp.UmeCodigo));
                cmd.Parameters.Add(new OracleParameter("nom", dp.MtpNombre));
                cmd.Parameters.Add(new OracleParameter("des", dp.MtpDescripcion));
                cmd.Parameters.Add(new OracleParameter("pant", dp.MtpPrecioCompraAnt));
                cmd.Parameters.Add(new OracleParameter("pact", dp.MtpPrecioCompra));

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
        /* Método para actualizar materia prima */
        public int ActualizarMD(MateriaPrimaDP dp)
        {
            /* Actualiza los datos de una materia prima existente */
            string sql = "UPDATE MATERIA_PRIMA SET UME_CODIGO = :uni, MTP_NOMBRE = :nom, " +
                         "MTP_DESCRIPCION = :des, MTP_PRECIO_COMPRA_ANT = :pant, " +
                         "MTP_PRECIO_COMPRA = :pact WHERE MTP_CODIGO = :cod";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("uni", dp.UmeCodigo));
                cmd.Parameters.Add(new OracleParameter("nom", dp.MtpNombre));
                cmd.Parameters.Add(new OracleParameter("des", dp.MtpDescripcion));
                cmd.Parameters.Add(new OracleParameter("pant", dp.MtpPrecioCompraAnt));
                cmd.Parameters.Add(new OracleParameter("pact", dp.MtpPrecioCompra));
                cmd.Parameters.Add(new OracleParameter("cod", dp.MtpCodigo));

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        /* Método para eliminar materia prima */
        public int EliminarMD(string codigo)
        {
            string sql = "DELETE FROM MATERIA_PRIMA WHERE MTP_CODIGO = :cod";

            using (OracleConnection conn = OracleDB.CrearConexion())
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("cod", codigo));
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
