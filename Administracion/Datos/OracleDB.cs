using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Administracion.Datos
{
    public static class OracleDB
    {
        /**
         * Crea y devuelve una conexión Oracle usando parámetros leídos desde un archivo .properties.
         */
        public static OracleConnection CrearConexion()
        {
            try
            {
                string propertiesFile = ConfigurationManager.AppSettings["propertiesFile"] ?? "Conexion.properties";
                Dictionary<string, string> props = LeerProperties(propertiesFile);

                string host = ObtenerValor(props, "db.host");
                string port = ObtenerValor(props, "db.port");
                string serviceName = ObtenerValor(props, "db.serviceName");
                string user = ObtenerValor(props, "db.user");
                string password = ObtenerValor(props, "db.password");

                string dataSource =
                    "(DESCRIPTION=" +
                        "(ADDRESS=(PROTOCOL=TCP)(HOST=" + host + ")(PORT=" + port + "))" +
                        "(CONNECT_DATA=(SERVICE_NAME=" + serviceName + ")))";

                var connBuilder = new OracleConnectionStringBuilder
                {
                    UserID = user,
                    Password = password,
                    DataSource = dataSource
                };

                return new OracleConnection(connBuilder.ConnectionString);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la conexión a Oracle. Verifique App.config y Conexion.properties.", ex);
            }
        }

        /**
         * Lee un archivo .properties (clave=valor) y lo convierte en diccionario.
         */
        private static Dictionary<string, string> LeerProperties(string propertiesPath)
        {
            string finalPath = propertiesPath;

            if (!File.Exists(finalPath))
            {
                string baseDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, propertiesPath);
                if (File.Exists(baseDirPath))
                {
                    finalPath = baseDirPath;
                }
            }

            if (!File.Exists(finalPath))
            {
                throw new FileNotFoundException("No se encontró el archivo de configuración: " + propertiesPath);
            }

            var props = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (string line in File.ReadAllLines(finalPath))
            {
                string cleanLine = line.Trim();

                if (cleanLine.Length == 0 || cleanLine.StartsWith("#"))
                {
                    continue;
                }

                int idx = cleanLine.IndexOf('=');
                if (idx <= 0)
                {
                    continue;
                }

                string key = cleanLine.Substring(0, idx).Trim();
                string value = cleanLine.Substring(idx + 1).Trim();

                props[key] = value;
            }

            return props;
        }

        /**
         * Obtiene un valor requerido del diccionario.
         */
        private static string ObtenerValor(Dictionary<string, string> props, string key)
        {
            if (!props.TryGetValue(key, out string value) || string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Falta el parámetro " + key + " en el archivo .properties.");
            }

            return value;
        }
    }
}
