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
                if (EstadoCodigo == "A")
                {
                    return "ACTIVO";
                }
                else
                {
                    return "INACTIVO";
                }
            }
        }

    }
}
