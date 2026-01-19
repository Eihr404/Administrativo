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
            this.productoDP = seleccionado; // Guardamos la referencia para el PrecioVentaAnt
            lblTituloForm.Text = OracleDB.GetConfig("titulo.formulario.editar");

            // Mapeo de atributos al formulario
            txtPrdCodigo.Text = seleccionado.Codigo;
            txtPrdCodigo.IsEnabled = false;
            txtPrdNombre.Text = seleccionado.Nombre;
            txtPrdDesc.Text = seleccionado.Descripcion;
            txtPrdPrecio.Text = seleccionado.PrecioVenta.ToString();
            txtPrdUtilidad.Text = seleccionado.Utilidad.ToString();
            txtPrdAltImagen.Text = seleccionado.AltTextImagen;

            // Asignación de Combos (SelectedValue usa el SelectedValuePath del XAML)
            cmbCategoria.SelectedValue = seleccionado.CategoriaCodigo;
            cmbClasificacion.SelectedValue = seleccionado.ClasificacionCodigo;

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

                if (!double.TryParse(txtPrdPrecio.Text, out double precio) ||
                    !double.TryParse(txtPrdUtilidad.Text, out double utilidad))
                {
                    MessageBox.Show(OracleDB.GetConfig("error.formato.numerico"));
                    return;
                }

                if (MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.guardar"),
                    OracleDB.GetConfig("titulo.confirmacion"), MessageBoxButton.YesNo) == MessageBoxResult.No) return;

                // Creación del objeto con todos los atributos necesarios
                ProductoDP datos = new ProductoDP
                {
                    Codigo = txtPrdCodigo.Text.Trim(),
                    Nombre = txtPrdNombre.Text.Trim(),
                    Descripcion = txtPrdDesc.Text.Trim(),
                    PrecioVenta = precio,
                    Utilidad = utilidad,
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
            if (prdDatGri.SelectedItem is not ProductoDP seleccionado) return;

            if (MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.borrar"),
                OracleDB.GetConfig("titulo.confirmacion"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (seleccionado.EliminarDP())
                {
                    MessageBox.Show(OracleDB.GetConfig("exito.eliminar"));
                    CargarProductos();
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
            txtPrdUtilidad.Clear();
            txtPrdImagen.Clear();
            txtPrdAltImagen.Clear();
            cmbCategoria.SelectedIndex = -1;
            cmbClasificacion.SelectedIndex = -1;
     
        }

        private bool CamposInvalidos()
        {
            return string.IsNullOrWhiteSpace(txtPrdCodigo.Text) ||
                   string.IsNullOrWhiteSpace(txtPrdNombre.Text) ||
                   cmbCategoria.SelectedValue == null ||
                   cmbClasificacion.SelectedValue == null;
        }
    }
}