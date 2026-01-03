using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracion.DP
{
    internal class ClasificacionDP
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        /* Consulta todas las clasificaciones registradas */
        public List<ClasificacionDP> ConsultarTodos()
        {
            return new ClasificacionMD().ConsultarAllMD();
        }
    }
}
