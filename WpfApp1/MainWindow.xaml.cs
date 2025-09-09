using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString;
        private DataTable studentsDataTable;

        public MainWindow()
        {
            InitializeComponent();
            connectionString = "Data Source=LAB1502-013\\SQLEXPRESS;" +
                           "Initial Catalog=Tecsup2025DB;User Id=Alexander; Pwd=123456;" +
                           "TrustServerCertificate=true";
        }

        private void ConexionConectada_Click(object sender, RoutedEventArgs e)
        {
            List<Student> students = new List<Student>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ProductId, Name, Price FROM Students";
                    SqlCommand command = new SqlCommand(query, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                ProductId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDecimal(2)
                            });
                        }
                    }
                }
                dgEstudiantes.ItemsSource = students;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ConexionDesconectada_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData(string filter = "")
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ProductId, Name, Price FROM Students";
                    // Si hay filtro, agregar WHERE
                    if (!string.IsNullOrEmpty(filter))
                    {
                        query += " WHERE Name LIKE @Filter";
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    if (!string.IsNullOrEmpty(filter))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Filter", "%" + filter + "%");
                    }
                    adapter.Fill(dataTable);
                }
                dgEstudiantes.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Evento del botón Buscar
        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string filter = txtBuscar.Text;
            LoadData(filter);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    public class Student
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

}