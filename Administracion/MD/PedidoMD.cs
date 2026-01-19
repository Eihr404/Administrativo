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
    internal class PedidoMD
    {
        public bool IngresarMD(PedidoDP p)
        {
            const string sql = @"
        INSERT INTO PEDIDO 
        (PDD_CODIGO, CLI_CEDULA, PDD_COMENTARIO, PDD_ESTADO, PDD_UBICACION, PDD_MONTO_TOTAL, PDD_ABONO)
        VALUES 
        (:codigo, :cedula, :comentario, :estado, :ubicacion, :total, :abono)";

            try
            {
                using OracleConnection conn = OracleDB.CrearConexion();
                conn.Open();
                using OracleCommand cmd = new OracleCommand(sql, conn);

                // Mapeo con las propiedades de tu DP
                cmd.Parameters.Add(":codigo", p.PedCodigo);
                cmd.Parameters.Add(":cedula", p.CliCedula);
                cmd.Parameters.Add(":comentario", p.PedComentario ?? (object)DBNull.Value);
                cmd.Parameters.Add(":estado", p.PedEstado);
                cmd.Parameters.Add(":ubicacion", p.PedUbicacion ?? (object)DBNull.Value);
                cmd.Parameters.Add(":total", p.PedTotal);
                cmd.Parameters.Add(":abono", p.PedAbono);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el pedido en la base de datos.", ex);
            }
        }

        public bool ModificarMD(PedidoDP p)
        {
            const string sql = @"
        UPDATE PEDIDO SET
            CLI_CEDULA = :cedula,
            PDD_COMENTARIO = :comentario,
            PDD_ESTADO = :estado,
            PDD_UBICACION = :ubicacion,
            PDD_MONTO_TOTAL = :total,
            PDD_ABONO = :abono
        WHERE PDD_CODIGO = :codigo";

            try
            {
                using OracleConnection conn = OracleDB.CrearConexion();
                conn.Open();
                using OracleCommand cmd = new OracleCommand(sql, conn);

                cmd.Parameters.Add(":cedula", p.CliCedula);
                cmd.Parameters.Add(":comentario", p.PedComentario);
                cmd.Parameters.Add(":estado", p.PedEstado);
                cmd.Parameters.Add(":ubicacion", p.PedUbicacion);
                cmd.Parameters.Add(":total", p.PedTotal);
                cmd.Parameters.Add(":abono", p.PedAbono);
                cmd.Parameters.Add(":codigo", p.PedCodigo); // El WHERE siempre va al final en el orden de parámetros de Oracle

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el pedido.", ex);
            }
        }

        public bool EliminarMD(string codigo)
        {
            string sql = "DELETE FROM PEDIDO WHERE PDD_CODIGO = :codigo";

            try
            {
                using OracleConnection conn = OracleDB.CrearConexion();
                conn.Open();

                using OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":codigo", codigo);

                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"{OracleDB.GetConfig("error.general")} (EliminarMD): {ex.Message}");
            }
        }

        public List<PedidoDP> ConsultarByCodMD(string codigo)
        {
            List<PedidoDP> pedidos = new List<PedidoDP>();

            // Ajustamos el SQL a la tabla de pedidos y las columnas PDD_
            string sql = @"
        SELECT 
            PDD_CODIGO, 
            CLI_CEDULA, 
            PDD_COMENTARIO, 
            PDD_ESTADO, 
            PDD_UBICACION, 
            PDD_MONTO_TOTAL, 
            PDD_ABONO
        FROM PEDIDO
        WHERE PDD_CODIGO = :codigo";

            try
            {
                using OracleConnection conn = OracleDB.CrearConexion();
                conn.Open();

                using OracleCommand cmd = new OracleCommand(sql, conn);
                // Es buena práctica usar OracleParameter para definir el tipo si es posible
                cmd.Parameters.Add(new OracleParameter("codigo", codigo));

                using OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    // Usamos el nuevo método MapearPedido que corregimos anteriormente
                    pedidos.Add(MapearPedido(dr));
                }
            }
            catch (Exception ex)
            {
                // Usamos la configuración de error general o un mensaje personalizado
                string errorPrefix = OracleDB.GetConfig("error.general") ?? "Error en base de datos";
                throw new Exception($"{errorPrefix} (ConsultarByCodMD Pedido): {ex.Message}");
            }

            return pedidos;
        }
        public List<PedidoDP> ConsultarAllMD()
        {
            List<PedidoDP> pedidos = new();
            const string sql = @"
        SELECT PDD_CODIGO, CLI_CEDULA, PDD_COMENTARIO, PDD_ESTADO, PDD_UBICACION, PDD_MONTO_TOTAL, PDD_ABONO 
        FROM PEDIDO";

            try
            {
                using var conn = OracleDB.CrearConexion();
                conn.Open();
                using var cmd = new OracleCommand(sql, conn);
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    pedidos.Add(new PedidoDP
                    {
                        // ÍNDICES: 0=CODIGO, 1=CEDULA, 2=COMENTARIO, 3=ESTADO, 4=UBICACION, 5=TOTAL, 6=ABONO
                        PedCodigo = dr.GetString(0),
                        CliCedula = dr.GetString(1),
                        PedComentario = dr.IsDBNull(2) ? "" : dr.GetString(2),
                        PedEstado = dr.IsDBNull(3) ? "" : dr.GetString(3),
                        PedUbicacion = dr.IsDBNull(4) ? "" : dr.GetString(4),
                        PedTotal = dr.IsDBNull(5) ? 0 : Convert.ToDouble(dr.GetDecimal(5)),
                        PedAbono = dr.IsDBNull(6) ? 0 : Convert.ToDouble(dr.GetDecimal(6))
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los pedidos.", ex);
            }
            return pedidos;
        }

        private PedidoDP MapearPedido(OracleDataReader dr)
        {
            return new PedidoDP
            {
                // El orden depende del SELECT: 
                // 0:PDD_CODIGO, 1:CLI_CEDULA, 2:PDD_COMENTARIO, 3:PDD_ESTADO, 4:PDD_UBICACION, 5:PDD_MONTO_TOTAL, 6:PDD_ABONO

                PedCodigo = dr.IsDBNull(0) ? "" : dr.GetString(0),
                CliCedula = dr.IsDBNull(1) ? "" : dr.GetString(1),
                PedComentario = dr.IsDBNull(2) ? "" : dr.GetString(2),
                PedEstado = dr.IsDBNull(3) ? "" : dr.GetString(3),
                PedUbicacion = dr.IsDBNull(4) ? "" : dr.GetString(4),

                // Para campos NUMBER de Oracle, es más seguro usar GetDouble o Convert.ToDouble
                PedTotal = dr.IsDBNull(5) ? 0.0 : Convert.ToDouble(dr.GetValue(5)),
                PedAbono = dr.IsDBNull(6) ? 0.0 : Convert.ToDouble(dr.GetValue(6))
            };
        }



        public bool VerificarMD(string codigo)
        {
            string sql = "SELECT COUNT(*) FROM PRODUCTO WHERE PRO_Codigo = :codigo";
            try
            {
                using OracleConnection conn = OracleDB.CrearConexion();
                conn.Open();
                using OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":codigo", codigo);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"{OracleDB.GetConfig("error.general")} (VerificarMD): {ex.Message}");
            }
        }
    }
}
