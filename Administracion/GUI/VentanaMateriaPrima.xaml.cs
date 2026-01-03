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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Administracion.DP; // Donde están tus Getters y Setters
using Administracion.MD; // Donde está tu conexión a Oracle

namespace Administracion.GUI
{
    public partial class MateriaPrima : UserControl
    {
        // Instanciamos el Modelo para poder usar sus métodos de base de datos
        MateriaPrimaMD modelo = new MateriaPrimaMD();

        public MateriaPrima()
        {
            InitializeComponent();
            // Llamamos al método de consulta apenas se crea la instancia
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            try
            {
                // Reutilizamos la lógica que ya tienes en el botón consultar
                // mtpTxtblBuscarCodigo.Text estará vacío, por lo que traerá todo (SELECT * FROM...)
                var resultado = modelo.Consultar(mtpTxtblBuscarCodigo.Text);
                mtpDatGri.ItemsSource = resultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos iniciales: " + ex.Message);
            }
        }
        private void ProbarConexion()
        {
            string cs = "User Id=a_prueba;Password=lticPUCE24;Data Source=192.168.5.125:1521/PRUEBA";
            using (var conn = new Oracle.ManagedDataAccess.Client.OracleConnection(cs))
            {
                try
                {
                    conn.Open();
                    MessageBox.Show("¡Conexión exitosa a Oracle!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fallo de conexión: " + ex.Message);
                }
            }
        }

        // Evento para el botón CONSULTAR
        private void mtpBtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Obtenemos el código del TextBox
                string codigoABuscar = mtpTxtblBuscarCodigo.Text.Trim();

                // 2. Llamamos al método Consultar pasando el filtro
                // Si el TextBox está vacío, el MD ejecutará el SELECT general
                var resultado = modelo.Consultar(codigoABuscar);

                // 3. Refrescamos el DataGrid con el resultado filtrado
                mtpDatGri.ItemsSource = resultado;

                // Opcional: Avisar si no se encontró nada
                if (resultado.Count == 0 && !string.IsNullOrEmpty(codigoABuscar))
                {
                    MessageBox.Show("No se encontró ninguna materia prima con el código: " + codigoABuscar);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar: " + ex.Message);
            }
        }

        // Evento para el botón INGRESAR
        private void mtpBtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Abrimos el formulario
            MateriaPrimaForm formulario = new MateriaPrimaForm();
            formulario.Owner = Window.GetWindow(this); // Para que se centre respecto al Main

            // 2. Si el usuario presionó "Guardar"
            if (formulario.ShowDialog() == true)
            {
                try
                {
                    // 3. Obtenemos el objeto del formulario y lo enviamos al MD
                    int filas = modelo.Insertar(formulario.Resultado);

                    if (filas > 0)
                    {
                        MessageBox.Show("Materia prima registrada.");
                        mtpBtnConsultar_Click(null, null); // Refrescar la tabla
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message);
                }
            }
        }

        // Evento para el botón ELIMINAR
        private void mtpBtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string codigoAEliminar = "";
                string nombreMostrar = "";

                // 1. Prioridad: Revisar si hay una fila seleccionada en el DataGrid
                if (mtpDatGri.SelectedItem is MateriaPrimaDP seleccionado)
                {
                    codigoAEliminar = seleccionado.MtpCodigo;
                    nombreMostrar = seleccionado.MtpNombre;
                }
                // 2. Si no hay selección, revisar el TextBox
                else if (!string.IsNullOrEmpty(mtpTxtblBuscarCodigo.Text.Trim()))
                {
                    codigoAEliminar = mtpTxtblBuscarCodigo.Text.Trim();
                    nombreMostrar = "el registro con código " + codigoAEliminar;
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione una fila o ingrese un código en el cuadro de búsqueda.");
                    return;
                }

                // 3. Confirmación del usuario
                var respuesta = MessageBox.Show($"¿Está seguro de que desea eliminar {nombreMostrar}?",
                                               "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (respuesta == MessageBoxResult.Yes)
                {
                    // 4. Llamada al método Eliminar en el MD
                    int filasAfectadas = modelo.Eliminar(codigoAEliminar);

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Registro eliminado correctamente.");
                        // Limpiamos el buscador y refrescamos la tabla
                        mtpTxtblBuscarCodigo.Clear();
                        mtpBtnConsultar_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró ningún registro con ese código para eliminar.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar eliminar: " + ex.Message);
            }
        }
    }
}
