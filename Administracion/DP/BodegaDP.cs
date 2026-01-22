using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracion.DP
{
    class BodegaDP
    {
        public string Codigo { get; set; }          // BOD_CODIGO
        public string SucursalCodigo { get; set; }  // SUC_CODIGO
        public string Direccion { get; set; }       // BOD_DIRECCION
        public string Nombre { get; set; }          // BOD_DESCRIPCION (Se usa Nombre para los ComboBox)
        public double Capacidad { get; set; }       // BOD_CAPACIDAD
        /* Consulta todas las categorías registradas */
        public List<BodegaDP> ConsultarTodos()
        {
            return new BodegaMD().ConsultarAllMD();
        }
    }
}
