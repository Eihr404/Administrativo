using Administracion.Datos;
using Administracion.DP;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Administracion.GUI
{
    public partial class VentanaCliente : UserControl
    {
        private readonly ClienteDPService clienteService = new ClienteDPService();

        public VentanaCliente()
        {
            InitializeComponent();
            CargarClientes();
        }

        private void CargarClientes()
        {
            try
            {
                GridClientes.ItemsSource = clienteService.ObtenerClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")}\n{ex.Message}");
            }
        }

        private void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string criterio = cliTxtBuscar.Text.Trim();
                GridClientes.ItemsSource = clienteService.BuscarClientes(criterio);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar: {ex.Message}");
            }
        }

        private void CliBtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            PanelNuevoCliente.Visibility = Visibility.Visible;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtCedula.Text) || string.IsNullOrWhiteSpace(TxtCorreo.Text))
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                    return;
                }

                // Insertar usando el servicio existente
                clienteService.InsertarCliente(
                    TxtCedula.Text.Trim(),
                    TxtCorreo.Text.Trim(),
                    TxtTelefono.Text.Trim()
                );

                MessageBox.Show(OracleDB.GetConfig("exito.guardar"));
                PanelNuevoCliente.Visibility = Visibility.Collapsed;
                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}");
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var seleccionado = GridClientes.SelectedItem as ClienteDP;
            if (seleccionado == null)
            {
                MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                return;
            }

            if (MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.borrar"), "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    clienteService.EliminarCliente(seleccionado.CliCedula);
                    MessageBox.Show(OracleDB.GetConfig("exito.eliminar"));
                    CargarClientes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}");
                }
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            PanelNuevoCliente.Visibility = Visibility.Collapsed;
        }

        private void LimpiarFormulario()
        {
            TxtCedula.Clear();
            TxtCorreo.Clear();
            TxtTelefono.Clear();
        }
    }
}