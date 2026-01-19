using Administracion.Datos;
using Administracion.DP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Administracion.GUI
{
    public partial class Cliente : UserControl
    {
        private readonly ClienteDPService clienteService = new ClienteDPService();

        public Cliente()
        {
            InitializeComponent();
            CargarClientes();
        }

        /**
         * Carga la lista completa en el grid
         */
        private void CargarClientes()
        {
            try
            {
                List<ClienteDP> data = clienteService.ObtenerClientes();
                GridClientes.ItemsSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "ERROR COMPLETO:\n\n" + ex.ToString(),
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }

        }

        private ClienteDP? ClienteSeleccionado()
        {
            return GridClientes.SelectedItem as ClienteDP;
        }

        private void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string texto = TxtBuscar.Text?.Trim() ?? "";
                GridClientes.ItemsSource = clienteService.BuscarClientes(texto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar:\n" + ex.Message);
            }
        }

        private void BtnToggleEliminar(object sender, RoutedEventArgs e)
        {
            if (GridClientes.SelectedItems.Count == 0)
            {
                MessageBox.Show("Seleccione al menos un cliente.");
                return;
            }
            ClienteDP? cli = ClienteSeleccionado();
            if (cli == null)
            {
                MessageBox.Show("Seleccione un cliente.");
                return;
            }

            try
            {
                clienteService.EliminarCliente(cli.CliCedula);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar estado:\n" + ex.Message);
            }
        }

  
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // nada
        }

        // Para el panel de nuevo cliente
        private void CliBtnIngresar(object sender, RoutedEventArgs e)
        {
            PanelNuevoCliente.Visibility = Visibility.Visible;
            LimpiarFormulario();
        }
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            PanelNuevoCliente.Visibility = Visibility.Collapsed;
        }
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtCorreo.Text) ||
                    string.IsNullOrWhiteSpace(TxtCedula.Text) ||
                    string.IsNullOrWhiteSpace(TxtTelefono.Text))
                {
                    MessageBox.Show("Complete todos los campos.");
                    return;
                }

                // CORRECCIÓN: El orden debe ser Cedula, Correo, Telefono según tu ClienteDPService
                clienteService.InsertarCliente(
                    TxtCedula.Text.Trim(),
                    TxtCorreo.Text.Trim(),
                    TxtTelefono.Text.Trim()
                );

                PanelNuevoCliente.Visibility = Visibility.Collapsed;
                CargarClientes(); // Recarga el grid
                MessageBox.Show("Cliente guardado con éxito.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ingresar cliente:\n" + ex.Message);
            }
        }
        private void LimpiarFormulario()
        {          
            TxtCedula.Text = "";
            TxtCorreo.Text = "";
            TxtTelefono.Text = "";
        }

        private void BtnEliminar(object sender, RoutedEventArgs e)
        {

            if (GridClientes.SelectedItems.Count == 0)
            {
                MessageBox.Show("Seleccione al menos un cliente.");
                return;
            }
            ClienteDP? cli = ClienteSeleccionado();
            if (cli == null)
            {
                MessageBox.Show("Seleccione un cliente.");
                return;
            }

            if (MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.borrar"),
                OracleDB.GetConfig("titulo.confirmacion"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    clienteService.EliminarCliente(cli.CliCedula);
                    MessageBox.Show(OracleDB.GetConfig("exito.eliminar"));
                    CargarClientes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cambiar estado:\n" + ex.Message);
                }
            }
            
        }
    }
}

