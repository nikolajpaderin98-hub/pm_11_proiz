using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace SchoolClubsApp.Views
{
    public partial class StudentsWindow : Window
    {
        string connectionString =
            @"Server=pc\SQLEXPRESS;
              Database=SchoolClubsDB;
              Trusted_Connection=True;";

        private string currentRole;

        public StudentsWindow(string role)
        {
            InitializeComponent();

            currentRole = role;

            ConfigureAccess();

            LoadStudents();
        }

        private void ConfigureAccess()
        {
            if (currentRole == "Ученик")
            {
                AddButton.Visibility =
                    Visibility.Collapsed;

                DeleteButton.Visibility =
                    Visibility.Collapsed;
            }

            if (currentRole == "Преподаватель")
            {
                DeleteButton.Visibility =
                    Visibility.Collapsed;
            }
        }

        private void LoadStudents()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter adapter =
                    new SqlDataAdapter(
                        "SELECT * FROM Students",
                        connection);

                DataTable table =
                    new DataTable();

                adapter.Fill(table);

                StudentsGrid.ItemsSource =
                    table.DefaultView;
            }
        }

        private void AddButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            AddStudentWindow window =
                new AddStudentWindow();

            window.ShowDialog();

            LoadStudents();
        }

        private void DeleteButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (StudentsGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите ученика");

                return;
            }

            DataRowView row =
                (DataRowView)StudentsGrid.SelectedItem;

            int id =
                (int)row["StudentID"];

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command =
                    new SqlCommand(
                        "DELETE FROM Students WHERE StudentID=@id",
                        connection);

                command.Parameters.AddWithValue(
                    "@id",
                    id);

                command.ExecuteNonQuery();
            }

            LoadStudents();
        }

        private void RefreshButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            LoadStudents();
        }
    }
}