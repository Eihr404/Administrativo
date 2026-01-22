﻿using Administracion.Datos;
using Administracion.DP;
using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Administracion.GUI
{
    public partial class VentanaProducto : UserControl
    {
        private ProductoDP productoDP;
        private bool esModificacion = false;

        public VentanaProducto()
        {
            InitializeComponent();
            productoDP = new ProductoDP();
            CargarProductos();
            CargarCombos();
        }

        private void CargarProductos()
        {
            try
            {
                prdDatGri.ItemsSource = new ProductoDP().ConsultarAllDP();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void CargarCombos()
        {
            try
            {
                // Cargamos los catálogos necesarios para el formulario
                cmbCategoria.ItemsSource = new CategoriaDP().ConsultarTodos();
                cmbClasificacion.ItemsSource = new ClasificacionDP().ConsultarTodos();
                cmbBodega.ItemsSource = new BodegaDP().ConsultarTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void prdBtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string criterio = prdTxtblBuscarCodigo.Text.Trim();
                if (string.IsNullOrEmpty(criterio))
                {
                    CargarProductos();
                }
                else
                {
                    var result = new ProductoDP { Codigo = criterio }.ConsultarByCodDP();
                    if (result == null || result.Count == 0)
                    {
                        MessageBox.Show(OracleDB.GetConfig("error.no_encontrado"));
                    }
                    prdDatGri.ItemsSource = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void prdBtnIngresar_Click(object sender, RoutedEventArgs e)
        {

            esModificacion = false;
            lblTituloForm.Text = OracleDB.GetConfig("titulo.formulario.nuevo");
            LimpiarCampos();
            txtPrdCodigo.IsEnabled = true;
            PanelFormularioPrd.Visibility = Visibility.Visible;
        }

        private void prdBtnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (prdDatGri.SelectedItem is not ProductoDP seleccionado)
            {
                MessageBox.Show(OracleDB.GetConfig("error.no_encontrado"));
                return;
            }

            esModificacion = true;
            this.productoDP = seleccionado;
            lblTituloForm.Text = OracleDB.GetConfig("titulo.formulario.editar");

            // Mapeo de atributos al formulario
            txtPrdCodigo.Text = seleccionado.Codigo;
            txtPrdCodigo.IsEnabled = false;
            txtPrdNombre.Text = seleccionado.ProNombre;
            txtPrdDesc.Text = seleccionado.Descripcion;

            txtPrdExistencia.Text = seleccionado.Existencia.ToString();
            txtPrdPrecioAnt.Text = seleccionado.PrecioVentaAnt.ToString();
            txtPrdImagen.Text = seleccionado.Imagen;

            txtPrdPrecio.Text = seleccionado.PrecioVenta.ToString();
            txtPrdUtilidad.Text = seleccionado.Utilidad.ToString();
            txtPrdAltImagen.Text = seleccionado.AltTextImagen;

            cmbCategoria.SelectedValue = seleccionado.CategoriaCodigo;
            cmbClasificacion.SelectedValue = seleccionado.ClasificacionCodigo;
            cmbBodega.SelectedValue = seleccionado.BodegaCodigo;

            PanelFormularioPrd.Visibility = Visibility.Visible;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CamposInvalidos())
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                    return;
                }

                // Validación de formatos numéricos incluyendo la nueva Existencia
                if (!double.TryParse(txtPrdPrecio.Text, out double precio) ||
                    !double.TryParse(txtPrdUtilidad.Text, out double utilidad) ||
                    !int.TryParse(txtPrdExistencia.Text, out int existencia))
                {
                    MessageBox.Show(OracleDB.GetConfig("error.formato.numerico"));
                    return;
                }

                if (MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.guardar"),
                    OracleDB.GetConfig("titulo.confirmacion"), MessageBoxButton.YesNo) == MessageBoxResult.No) return;

                ProductoDP datos = new ProductoDP
                {
                    Codigo = txtPrdCodigo.Text.Trim(),
                    ProNombre = txtPrdNombre.Text.Trim(),
                    Descripcion = txtPrdDesc.Text.Trim(),
                    PrecioVenta = precio,
                    Utilidad = utilidad,
                    Existencia = existencia, 
                    BodegaCodigo = cmbBodega.SelectedValue.ToString(), 
                    Imagen = txtPrdImagen.Text.Trim(),
                    AltTextImagen = txtPrdAltImagen.Text.Trim(),
                    CategoriaCodigo = cmbCategoria.SelectedValue.ToString(),
                    ClasificacionCodigo = cmbClasificacion.SelectedValue.ToString(),
                    PrecioVentaAnt = esModificacion ? productoDP.PrecioVenta : 0
                };

                bool resultado = esModificacion ? datos.ModificarDP() : datos.IngresarDP();

                if (resultado)
                {
                    MessageBox.Show(esModificacion ? OracleDB.GetConfig("exito.actualizar") : OracleDB.GetConfig("exito.guardar"));
                    PanelFormularioPrd.Visibility = Visibility.Collapsed;
                    CargarProductos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void prdBtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Obtener el elemento seleccionado
            ProductoDP sel = prdDatGri.SelectedItem as ProductoDP;

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
                    if (sel.EliminarDP())
                    {
                        // Mensaje de éxito desde el archivo de configuración
                        MessageBox.Show(OracleDB.GetConfig("exito.eliminar"), "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Actualizar la tabla
                        CargarProductos();
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

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) => PanelFormularioPrd.Visibility = Visibility.Collapsed;

        private void LimpiarCampos()
        {
            txtPrdCodigo.Clear();
            txtPrdNombre.Clear();
            txtPrdDesc.Clear();
            txtPrdPrecio.Clear();
            txtPrdPrecioAnt.Clear(); 
            txtPrdExistencia.Clear();
            txtPrdUtilidad.Clear();
            txtPrdImagen.Clear();
            txtPrdAltImagen.Clear();
            cmbCategoria.SelectedIndex = -1;
            cmbClasificacion.SelectedIndex = -1;
            cmbBodega.SelectedIndex = -1;
        }

        private bool CamposInvalidos()
        {
            return string.IsNullOrWhiteSpace(txtPrdCodigo.Text) ||
                   string.IsNullOrWhiteSpace(txtPrdNombre.Text) ||
                   cmbCategoria.SelectedValue == null ||
                   cmbClasificacion.SelectedValue == null ||
                   cmbBodega.SelectedValue == null; ;
        }
    }
}