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
            NotaVentaDP sel = ndvDatGri.SelectedItem as NotaVentaDP;
            if (sel != null && MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.borrar"), "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (sel.EliminarDP() > 0) { MessageBox.Show(OracleDB.GetConfig("exito.eliminar")); CargarDatosIniciales(); }
            }
        }
    }
}