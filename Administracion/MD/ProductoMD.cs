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
    internal class ProductoMD
    {
        public ProductoMD() { }

        /* Ingresa un nuevo producto a la base de datos */
        public bool IngresarMD(ProductoDP p)
        {
            string sql = @"
                INSERT INTO PRODUCTO
                (PRO_Codigo, CAT_Codigo, CLA_Codigo, UME_Codigo,
                 PRO_Nombre, PRO_Descripcion,
                 PRO_Precio_venta, PRO_Precio_venta_ant,
                 PRO_Utilidad, PRO_Imagen, PRO_Alt_Imagen)
                VALUES
                (:codigo, :catCodigo, :claCodigo, :umeCodigo,
                 :nombre, :descripcion,
                 :precio, :precioAnt,
                 :utilidad, :imagen, :alt)";

            using OracleConnection conn = OracleDB.CrearConexion();
            conn.Open();

            using OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(":codigo", p.Codigo);

            cmd.Parameters.Add(":catCodigo", p.CategoriaCodigo);
            cmd.Parameters.Add(":claCodigo", p.ClasificacionCodigo);
            cmd.Parameters.Add(":umeCodigo", p.UnidadMedidaCodigo);

            cmd.Parameters.Add(":nombre", p.Nombre);
            cmd.Parameters.Add(":descripcion", p.Descripcion);
            cmd.Parameters.Add(":precio", p.PrecioVenta);
            cmd.Parameters.Add(":precioAnt", p.PrecioVentaAnt);
            cmd.Parameters.Add(":utilidad", p.Utilidad);
            cmd.Parameters.Add(":imagen", p.Imagen);
            cmd.Parameters.Add(":alt", p.AltTextImagen);

            return cmd.ExecuteNonQuery() > 0;
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

            using OracleConnection conn = OracleDB.CrearConexion();
            conn.Open();

            using OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(":catCodigo", p.CategoriaCodigo);
            cmd.Parameters.Add(":claCodigo", p.ClasificacionCodigo);
            cmd.Parameters.Add(":umeCodigo", p.UnidadMedidaCodigo);

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

        /* Elimina un producto de la base de datos */
        public bool EliminarMD(string codigo)
        {
            string sql = "DELETE FROM PRODUCTO WHERE PRO_Codigo = :codigo";

            using OracleConnection conn = OracleDB.CrearConexion();
            conn.Open();

            using OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(":codigo", codigo);

            int filas = cmd.ExecuteNonQuery();
            conn.Close();

            return filas > 0;
        }

        /* Consulta un producto por su código */
        public ProductoDP ConsultarByCodMD(string codigo)
        {
            ProductoDP producto = null;

            string sql = @"
                SELECT 
                    p.PRO_Codigo,
                    c.CAT_Codigo,
                    c.CAT_Descripcion,
                    cl.CLA_Codigo,
                    cl.CLA_Nombre,
                    u.UME_Codigo,
                    u.UME_Descripcion,
                    p.PRO_Nombre,
                    p.PRO_Descripcion,
                    p.PRO_Precio_venta,
                    p.PRO_Precio_venta_ant,
                    p.PRO_Utilidad,
                    p.PRO_Imagen,
                    p.PRO_Alt_Imagen
                FROM PRODUCTO p
                JOIN CATEGORIA c ON c.CAT_Codigo = p.CAT_Codigo
                JOIN CLASIFICACION cl ON cl.CLA_Codigo = p.CLA_Codigo
                JOIN UNIDAD_MEDIDA u ON u.UME_Codigo = p.UME_Codigo
                WHERE p.PRO_Codigo = :codigo";

            using OracleConnection conn = OracleDB.CrearConexion();
            conn.Open();

            using OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(":codigo", codigo);

            using OracleDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                producto = new ProductoDP
                {
                    Codigo = dr.GetString(0),

                    CategoriaCodigo = dr.GetString(1),
                    CategoriaDescripcion = dr.GetString(2),

                    ClasificacionCodigo = dr.GetString(3),
                    ClasificacionDescripcion = dr.GetString(4),

                    UnidadMedidaCodigo = dr.GetString(5),
                    UnidadMedidaDescripcion = dr.GetString(6),

                    Nombre = dr.GetString(7),
                    Descripcion = dr.GetString(8),
                    PrecioVenta = dr.GetDouble(9),
                    PrecioVentaAnt = dr.GetDouble(10),
                    Utilidad = dr.GetDouble(11),
                    Imagen = dr.GetString(12),
                    AltTextImagen = dr.GetString(13)
                };
            }

            return producto;
        }

        /* Consulta todos los productos en la base de datos */
        public List<ProductoDP> ConsultarAllMD()
        {
            List<ProductoDP> productos = new();

            string sql = @"
                SELECT 
                    p.PRO_Codigo,
                    c.CAT_Codigo,
                    c.CAT_Descripcion,
                    cl.CLA_Codigo,
                    cl.CLA_Nombre,
                    u.UME_Codigo,
                    u.UME_Descripcion,
                    p.PRO_Nombre,
                    p.PRO_Descripcion,
                    p.PRO_Precio_venta,
                    p.PRO_Precio_venta_ant,
                    p.PRO_Utilidad,
                    p.PRO_Imagen,
                    p.PRO_Alt_Imagen
                FROM PRODUCTO p
                JOIN CATEGORIA c ON c.CAT_Codigo = p.CAT_Codigo
                JOIN CLASIFICACION cl ON cl.CLA_Codigo = p.CLA_Codigo
                JOIN UNIDAD_MEDIDA u ON u.UME_Codigo = p.UME_Codigo";

            using OracleConnection conn = OracleDB.CrearConexion();
            conn.Open();

            using OracleCommand cmd = new OracleCommand(sql, conn);
            using OracleDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                productos.Add(new ProductoDP
                {
                    Codigo = dr.GetString(0),

                    CategoriaCodigo = dr.GetString(1),
                    CategoriaDescripcion = dr.GetString(2),

                    ClasificacionCodigo = dr.GetString(3),
                    ClasificacionDescripcion = dr.GetString(4),

                    UnidadMedidaCodigo = dr.GetString(5),
                    UnidadMedidaDescripcion = dr.GetString(6),

                    Nombre = dr.GetString(7),
                    Descripcion = dr.GetString(8),
                    PrecioVenta = dr.GetDouble(9),
                    PrecioVentaAnt = dr.GetDouble(10),
                    Utilidad = dr.GetDouble(11),
                    Imagen = dr.GetString(12),
                    AltTextImagen = dr.GetString(13)
                });
            }
            return productos;
        }

        /* Verifica si el producto existe en la base de datos para evitar repetidos*/
        public bool VerificarMD(string codigo)
        {
            string sql = "SELECT COUNT(*) FROM PRODUCTO WHERE PRO_Codigo = :codigo";

            using OracleConnection conn = OracleDB.CrearConexion();
            conn.Open();

            using OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(":codigo", codigo);

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();

            return count > 0;
        }
    }
}
