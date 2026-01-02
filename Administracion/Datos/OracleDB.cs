using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace Administracion.Data
{
    public static class OracleDb
    {
        public static OracleConnection GetConnection()
        {
            var cs = ConfigurationManager.ConnectionStrings["OracleConn"];

            if (cs == null)
                throw new ConfigurationErrorsException(
                    "No se encontró la cadena de conexión 'OracleConn'.");

            return new OracleConnection(cs.ConnectionString);
        }
    }
}
