using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracion.DP
{
    public class MateriaPrimaDP
    {
        public string MtpCodigo { get; set; }          // MTP_CODIGO
        public string UmeCodigo { get; set; }       // EMP_CEDULA_RUC
        public string MtpNombre { get; set; }          // MTP_NOMBRE
        public string MtpDescripcion { get; set; }     // MTP_DESCRIPCION
        public double MtpPrecioCompraAnt { get; set; } // MTP_PRECIO_COMPRA_ANT
        public double MtpPrecioCompra { get; set; }    // MTP_PRECIO_COMPRA
    }
}
