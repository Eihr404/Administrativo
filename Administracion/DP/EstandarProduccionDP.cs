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
        public double EdpCantidad { get; set; }

        // Instancia del modelo
        private EstandarProduccionMD modelo = new EstandarProduccionMD();

        public List<EstandarProduccionDP> ConsultarAllDP()
        {
            return modelo.ConsultarAllMD();
        }

        // Nuevo método para buscar por coincidencia en ambas llaves
        public List<EstandarProduccionDP> ConsultarByCodDP(string criterio)
        {
            return modelo.ConsultarByCodMD(criterio);
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
            return modelo.EliminarMD(this.MtpCodigo, this.ProCodigo);
        }
    }
}