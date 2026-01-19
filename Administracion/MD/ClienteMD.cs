using Administracion.Datos;
using Administracion.DP;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace Administracion.MD
{
    public class ClienteMD
    {
        /**
         * Devuelve todos los usuarios clientes (USUARIO_APP).
         */
        public List<ClienteDP> ObtenerClientes()
        {
            var clientes = new List<ClienteDP>();
            const string sql = "SELECT CLI_CEDULA, CLI_CORREO, CLI_TELEFONO FROM CLIENTE";

            try
            {
                using var conn = OracleDB.CrearConexion();
                conn.Open();
                using var cmd = new OracleCommand(sql, conn);
                using var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    clientes.Add(new ClienteDP
                    {
                        // ÍNDICES CORREGIDOS A 0, 1, 2
                        CliCedula = dr.IsDBNull(0) ? "" : dr.GetString(0),
                        CliCorreo = dr.IsDBNull(1) ? "" : dr.GetString(1),
                        CliTelefono = dr.IsDBNull(2) ? "" : dr.GetString(2)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener clientes.", ex);
            }
            return clientes;
        }

        /**
         * Busca por usuario o cédula (parcial).
         */
        public List<ClienteDP> BuscarClientes(string textoBusqueda)
        {
            var clientes = new List<ClienteDP>();
            // Asegúrate de que el SELECT tenga el orden que esperas
            const string sql = @"
                SELECT CLI_CEDULA, CLI_CORREO, CLI_TELEFONO
                FROM CLIENTE
                WHERE UPPER(CLI_CEDULA) LIKE UPPER(:pTexto)
            ";

            try
            {
                using var conn = OracleDB.CrearConexion();
                conn.Open();
                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("pTexto", "%" + (textoBusqueda ?? "") + "%"));

                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    clientes.Add(new ClienteDP
                    {
                        // Corregimos los índices: 0, 1, 2
                        CliCedula = dr.IsDBNull(0) ? "" : dr.GetString(0),
                        CliCorreo = dr.IsDBNull(1) ? "" : dr.GetString(1),
                        CliTelefono = dr.IsDBNull(2) ? "" : dr.GetString(2)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en MD: No se pudo recuperar la lista de clientes.", ex);
            }
            return clientes;
        }


        public bool EliminarCliente(string codigo)
        {
            string sql = "DELETE FROM CLIENTE WHERE CLI_CEDULA = :codigo";

            try
            {
                using OracleConnection conn = OracleDB.CrearConexion();
                conn.Open();

                using OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":codigo", codigo);

                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"{OracleDB.GetConfig("error.general")} (EliminarMD): {ex.Message}");
            }
        }

        /**
         * Inserta un nuevo usuario.
         */
        public int InsertarCliente(string cedula, string correo, string telefono, string empresa)
        {
            // Se agregó :empresa al final de VALUES
            const string sql = @"
        INSERT INTO CLIENTE (CLI_CEDULA, CLI_CORREO, CLI_TELEFONO, EMP_CEDULA_RUC)
        VALUES (:cedula, :correo, :telef, :empresa)";

            using var conn = OracleDB.CrearConexion();
            conn.Open();

            using var cmd = new OracleCommand(sql, conn);
            // El orden de los parámetros debe coincidir con el SQL
            cmd.Parameters.Add(new OracleParameter("cedula", cedula));
            cmd.Parameters.Add(new OracleParameter("correo", correo));
            cmd.Parameters.Add(new OracleParameter("telef", telefono));
            cmd.Parameters.Add(new OracleParameter("empresa", empresa));

            return cmd.ExecuteNonQuery();
        }

    }
}
