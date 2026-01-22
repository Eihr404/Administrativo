using Administracion.Datos;
using Administracion.DP;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Administracion.GUI
{
    public partial class VentanaNotaVenta : UserControl
    {
        private NotaVentaDP Resultado { get; set; }
        private bool esModificacion = false;

        public VentanaNotaVenta()
        {
            InitializeComponent();
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            try
            {
                NotaVentaDP mensajero = new NotaVentaDP();
                ndvDatGri.ItemsSource = mensajero.ConsultarAllDP();
            }
            catch (Exception ex) { MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}"); }
        }

        private void ndvBtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NotaVentaDP mensajero = new NotaVentaDP();
                string busqueda = ndvTxtblBuscarNumero.Text.Trim();
                ndvDatGri.ItemsSource = string.IsNullOrEmpty(busqueda)
                    ? mensajero.ConsultarAllDP()
                    : mensajero.ConsultarByCodDP(busqueda);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ndvBtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            esModificacion = false;
            LimpiarFormulario();
            TxtNdvNumero.IsEnabled = true;
            DtNdvFecha.SelectedDate = DateTime.Now;
            PanelFormularioNdv.Visibility = Visibility.Visible;
        }

        private void ndvBtnModificar_Click(object sender, RoutedEventArgs e)
        {
            NotaVentaDP seleccionado = ndvDatGri.SelectedItem as NotaVentaDP;
            if (seleccionado == null) { MessageBox.Show(OracleDB.GetConfig("error.validacion")); return; }

            esModificacion = true;
            Resultado = seleccionado;

            TxtNdvNumero.Text = seleccionado.NdvNumero;
            TxtNdvNumero.IsEnabled = false;
            TxtSucCodigo.Text = seleccionado.SucCodigo;
            TxtPddCodigoNota.Text = seleccionado.PddCodigo;
            DtNdvFecha.SelectedDate = seleccionado.NdvFechaEmision;
            TxtNdvMontoTotal.Text = seleccionado.NdvMontoTotal.ToString();
            TxtNdvResponsable.Text = seleccionado.NdvResponsable;
            TxtNdvDescripcion.Text = seleccionado.NdvDescripcion;

            PanelFormularioNdv.Visibility = Visibility.Visible;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtNdvNumero.Text) || string.IsNullOrWhiteSpace(TxtPddCodigoNota.Text))
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion")); return;
                }

                NotaVentaDP nota = new NotaVentaDP
                {
                    NdvNumero = TxtNdvNumero.Text.Trim(),
                    SucCodigo = TxtSucCodigo.Text.Trim(),
                    PddCodigo = TxtPddCodigoNota.Text.Trim(),
                    NdvFechaEmision = DtNdvFecha.SelectedDate ?? DateTime.Now,
                    NdvMontoTotal = double.TryParse(TxtNdvMontoTotal.Text, out double m) ? m : 0,
                    NdvResponsable = TxtNdvResponsable.Text.Trim(),
                    NdvDescripcion = TxtNdvDescripcion.Text.Trim()
                };

                int filas = esModificacion ? nota.ActualizarDP() : nota.InsertarDP();
                if (filas > 0)
                {
                    MessageBox.Show(esModificacion ? OracleDB.GetConfig("exito.actualizar") : OracleDB.GetConfig("exito.guardar"));
                    PanelFormularioNdv.Visibility = Visibility.Collapsed;
                    CargarDatosIniciales();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            PanelFormularioNdv.Visibility = Visibility.Collapsed;
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            TxtNdvNumero.Text = TxtSucCodigo.Text = TxtPddCodigoNota.Text = TxtNdvMontoTotal.Text = TxtNdvResponsable.Text = TxtNdvDescripcion.Text = string.Empty;
            DtNdvFecha.SelectedDate = null;
            Resultado = null;
        }

        private void ndvBtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Obtener el elemento seleccionado
            NotaVentaDP sel = ndvDatGri.SelectedItem as NotaVentaDP;

            // 2. Validar que no sea nulo antes de proceder
            if (sel == null)
            {
                MessageBox.Show("Por favor, seleccione una nota de venta de la lista.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // 3. Confirmación del usuario
            if (MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.borrar"), "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    // 4. Intento de eliminación en la base de datos
                    if (sel.EliminarDP() > 0)
                    {
                        // Mensaje de éxito desde el archivo de configuración
                        MessageBox.Show(OracleDB.GetConfig("exito.eliminar"), "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Actualizar la tabla
                        CargarDatosIniciales();
                    }
                }
                catch (Exception ex)
                {
                    // 5. Captura el error (evita que el programa se cierre) y muestra el motivo real
                    // Por ejemplo: "No se puede eliminar porque tiene detalles asociados"
                    MessageBox.Show($"No se pudo eliminar el registro.\n\nDetalle: {ex.Message}",
                                    "Error de Base de Datos",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }
    }
}