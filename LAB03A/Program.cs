using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB03A
{
    public class Student
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=LAB1502-013\\SQLEXPRESS;" +
                                   "Initial Catalog=Tecsup2025DB;User Id=Alexander; Pwd=123456;" +
                                   "TrustServerCertificate=true";

            Console.WriteLine("=== LISTADO DE ESTUDIANTES (MODO DESCONECTADO - DATATABLE) ===");
            DataTable studentsTable = GetStudentsDataTable(connectionString);
            DisplayDataTable(studentsTable);

            Console.WriteLine("\n\n=== LISTADO DE ESTUDIANTES (MODO CONECTADO - DATAREADER) ===");
            List<Student> studentsList = GetStudentsList(connectionString);
            DisplayStudentList(studentsList);

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
        static DataTable GetStudentsDataTable(string connectionString)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT ProductId, Name, Price FROM Students";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                adapter.Fill(dataTable);
            }

            return dataTable;
        }

        static List<Student> GetStudentsList(string connectionString)
        {
            List<Student> students = new List<Student>();

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

            return students;
        }
        static void DisplayDataTable(DataTable dataTable)
        {
            Console.WriteLine("\nDatos desde DataTable:");
            Console.WriteLine("-----------------------");

            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine($"ID: {row["ProductId"]}, Nombre: {row["Name"]}, Precio: {row["Price"]:C}");
            }

            Console.WriteLine($"\nTotal de registros: {dataTable.Rows.Count}");
        }

        // Mostrar datos de la lista de objetos
        static void DisplayStudentList(List<Student> students)
        {
            Console.WriteLine("\nDatos desde Lista de Objetos:");
            Console.WriteLine("-------------------------------");

            foreach (var student in students)
            {
                Console.WriteLine(student.ToString());
            }

            Console.WriteLine($"\nTotal de registros: {students.Count}");
        }
    }
}
