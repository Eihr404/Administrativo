using Administracion.Datos;
using Administracion.DP;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Administracion.GUI
{
    public partial class VentanaDetalleNotaVenta : UserControl
    {
        private DetalleNotaVentaDP seleccionadoDP;
        private bool esModificacion = false;

        public VentanaDetalleNotaVenta()
        {
            InitializeComponent();
            CargarDatosIniciales();
            CargarCatalogos();
        }

        private void CargarDatosIniciales()
        {
            try
            {
                dnvDatGri.ItemsSource = new DetalleNotaVentaDP().ConsultarAllDP();
            }
            catch (Exception ex) { MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}"); }
        }

        private void CargarCatalogos()
        {
            try
            {
                // Cargamos Notas de Venta y Productos para los ComboBox
                cmbNotaVenta.ItemsSource = new NotaVentaDP().ConsultarAllDP();
                cmbProductoDnv.ItemsSource = new ProductoDP().ConsultarAllDP(); // Asumiendo que existe ProductoDP
            }
            catch (Exception ex) { MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}"); }
        }

        private void dnvBtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            esModificacion = false;
            LimpiarCampos();
            cmbNotaVenta.IsEnabled = true;
            cmbProductoDnv.IsEnabled = true;
            PanelFormularioDnv.Visibility = Visibility.Visible;
        }

        private void dnvBtnModificar_Click(object sender, RoutedEventArgs e)
        {
            seleccionadoDP = dnvDatGri.SelectedItem as DetalleNotaVentaDP;
            if (seleccionadoDP == null)
            {
                MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                return;
            }

            esModificacion = true;
            cmbNotaVenta.SelectedValue = seleccionadoDP.NdvNumero;
            cmbProductoDnv.SelectedValue = seleccionadoDP.ProCodigo;
            txtDnvCantidad.Text = seleccionadoDP.DnvCantidad.ToString();
            txtDnvMonto.Text = seleccionadoDP.DnvMonto.ToString();

            // Bloquear llaves primarias en modificación
            cmbNotaVenta.IsEnabled = false;
            cmbProductoDnv.IsEnabled = false;

            PanelFormularioDnv.Visibility = Visibility.Visible;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DetalleNotaVentaDP objeto = new DetalleNotaVentaDP
                {
                    NdvNumero = cmbNotaVenta.SelectedValue?.ToString(),
                    ProCodigo = cmbProductoDnv.SelectedValue?.ToString(),
                    DnvCantidad = int.Parse(txtDnvCantidad.Text),
                    DnvMonto = double.Parse(txtDnvMonto.Text)
                };

                int filas = esModificacion ? objeto.ActualizarDP() : objeto.InsertarDP();

                if (filas > 0)
                {
                    MessageBox.Show(OracleDB.GetConfig(esModificacion ? "exito.actualizar" : "exito.guardar"));
                    PanelFormularioDnv.Visibility = Visibility.Collapsed;
                    CargarDatosIniciales();
                }
            }
            catch (Exception ex) { MessageBox.Show("Error al guardar: " + ex.Message); }
        }

        private void dnvBtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string criterio = dnvTxtblBuscarCodigo.Text.Trim();
                DetalleNotaVentaDP mensajeroDP = new DetalleNotaVentaDP();
                dnvDatGri.ItemsSource = string.IsNullOrEmpty(criterio)
                    ? mensajeroDP.ConsultarAllDP()
                    : mensajeroDP.ConsultarByCodDP(criterio);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dnvBtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var item = dnvDatGri.SelectedItem as DetalleNotaVentaDP;
            if (item != null && MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.borrar"),
                "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (item.EliminarDP() > 0) CargarDatosIniciales();
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) => PanelFormularioDnv.Visibility = Visibility.Collapsed;

        private void LimpiarCampos()
        {
            cmbNotaVenta.SelectedIndex = -1;
            cmbProductoDnv.SelectedIndex = -1;
            txtDnvCantidad.Text = "";
            txtDnvMonto.Text = "";
        }
    }
}