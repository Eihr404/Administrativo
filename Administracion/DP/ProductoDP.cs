using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracion.DP
{
    internal class ProductoDP
    {
        private ProductoMD productoMD = new ProductoMD();
        public ProductoDP() {
            Codigo = "Sin codigo";
            Categoria = "Sin categoría";
            Clasificacion = "Sin clasificación";
            UnidadMedida = "Sin unidad de medida";
            Nombre = "Sin nombre";
            Descripcion = "Sin descripción";
            PrecioVenta = 0;
            PrecioVentaAnt = 0;
            Utilidad = 0;
            Imagen = "Sin imagen de producto";
            AltTextImagen = "Sin texto alternativo";
        }
        public string Codigo { get; set; } = "Sin codigo";
        public string Categoria { get; set; } = "Sin categoría";
        public string Clasificacion { get; set; } = "Sin clasificación";
        public string UnidadMedida { get; set; } = "Sin unidad de medida";
        public string Nombre { get; set; } = "Sin nombre";
        public string Descripcion { get; set; } = "Sin descripción";
        public double PrecioVenta { get; set; } = 0;
        public double PrecioVentaAnt { get; set; } = 0;
        public double Utilidad { get; set; } = 0;
        public string Imagen { get; set; } = "Sin imagen de producto";
        public string AltTextImagen { get; set; } = "Sin texto alternativo";

        /* Ingresa un nuevo producto a la base de datos */
        public bool IngresarDP()
        {
            return productoMD.IngresarMD(this);
        }

        /* Modifica un producto en la base de datos */
        public void ModificarDP()
        {

        }

        /* Elimina un producto de la base de datos */
        public void EliminarDP()
        {

        }

        /* Consulta un producto por su código */
        public void ConsultarByCodDP()
        {

        }

        /* Consulta los productos en la base de datos */
        public List<ProductoDP> ConsultarAllDP()
        {
            List<ProductoDP> productos = new List<ProductoDP>();
            productos = productoMD.ConsultarAllMD();
            return productos;
        }

        /* Verifica si el producto existe en la base de datos para evitar repetidos*/
        public bool VerificarDP(string codigo)
        {
            return productoMD.VerificarMD(codigo);
        }
    }
}
