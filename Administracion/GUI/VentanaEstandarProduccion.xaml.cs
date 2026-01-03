using Administracion.Datos;
using Administracion.DP;
using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Administracion.GUI
{
    public partial class EstandarProduccion : UserControl
    {
        EstandarProduccionMD modelo = new EstandarProduccionMD();

        public EstandarProduccion()
        {
            InitializeComponent();
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            try
            {
                EstandarProduccionDP mensajeroDP = new EstandarProduccionDP();
                edpDatGri.ItemsSource = mensajeroDP.ConsultarAllDP();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void edpBtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EstandarProduccionDP mensajeroDP = new EstandarProduccionDP();
                string textoBusqueda = edpTxtblBuscarCodigo.Text.Trim();

                // Inicializamos la lista como vacía por defecto
                List<EstandarProduccionDP> resultado = new List<EstandarProduccionDP>();

                if (string.IsNullOrWhiteSpace(textoBusqueda))
                {
                    resultado = mensajeroDP.ConsultarAllDP();
                }
                else
                {
                    string[] partes = textoBusqueda.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Solo ejecutamos la consulta si hay exactamente 2 códigos
                    if (partes.Length == 2)
                    {
                        // Agregamos el objeto único a la lista para evitar el error CS0029
                        var item = mensajeroDP.ConsultarByCodDP(partes[0], partes[1]);
                        if (item != null) resultado.Add(item);
                    }
                }

                edpDatGri.ItemsSource = resultado;

                if (resultado.Count == 0 && !string.IsNullOrWhiteSpace(textoBusqueda))
                {
                    MessageBox.Show(OracleDB.GetConfig("error.no_encontrado"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void edpBtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            EstandarProduccionForm formulario = new EstandarProduccionForm();
            formulario.Owner = Window.GetWindow(this);

            if (formulario.ShowDialog() == true)
            {
                try
                {
                    if (formulario.Resultado.InsertarDP() > 0)
                    {
                        MessageBox.Show(OracleDB.GetConfig("exito.guardar"),
                                        OracleDB.GetConfig("titulo.confirmacion"),
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                        CargarDatosIniciales();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
                }
            }
        }

        private void edpBtnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EstandarProduccionDP seleccionado = edpDatGri.SelectedItem as EstandarProduccionDP;

                if (seleccionado == null)
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                    return;
                }

                EstandarProduccionForm formulario = new EstandarProduccionForm(seleccionado);
                formulario.Owner = Window.GetWindow(this);

                if (formulario.ShowDialog() == true)
                {
                    if (formulario.Resultado.ActualizarDP() > 0)
                    {
                        MessageBox.Show(OracleDB.GetConfig("exito.actualizar"));
                        CargarDatosIniciales();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void edpBtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EstandarProduccionDP seleccionado = edpDatGri.SelectedItem as EstandarProduccionDP;

                if (seleccionado == null)
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                    return;
                }

                var respuesta = MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.borrar"),
                                               OracleDB.GetConfig("titulo.confirmacion"),
                                               MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (respuesta == MessageBoxResult.Yes)
                {
                    if (seleccionado.EliminarDP() > 0)
                    {
                        MessageBox.Show(OracleDB.GetConfig("exito.eliminar"));
                        CargarDatosIniciales();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }
    }
}