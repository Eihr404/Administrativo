using Oracle.ManagedDataAccess.Client;
using Administracion.Datos;
using Administracion.DP;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Administracion.MD
{
    public class ProveedorMD
    {
        public List<ProveedorDP> Listar()
        {
            List<ProveedorDP> lista = new();

            using OracleConnection conn = OracleDB.CrearConexion();
            conn.Open();

            string sql = @"SELECT
                             PRV_CODIGO,
                             EMP_CEDULA_RUC,
                             PRV_NOMBRE,
                             PRV_DIRECCION,
                             PRV_TELEFONO
                           FROM PROVEEDOR";

            using OracleCommand cmd = new OracleCommand(sql, conn);
            using OracleDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new ProveedorDP
                {
                    PrvCodigo = dr.GetString(0),
                    EmpCedulaRuc = dr.GetString(1),
                    PrvNombre = dr.IsDBNull(2) ? null : dr.GetString(2),
                    PrvDireccion = dr.IsDBNull(3) ? null : dr.GetString(3),
                    PrvTelefono = dr.IsDBNull(4) ? null : dr.GetString(4)
                });
            }

            return lista;
        }
    }
}
