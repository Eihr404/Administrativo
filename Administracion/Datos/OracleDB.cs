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
            // Read configured file name (may be null)
            string configured = ConfigurationManager.AppSettings["propertiesFile"];

            // If configured references the old Path.properties name, map it to Conexion.properties
            if (!string.IsNullOrEmpty(configured) && configured.IndexOf("Path.properties", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                configured = configured.Replace("Path.properties", "Conexion.properties", StringComparison.OrdinalIgnoreCase);
            }

            // Prepare candidate paths (relative to app base dir)
            var candidates = new List<string>();
            if (!string.IsNullOrWhiteSpace(configured))
                candidates.Add(configured);

            // Common names/locations to try (project has the file under Datos\Conexion.properties or at app root as Conexion.properties)
            candidates.Add(Path.Combine("Datos", "Conexion.properties"));
            candidates.Add("Conexion.properties");
            candidates.Add(Path.Combine("Datos", "conexion.properties"));
            candidates.Add("conexion.properties");

            string foundPath = null;

            foreach (var candidate in candidates)
            {
                string finalCandidate = candidate;
                // If candidate is rooted, use as-is, otherwise combine with base directory
                if (!Path.IsPathRooted(finalCandidate))
                    finalCandidate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, finalCandidate);

                if (File.Exists(finalCandidate))
                {
                    foundPath = finalCandidate;
                    break;
                }
            }

            if (foundPath == null)
                throw new FileNotFoundException("No se encontró el archivo de propiedades. Se intentaron: " + string.Join(", ", candidates));

            var props = LeerProperties(foundPath);

            string host = ObtenerValor(props, "db.host", Path.GetFileName(foundPath));
            string port = ObtenerValor(props, "db.port", Path.GetFileName(foundPath));
            string serviceName = ObtenerValor(props, "db.serviceName", Path.GetFileName(foundPath));
            string user = ObtenerValor(props, "db.user", Path.GetFileName(foundPath));
            string password = ObtenerValor(props, "db.password", Path.GetFileName(foundPath));

            string dataSource =
                $"(DESCRIPTION=" +
                $"(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port}))" +
                $"(CONNECT_DATA=(SERVICE_NAME={serviceName})))";

            var connBuilder = new OracleConnectionStringBuilder
            {
                UserID = user,
                Password = password,
                DataSource = dataSource
            };

            return new OracleConnection(connBuilder.ConnectionString);
        }
        /**
         * Lee un archivo .properties (clave=valor) y lo convierte en diccionario.
         */

        private static Dictionary<string, string> LeerProperties(string propertiesPath)
        {
            string finalPath = propertiesPath;

            // If a relative path was provided, make it absolute relative to the app base directory
            if (!Path.IsPathRooted(finalPath))
                finalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, finalPath);

            if (!File.Exists(finalPath))
                throw new FileNotFoundException(
                    "No se encontró el archivo: " + finalPath
                );

            var props = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var line in File.ReadAllLines(finalPath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                    continue;

                var parts = line.Split('=', 2);
                if (parts.Length == 2)
                    props[parts[0].Trim()] = parts[1].Trim();
            }

            return props;
        }
        /**
         * Obtiene un valor requerido del diccionario.
         */

        private static string ObtenerValor(Dictionary<string, string> props, string key, string propertiesFileName)
        {
            if (!props.ContainsKey(key))
                throw new Exception($"Falta el parámetro {key} en {propertiesFileName}");

            return props[key];
        }
    }
}
