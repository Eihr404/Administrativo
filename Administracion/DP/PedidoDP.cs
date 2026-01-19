using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracion.DP
{
    internal class PedidoDP
    {
        private PedidoMD pedidoMD = new PedidoMD();
        public string PedCodigo { get; set; }      // PED_CODIGO
        public string CliCedula { get; set; }      // CLI_CODIGO
        public string PedComentario { get; set; }   // PED_COMENTARIO
        public string PedEstado { get; set; }     // PED_ESTADO
        public string PedUbicacion { get; set; }  // PED_UBICACION
        public double PedTotal { get; set; }     // PED_TOTAL
        public double PedAbono { get; set; }     // PED_ABONO

        public bool IngresarDP()
        {
            return pedidoMD.IngresarMD(this);
        }

        /* Modifica un producto en la base de datos */
        public bool ModificarDP()
        {
            return pedidoMD.ModificarMD(this);
        }

        /* Elimina un producto de la base de datos */
        public bool EliminarDP()
        {
            return pedidoMD.EliminarMD(this.PedCodigo);
        }

        /* Consulta un producto por su código */
        public List<PedidoDP> ConsultarByCodDP()
        {
            // Asegúrate de que pedidoMD esté declarado e instanciado en esta clase
            return pedidoMD.ConsultarByCodMD(this.PedCodigo);
        }

        /* Consulta los pedidos en la base de datos */
        // Corrige el tipo de retorno de 'ProductoDP' a 'PedidoDP'
        public List<PedidoDP> ConsultarAllDP()
        {
            // El 'new List<PedidoDP>()' ahora sí coincide con el tipo de retorno
            return pedidoMD.ConsultarAllMD() ?? new List<PedidoDP>();
        }

        /* Verifica si el producto existe en la base de datos para evitar repetidos*/
        public bool VerificarDP()
        {
            return pedidoMD.VerificarMD(this.PedCodigo);
        }
    }
}
