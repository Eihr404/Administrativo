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

            const string sql = @"
                SELECT USR_NOMBRE, CLI_CEDULA, USR_ROL, USR_ESTADO
                FROM USUARIO_APP
                ORDER BY USR_NOMBRE ASC";

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
                        UsrNombre = dr.IsDBNull(0) ? "" : dr.GetString(0),
                        CliCedula = dr.IsDBNull(1) ? "" : dr.GetString(1),
                        Rol = dr.IsDBNull(2) ? "" : dr.GetString(2),
                        EstadoCodigo = dr.IsDBNull(3) ? "" : dr.GetString(3)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener clientes (USUARIO_APP).", ex);
            }

            return clientes;
        }

        /**
         * Busca por usuario o cédula (parcial).
         */
        public List<ClienteDP> BuscarClientes(string textoBusqueda)
        {
            var clientes = new List<ClienteDP>();

            const string sql = @"
                SELECT USR_NOMBRE, CLI_CEDULA, USR_ROL, USR_ESTADO
                FROM USUARIO_APP
                WHERE UPPER(USR_NOMBRE) LIKE UPPER(:pTexto)
                   OR UPPER(CLI_CEDULA) LIKE UPPER(:pTexto)
                ORDER BY USR_NOMBRE ASC";

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
                        UsrNombre = dr.IsDBNull(0) ? "" : dr.GetString(0),
                        CliCedula = dr.IsDBNull(1) ? "" : dr.GetString(1),
                        Rol = dr.IsDBNull(2) ? "" : dr.GetString(2),
                        EstadoCodigo = dr.IsDBNull(3) ? "" : dr.GetString(3)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar clientes (USUARIO_APP).", ex);
            }

            return clientes;
        }

        /**
         * Cambia el estado del usuario ('A'/'I').
         */
        public int CambiarEstado(string usrNombre, string nuevoEstadoCodigo)
        {
            const string sql = @"UPDATE USUARIO_APP SET USR_ESTADO = :pEstado WHERE USR_NOMBRE = :pUsr";

            try
            {
                using var conn = OracleDB.CrearConexion();
                conn.Open();

                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("pEstado", nuevoEstadoCodigo));
                cmd.Parameters.Add(new OracleParameter("pUsr", usrNombre));

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar estado del cliente.", ex);
            }
        }

        /**
         * Cambia el rol del usuario.
         */
        public int CambiarRol(string usrNombre, string nuevoRol)
        {
            const string sql = @"UPDATE USUARIO_APP SET USR_ROL = :pRol WHERE USR_NOMBRE = :pUsr";

            try
            {
                using var conn = OracleDB.CrearConexion();
                conn.Open();

                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("pRol", nuevoRol));
                cmd.Parameters.Add(new OracleParameter("pUsr", usrNombre));

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar rol del cliente.", ex);
            }
        }
    }
}
