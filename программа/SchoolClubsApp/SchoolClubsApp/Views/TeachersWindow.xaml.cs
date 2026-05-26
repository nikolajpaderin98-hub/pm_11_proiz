using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace SchoolClubsApp.Views
{
    public partial class TeachersWindow : Window
    {
        private string connectionString =
            @"Server=pc\SQLEXPRESS;Database=SchoolClubsDB;Trusted_Connection=True;";

        private DataTable teachersTable;
        private string currentRole;

        public TeachersWindow(string role)
        {
            InitializeComponent();

            currentRole = role;

            LoadTeachers();
            ConfigureAccess();
        }

        private void ConfigureAccess()
        {
            if (currentRole == "Ученик")
            {
                AddButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            }
        }

        private void LoadTeachers()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
SELECT
    TeacherID AS 'ID',
    LastName AS 'Фамилия',
    FirstName AS 'Имя',
    MiddleName AS 'Отчество',
    Phone AS 'Телефон',
    Specialty AS 'Специальность'
FROM Teachers";

                SqlDataAdapter adapter =
                    new SqlDataAdapter(query, connection);

                teachersTable = new DataTable();

                adapter.Fill(teachersTable);

                TeachersGrid.ItemsSource =
                    teachersTable.DefaultView;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTeachers();
        }

        private void SearchBox_TextChanged(object sender,
                                           System.Windows.Controls.TextChangedEventArgs e)
        {
            if (teachersTable != null)
            {
                teachersTable.DefaultView.RowFilter =
                    $"Фамилия LIKE '%{SearchBox.Text}%'";
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddTeacherWindow window = new AddTeacherWindow();
            window.ShowDialog();

            LoadTeachers();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeachersGrid.SelectedItem == null)
                return;

            DataRowView row =
                (DataRowView)TeachersGrid.SelectedItem;

            int id = Convert.ToInt32(row["ID"]);

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                string query =
                    "DELETE FROM Teachers WHERE TeacherID=@id";

                SqlCommand command =
                    new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }

            LoadTeachers();
        }
    }
}