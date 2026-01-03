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
using Administracion.DP;

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

        private void BtnToggleEstado_Click(object sender, RoutedEventArgs e)
        {
            ClienteDP? cli = ClienteSeleccionado();
            if (cli == null)
            {
                MessageBox.Show("Seleccione un cliente.");
                return;
            }

            try
            {
                string nuevoEstado = cli.EstadoCodigo == "A" ? "I" : "A";
                clienteService.CambiarEstado(cli.UsrNombre, nuevoEstado);
                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar estado:\n" + ex.Message);
            }
        }

        private void BtnModificarRol_Click(object sender, RoutedEventArgs e)
        {
            ClienteDP? cli = ClienteSeleccionado();
            if (cli == null)
            {
                MessageBox.Show("Seleccione un cliente.");
                return;
            }

            // Alterna usuario entre ADMIN y CLIENTE
            string nuevoRol = string.Equals(cli.Rol, "ADMIN", StringComparison.OrdinalIgnoreCase)
                ? "CLIENTE"
                : "ADMIN";

            try
            {
                clienteService.CambiarRol(cli.UsrNombre, nuevoRol);
                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar rol:\n" + ex.Message);
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
                if (string.IsNullOrWhiteSpace(TxtUsrNombre.Text) ||
                    string.IsNullOrWhiteSpace(TxtCedula.Text) ||
                    CmbRol.SelectedItem == null)
                {
                    MessageBox.Show("Complete todos los campos.");
                    return;
                }

                clienteService.InsertarCliente(
                    TxtUsrNombre.Text.Trim(),
                    TxtCedula.Text.Trim(),
                    TxtPassword.Password.Trim(),
                    ((ComboBoxItem)CmbRol.SelectedItem).Content.ToString()
                );

                PanelNuevoCliente.Visibility = Visibility.Collapsed;
                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ingresar cliente:\n" + ex.Message);
            }
        }
        private void LimpiarFormulario()
        {
            TxtUsrNombre.Text = "";
            TxtCedula.Text = "";
            TxtPassword.Password = "";
            CmbRol.SelectedIndex = -1;
        }

    }
}

