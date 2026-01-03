using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracion.DP
{
    internal class CategoriaDP
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        /* Consulta todas las categorías registradas */
        public List<CategoriaDP> ConsultarTodos()
        {
            return new CategoriaMD().ConsultarAllMD();
        }
    }
}
