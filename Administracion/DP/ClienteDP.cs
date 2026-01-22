using Administracion.MD;

namespace Administracion.DP
{
    public class ClienteDP
    {
        public string CliCorreo { get; set; } = string.Empty;
        public string CliCedula { get; set; } = string.Empty;                 
        public string EmpCedula { get; set; } = "1790011223001";    
        public string CliTelefono { get; set; } = string.Empty;

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

        public int InsertarCliente(string cedula, string correo, string telefono)
        {
            return clienteMd.InsertarCliente(cedula, correo, telefono, "1790011223001");
        }

        public bool EliminarCliente(string cedula)
        {
            return clienteMd.EliminarCliente(cedula);
        }
    }

}



