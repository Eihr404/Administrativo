using Administracion.Datos;
using Administracion.DP;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Administracion.MD
{
    internal class ProductoMD
    {
        public ProductoMD() { }

        /* Ingresa un nuevo producto a la base de datos */
        public bool IngresarMD(ProductoDP p)
        {
            string sql = @"
                INSERT INTO PRODUCTO
                (
                    PRO_CODIGO,
                    CAT_CODIGO,
                    CLA_CODIGO,
                    BOD_CODIGO,
                    PRO_NOMBRE,
                    PRO_DESCRIPCION,
                    PRO_EXISTENCIA,
                    PRO_PRECIO_VENTA,
                    PRO_PRECIO_VENTA_ANT,
                    PRO_UTILIDAD,
                    PRO_IMAGEN,
                    PRO_ALT_IMAGEN
                )
                VALUES
                (
                    :codigo,
                    :catCodigo,
                    :claCodigo,
                    :bodCodigo,
                    :nombre,
                    :descripcion,
                    :existencia,
                    :precio,
                    :precioAnt,
                    :utilidad,
                    :imagen,
                    :alt
                )";

            try
            {
                using OracleConnection conn = OracleDB.CrearConexion();
                conn.Open();

                using OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":codigo", p.Codigo);
                cmd.Parameters.Add(":catCodigo", p.CategoriaCodigo);
                cmd.Parameters.Add(":claCodigo", p.ClasificacionCodigo);
                cmd.Parameters.Add(":bodCodigo", p.BodegaCodigo);   // OBLIGATORIO
                cmd.Parameters.Add(":nombre", p.Nombre);
                cmd.Parameters.Add(":descripcion", p.Descripcion);
                cmd.Parameters.Add(":existencia", p.Existencia);
                cmd.Parameters.Add(":precio", p.PrecioVenta);
                cmd.Parameters.Add(":precioAnt", p.PrecioVentaAnt);
                cmd.Parameters.Add(":utilidad", p.Utilidad);
                cmd.Parameters.Add(":imagen", p.Imagen);
                cmd.Parameters.Add(":alt", p.AltTextImagen);


                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"{OracleDB.GetConfig("error.general")} (IngresarMD): {ex.Message}");
            }
        }

        /* Modifica un producto en la base de datos */
        public bool ModificarMD(ProductoDP p)
        {
            string sql = @"
                UPDATE PRODUCTO SET
                    CAT_Codigo = :catCodigo,
                    CLA_Codigo = :claCodigo,
                    UME_Codigo = :umeCodigo,
                    PRO_Nombre = :nombre,
                    PRO_Descripcion = :descripcion,
                    PRO_Precio_venta = :precio,
                    PRO_Precio_venta_ant = :precioAnt,
                    PRO_Utilidad = :utilidad,
                    PRO_Imagen = :imagen,
                    PRO_Alt_Imagen = :alt
                WHERE PRO_Codigo = :codigo";

            try
            {
                using OracleConnection conn = OracleDB.CrearConexion();
                conn.Open();

                using OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":catCodigo", p.CategoriaCodigo);
                cmd.Parameters.Add(":claCodigo", p.ClasificacionCodigo);
                cmd.Parameters.Add(":nombre", p.Nombre);
                cmd.Parameters.Add(":descripcion", p.Descripcion);
                cmd.Parameters.Add(":precio", p.PrecioVenta);
                cmd.Parameters.Add(":precioAnt", p.PrecioVentaAnt);
                cmd.Parameters.Add(":utilidad", p.Utilidad);
                cmd.Parameters.Add(":imagen", p.Imagen);
                cmd.Parameters.Add(":alt", p.AltTextImagen);
                cmd.Parameters.Add(":codigo", p.Codigo);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"{OracleDB.GetConfig("error.general")} (ModificarMD): {ex.Message}");
            }
        }

        /* Elimina un producto de la base de datos */
        public bool EliminarMD(string codigo)
        {
            string sql = "DELETE FROM PRODUCTO WHERE PRO_Codigo = :codigo";

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

        /* Consulta un producto por su código */
        public List<ProductoDP> ConsultarByCodMD(string codigo)
        {
            List<ProductoDP> producto = new List<ProductoDP>();
            string sql = @"
                SELECT 
                    p.PRO_CODIGO,
                    c.CAT_CODIGO,
                    c.CAT_DESCRIPCION,
                    cl.CLA_CODIGO,
                    cl.CLA_NOMBRE,
                    p.PRO_NOMBRE,
                    p.PRO_DESCRIPCION,
                    p.PRO_PRECIO_VENTA,
                    p.PRO_PRECIO_VENTA_ANT,
                    p.PRO_UTILIDAD,
                    p.PRO_IMAGEN,
                    p.PRO_ALT_IMAGEN
                FROM PRODUCTO p
                JOIN CATEGORIA c 
                    ON c.CAT_CODIGO = p.CAT_CODIGO
                JOIN CLASIFICACION cl 
                    ON cl.CLA_CODIGO = p.CLA_CODIGO
                WHERE p.PRO_Codigo = :codigo";

            try
            {
                using OracleConnection conn = OracleDB.CrearConexion();
                conn.Open();
                using OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":codigo", codigo);

                using OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    producto.Add(MapearProducto(dr));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{OracleDB.GetConfig("error.general")} (ConsultarByCodMD): {ex.Message}");
            }
            return producto;
        }

        /* Consulta todos los productos */
        public List<ProductoDP> ConsultarAllMD()
        {
            List<ProductoDP> productos = new();
            string sql = @"
                SELECT 
                    p.PRO_CODIGO,
                    c.CAT_CODIGO,
                    c.CAT_DESCRIPCION,
                    cl.CLA_CODIGO,
                    cl.CLA_NOMBRE,
                    p.PRO_NOMBRE,
                    p.PRO_DESCRIPCION,
                    p.PRO_PRECIO_VENTA,
                    p.PRO_PRECIO_VENTA_ANT,
                    p.PRO_UTILIDAD,
                    p.PRO_IMAGEN,
                    p.PRO_ALT_IMAGEN
                FROM PRODUCTO p
                JOIN CATEGORIA c 
                    ON c.CAT_CODIGO = p.CAT_CODIGO
                JOIN CLASIFICACION cl 
                    ON cl.CLA_CODIGO = p.CLA_CODIGO
                ";

            try
            {
                using OracleConnection conn = OracleDB.CrearConexion();
                conn.Open();
                using OracleCommand cmd = new OracleCommand(sql, conn);
                using OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    productos.Add(MapearProducto(dr));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{OracleDB.GetConfig("error.general")} (ConsultarAllMD): {ex.Message}");
            }
            return productos;
        }

        /* Método privado para reutilizar el mapeo de datos */
        private ProductoDP MapearProducto(OracleDataReader dr)
        {
            return new ProductoDP
            {
                Codigo = dr.GetString(0),
                CategoriaCodigo = dr.GetString(1),
                CategoriaDescripcion = dr.GetString(2),
                ClasificacionCodigo = dr.GetString(3),
                ClasificacionDescripcion = dr.GetString(4),

                Nombre = dr.GetString(5),
                Descripcion = dr.IsDBNull(6) ? "" : dr.GetString(6),

                PrecioVenta = dr.GetDouble(7),
                PrecioVentaAnt = dr.IsDBNull(8) ? 0 : dr.GetDouble(8),
                Utilidad = dr.GetDouble(9),

                Imagen = dr.IsDBNull(10) ? "" : dr.GetString(10),
                AltTextImagen = dr.IsDBNull(11) ? "" : dr.GetString(11)
            };
        }


        /* Verifica si el producto existe */
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