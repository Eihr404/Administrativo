using Administracion.MD; // Asegúrate de tener SucursalMD creada aquí
using System;
using System.Collections.Generic;

namespace Administracion.DP
{
    public class SucursalDP
    {
        // Propiedades basadas en la estructura de la tabla SUCURSAL
        public string SucCodigo { get; set; }        // SUC_CODIGO
        public string EmpCedulaRuc { get; set; }     // EMP_CEDULA_RUC
        public string SucNombre { get; set; }       // SUC_NOMBRE
        public string SucDireccion { get; set; }     // SUC_DIRECCION
        public string SucTelefono { get; set; }      // SUC_TELEFONO
        public string SucCorreo { get; set; }        // SUC_CORREO
        public string SucRepresentante { get; set; } // SUC_REPRESENTANTE
        public string SucCodigoSri { get; set; }     // SUC_CODIGO_SRI
        public string SucNumFactura { get; set; }    // SUC_NUM_FACTURA

        /// <summary>
        /// Consulta todas las sucursales registradas llamando a la capa de Acceso a Datos (MD)
        /// </summary>
        /// <returns>Lista de objetos SucursalDP</returns>
        public List<SucursalDP> ConsultarAllDP()
        {
            // Nota: Debes asegurarte de que SucursalMD tenga el método ConsultarAllMD
            // que retorne una List<SucursalDP>
            return new SucursalMD().ConsultarAllMD();
        }
    }
}