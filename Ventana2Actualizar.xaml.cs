using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Ventana2Actualizar : Window
    {
        SqlConnection miConexionSQL;

        private int variableIDcliente; 
        public Ventana2Actualizar(int elId)
        {
            InitializeComponent();

            variableIDcliente = elId;

            string miConexion = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.MasterDBConnectionString"].ConnectionString;

            miConexionSQL = new SqlConnection(miConexion);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "UPDATE cliente SET nombreCliente=@nombre WHERE ID=" + variableIDcliente;

                SqlCommand miSqlCommand = new SqlCommand(consulta, miConexionSQL);

                miConexionSQL.Open();

                miSqlCommand.Parameters.AddWithValue("@nombre", cuadroActualiza.Text);

                miSqlCommand.ExecuteNonQuery();

                miConexionSQL.Close();

                MessageBox.Show("Nombre del cliente actualizado exitosamente");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
    }

