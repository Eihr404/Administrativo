using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Administracion.MD;

namespace Administracion.DP
{
    public class EstandarProduccionDP
    {
        public string MtpCodigo { get; set; }
        public string ProCodigo { get; set; }
        public string EdpDescripcion { get; set; }
        public double EdpCantidad { get; set; }

        // Instancia del modelo
        EstandarProduccionMD modelo = new EstandarProduccionMD();

        public List<EstandarProduccionDP> ConsultarAllDP() => modelo.ConsultarAllMD();
        public EstandarProduccionDP ConsultarByCodDP(string mtpCodigo, string proCodigo)
        {
            return modelo.ConsultarByCodMD(mtpCodigo, proCodigo);
        }
        public int InsertarDP() => modelo.IngresarMD(this);
        public int ActualizarDP() => modelo.ActualizarMD(this);
        public int EliminarDP() => modelo.EliminarMD(this.MtpCodigo, this.ProCodigo);
    }
}
