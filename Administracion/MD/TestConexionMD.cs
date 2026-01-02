using Oracle.ManagedDataAccess.Client;
using Administracion.Data;

namespace Administracion.MD
{
    public class TestConexionMD
    {
        public void ProbarConexion()
        {
            using OracleConnection conn = OracleDb.GetConnection();
            conn.Open();
        }
    }
}
