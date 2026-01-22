using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracion.DP
{
    public class PedidoDP
    {
        // Atributos de Pedido según la estructura de base de datos
        public string PddCodigo { get; set; } = string.Empty;       // PDD_CODIGO
        public string CliCedula { get; set; } = string.Empty;       // CLI_CEDULA
        public string PddComentario { get; set; } = string.Empty;   // PDD_COMENTARIO
        public string PddEstado { get; set; } = string.Empty;       // PDD_ESTADO
        public string PddUbicacion { get; set; } = string.Empty;    // PDD_UBICACION

        public double PddMontoTotal { get; set; }                   // PDD_MONTO_TOTAL
        public double PddAbono { get; set; }                        // PDD_ABONO

        /* Instancia del MD para comunicación con DB */
        private PedidoMD modelo = new PedidoMD();

        /* Método para consulta general */
        public List<PedidoDP> ConsultarAllDP()
        {
            PedidoMD modelo = new PedidoMD();
            return modelo.ConsultarAllMD();
        }

        /* Método para consulta por parámetro (código) */
        public List<PedidoDP> ConsultarByCodDP(string codigo)
        {
            PedidoMD modelo = new PedidoMD();
            return modelo.ConsultarByCodMD(codigo);
        }

        /* Métodos para Insertar */
        public int InsertarDP()
        {
            return modelo.IngresarMD(this);
        }

        /* Método para Actualizar */
        public int ActualizarDP()
        {
            return modelo.ActualizarMD(this);
        }

        /* Método para Eliminar */
        public int EliminarDP()
        {
            return modelo.EliminarMD(this.PddCodigo);
        }
    }
}