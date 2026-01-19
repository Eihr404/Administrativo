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
        public string MtpCodigo { get; set; } = string.Empty;
        public string UmeCodigo { get; set; } = string.Empty;
        public string BodCodigo { get; set; } = "0000001";

        public string MtpNombre { get; set; } = string.Empty;
        public string MtpDescripcion { get; set; } = string.Empty;

        public double MtpPrecioCompraAnt { get; set; }
        public double MtpPrecioCompra { get; set; }
        public int MtpExistencia { get; set; }

        /* Instancia del MD para comunicación con DB */
        private MateriaPrimaMD modelo = new MateriaPrimaMD();

        /*  Método para consulta general */
        public List<MateriaPrimaDP> ConsultarAllDP()
        {
            MateriaPrimaMD modelo = new MateriaPrimaMD();
            return modelo.ConsultarAllMD();
        }
        /* Método para consulta por parámetro (código) */
        public List<MateriaPrimaDP> ConsultarByCodDP(string codigo)
        {
            MateriaPrimaMD modelo = new MateriaPrimaMD();
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
            return modelo.EliminarMD(this.MtpCodigo);
        }
    }
}
