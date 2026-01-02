using Oracle.ManagedDataAccess.Client;
using System;

namespace Administracion.MD
{
    public static class TestConexionMD
    {
        /**
         * Prueba la conexión a la base Oracle.
         */
        public static bool ProbarConexion()
        {
            try
            {
                using (OracleConnection conn = Administracion.Datos.OracleDB.CrearConexion())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
