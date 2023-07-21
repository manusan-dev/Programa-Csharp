using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Data.SqlClient;
using System.Data;
using System.Net.NetworkInformation;
using System.Linq.Expressions;

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection miConexionSQL;

        public MainWindow()
        {
            InitializeComponent();
            string miConexion = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.MasterDBConnectionString"].ConnectionString;

            miConexionSQL = new SqlConnection(miConexion);

            // iniciar la tabla Clientes
            MuestraClientes();

            // mostrar los pedidos totales
            CargaPedidos();
        }

        private void MuestraClientes()
        {
            try
            {
                string consulta = "SELECT * FROM cliente";

                SqlDataAdapter miAdaptadorSQL = new SqlDataAdapter(consulta, miConexionSQL);

                using (miAdaptadorSQL)
                {
                    DataTable clientesTabla = new DataTable();

                    miAdaptadorSQL.Fill(clientesTabla);

                    // accede a la interfaz mediante el nombre del objeto (listaClientes) que es un listbox

                    listaClientes.ItemsSource = clientesTabla.DefaultView;
                    listaClientes.DisplayMemberPath = "nombreCliente";
                    listaClientes.SelectedValuePath = "Id";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MuestraPedidos() {

            try
            {

                string consulta = "SELECT * FROM pedido P INNER JOIN cliente C ON C.Id = P.codigoCliente" +
        " WHERE C.Id = @ClienteID";



                SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSQL);

                SqlDataAdapter miAdaptadorSQL = new SqlDataAdapter(sqlCommand);

                using (miAdaptadorSQL)
                {

                    sqlCommand.Parameters.AddWithValue("@ClienteID", listaClientes.SelectedValue);

                    DataTable pedidosTabla = new DataTable();

                    miAdaptadorSQL.Fill(pedidosTabla);

                    // accede a la interfaz mediante el nombre del objeto (listaClientes) que es un listbox

                    listaPedidos.ItemsSource = pedidosTabla.DefaultView;
                    listaPedidos.DisplayMemberPath = "fechaPedido";
                    listaPedidos.SelectedValuePath = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CargaPedidos() {

            try
            {
                string consulta = "SELECT *, CONCAT (codigoCliente,' ',fechaPedido,' ',formaPago) AS junta FROM pedido";

                SqlDataAdapter miAdaptadorSQL = new SqlDataAdapter(consulta, miConexionSQL);

                using (miAdaptadorSQL)
                {

                    DataTable totalesTabla = new DataTable();

                    miAdaptadorSQL.Fill(totalesTabla);

                    pedidosTotales.ItemsSource = totalesTabla.DefaultView;
                    pedidosTotales.DisplayMemberPath = "junta";
                    pedidosTotales.SelectedValuePath = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


        private void listaClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MuestraPedidos();
        }

        private void listaPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void pedidosTotales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ButtonDELETE(object sender, RoutedEventArgs e)
        {
            try {
            string consulta = "DELETE from pedido WHERE ID=@PEDIDOID";

            SqlCommand miSqlCommand = new SqlCommand(consulta, miConexionSQL);

            miConexionSQL.Open();

            miSqlCommand.Parameters.AddWithValue("@PEDIDOID", pedidosTotales.SelectedValue);

            miSqlCommand.ExecuteNonQuery();

            miConexionSQL.Close();

            MessageBox.Show("Pedido borrado exitosamente");

            CargaPedidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonINSERT(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "INSERT INTO cliente(nombreCliente) VALUES(@nombre)";

                SqlCommand miSqlCommand = new SqlCommand(consulta, miConexionSQL);

                miConexionSQL.Open();

                miSqlCommand.Parameters.AddWithValue("@nombre", insertaCliente.Text);

                miSqlCommand.ExecuteNonQuery();

                miConexionSQL.Close();

                MessageBox.Show("Cliente agregado exitosamente");

                MuestraClientes();

                insertaCliente.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonDELETE2(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "DELETE from cliente WHERE ID=@CLIENTEID";

                SqlCommand miSqlCommand = new SqlCommand(consulta, miConexionSQL);

                miConexionSQL.Open();

                miSqlCommand.Parameters.AddWithValue("@CLIENTEID", listaClientes.SelectedValue);

                miSqlCommand.ExecuteNonQuery();

                miConexionSQL.Close();

                MessageBox.Show("Cliente borrado exitosamente");

                MuestraClientes();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Ventana2Actualizar ventana2 = new Ventana2Actualizar((int)listaClientes.SelectedValue);

            ventana2.Show();

            try { 
            string consulta = "SELECT nombreCliente FROM cliente WHERE ID=@ESTEID";

                SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSQL);

            SqlDataAdapter miAdaptadorSQL = new SqlDataAdapter(sqlCommand);

                using (miAdaptadorSQL)
                {
                    sqlCommand.Parameters.AddWithValue("@ESTEID", listaClientes.SelectedValue);

                    DataTable clientesTabla = new DataTable();

                    miAdaptadorSQL.Fill(clientesTabla);

                   ventana2.cuadroActualiza.Text = clientesTabla.Rows[0]["nombreCliente"].ToString();


                }
                }   catch (Exception ex){
                
                MessageBox.Show("error en el nombre " + ex.ToString());
            }
                }

    }
}
