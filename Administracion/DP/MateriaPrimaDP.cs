using Administracion.MD;
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

        // Instancia del MD para comunicación con DB
        private MateriaPrimaMD modelo = new MateriaPrimaMD();

        // MÉTODOS DE NEGOCIO QUE LLAMAN AL MD
        public List<MateriaPrimaDP> ConsultaGeneralDP()
        {
            MateriaPrimaMD modelo = new MateriaPrimaMD();
            return modelo.ConsultaGeneralMD();
        }

        public List<MateriaPrimaDP> ConsultaPorParametroDP(string codigo)
        {
            MateriaPrimaMD modelo = new MateriaPrimaMD();
            return modelo.ConsultaPorParametroMD(codigo);
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
            return modelo.EliminarMD(this.MtpCodigo);
        }
    }
}
