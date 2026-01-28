using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Administracion.MD;

namespace Administracion.DP
{
    public class DetallePedidoDP
    {
        public string ProCodigo { get; set; } = string.Empty;
        public string PddCodigo { get; set; } = string.Empty;
        public int DppCantidad { get; set; }
        public double DppMonto { get; set; }
        public double ProPrecio { get; set; }

        // Instancia del modelo tal como en tu estándar
        private DetallePedidoMD modelo = new DetallePedidoMD();

        public List<DetallePedidoDP> ConsultarAllDP()
        {
            return modelo.ConsultarAllMD();
        }

        // Método para buscar por coincidencia en ambas llaves
        public List<DetallePedidoDP> ConsultarByCodDP(string criterio)
        {
            return modelo.ConsultarByCodMD(criterio);
        }

        public int InsertarDP()
        {
            return modelo.IngresarMD(this);
        }

        public int ActualizarDP()
        {
            return modelo.ActualizarMD(this);
        }

        public int EliminarDP()
        {
            return modelo.EliminarMD(this.ProCodigo, this.PddCodigo);
        }
    }
}