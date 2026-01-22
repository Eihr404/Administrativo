using Administracion.MD;
using System;
using System.Collections.Generic;

namespace Administracion.DP
{
    public class NotaVentaDP
    {
        public string NdvNumero { get; set; } = string.Empty;
        public string SucCodigo { get; set; } = string.Empty;
        public string PddCodigo { get; set; } = string.Empty;
        public DateTime NdvFechaEmision { get; set; } = DateTime.Now;
        public double NdvMontoTotal { get; set; }
        public string NdvResponsable { get; set; } = string.Empty;
        public string NdvDescripcion { get; set; } = string.Empty;

        private NotaVentaMD modelo = new NotaVentaMD();

        public List<NotaVentaDP> ConsultarAllDP()
        {
            return new NotaVentaMD().ConsultarAllMD();
        }

        public List<NotaVentaDP> ConsultarByCodDP(string numero)
        {
            return new NotaVentaMD().ConsultarByCodMD(numero);
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
            return modelo.EliminarMD(this.NdvNumero);
        }
    }
}