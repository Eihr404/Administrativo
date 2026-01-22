using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Administracion.MD;

namespace Administracion.DP
{
    public class DetalleNotaVentaDP
    {
        // Atributos basados en la tabla DETALLE_NOTA_VENTA
        public string NdvNumero { get; set; } = string.Empty;
        public string ProCodigo { get; set; } = string.Empty;
        public int DnvCantidad { get; set; }
        public double DnvMonto { get; set; }

        // Instancia del modelo para comunicación con la base de datos
        private DetalleNotaVentaMD modelo = new DetalleNotaVentaMD();

        /* Método para consulta general */
        public List<DetalleNotaVentaDP> ConsultarAllDP()
        {
            return modelo.ConsultarAllMD();
        }

        /* Método para buscar por coincidencia (Número de nota o Producto) */
        public List<DetalleNotaVentaDP> ConsultarByCodDP(string criterio)
        {
            return modelo.ConsultarByCodMD(criterio);
        }

        /* Método para Insertar */
        public int InsertarDP()
        {
            return modelo.IngresarMD(this);
        }

        /* Método para Actualizar */
        public int ActualizarDP()
        {
            return modelo.ActualizarMD(this);
        }

        /* Método para Eliminar (Usa llave compuesta) */
        public int EliminarDP()
        {
            return modelo.EliminarMD(this.NdvNumero, this.ProCodigo);
        }
    }
}