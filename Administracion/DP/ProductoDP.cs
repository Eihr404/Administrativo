using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Administracion.DP
{
    public class ProductoDP
    {
        private ProductoMD productoMD = new ProductoMD();
        public string Codigo { get; set; } = string.Empty;
        public string CategoriaCodigo { get; set; } = string.Empty;
        public string CategoriaDescripcion { get; set; } = string.Empty;
        public string ClasificacionCodigo { get; set; } = string.Empty;
        public string ClasificacionDescripcion { get; set; } = string.Empty;
        public string UnidadMedidaCodigo { get; set; } = string.Empty;
        public string UnidadMedidaDescripcion { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public double PrecioVenta { get; set; } = 0;
        public double PrecioVentaAnt { get; set; } = 0;
        public double Utilidad { get; set; } = 0;
        public string Imagen { get; set; } = string.Empty;
        public string AltTextImagen { get; set; } = string.Empty;

        /* Ingresa un nuevo producto a la base de datos */
        public bool IngresarDP()
        {
            return productoMD.IngresarMD(this);
        }

        /* Modifica un producto en la base de datos */
        public bool ModificarDP()
        {
            return productoMD.ModificarMD(this);
        }

        /* Elimina un producto de la base de datos */
        public bool EliminarDP()
        {
            return productoMD.EliminarMD(this.Codigo);
        }

        /* Consulta un producto por su código */
        public ProductoDP ConsultarByCodDP()
        {
            return productoMD.ConsultarByCodMD(this.Codigo);
        }

        /* Consulta los productos en la base de datos */
        public List<ProductoDP> ConsultarAllDP()
        {
            return productoMD.ConsultarAllMD() ?? new List<ProductoDP>();
        }

        /* Verifica si el producto existe en la base de datos para evitar repetidos*/
        public bool VerificarDP()
        {
            return productoMD.VerificarMD(this.Codigo);
        }
    }
}
