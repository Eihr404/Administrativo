using Administracion.MD;

namespace Administracion.DP
{
    public class ClienteDP
    {
        public string UsrNombre { get; set; } = string.Empty;                   
        public string CliCedula { get; set; } = string.Empty;                   
        public string Rol { get; set; } = string.Empty;
        public string EstadoCodigo { get; set; } = string.Empty;
        public string Estado
        {
            get
            {
                return EstadoCodigo == "A" ? "ACTIVO" : "ELIMINADO";
            }
        }
        public string NombreFactura { get; set; } = string.Empty;
        public string DireccionFactura { get; set; } = string.Empty;

    }
    public class ClienteDPService
    {
        private readonly ClienteMD clienteMd = new ClienteMD();

        public List<ClienteDP> ObtenerClientes()
        {
            return clienteMd.ObtenerClientes();
        }

        public List<ClienteDP> BuscarClientes(string texto)
        {
            return clienteMd.BuscarClientes(texto);
        }

        public int CambiarEstado(string usrNombre, string nuevoEstado)
        {
            return clienteMd.CambiarEstado(usrNombre, nuevoEstado);
        }

        public int CambiarRol(string usrNombre, string nuevoRol)
        {
            return clienteMd.CambiarRol(usrNombre, nuevoRol);
        }

        public int InsertarCliente(string usr, string cedula, string password, string rol)
        {
            return clienteMd.InsertarCliente(usr, cedula, password, rol);
        }
    }

}



