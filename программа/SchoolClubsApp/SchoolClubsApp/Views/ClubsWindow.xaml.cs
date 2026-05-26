using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace SchoolClubsApp.Views
{
    public partial class ClubsWindow : Window
    {
        string connectionString =
            @"Server=pc\SQLEXPRESS;
              Database=SchoolClubsDB;
              Trusted_Connection=True;";

        private string currentRole;

        private DataTable clubsTable =
            new DataTable();

        public ClubsWindow(string role)
        {
            InitializeComponent();

            currentRole = role;

            ConfigureAccess();

            LoadClubs();
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

        private void LoadClubs()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter adapter =
                    new SqlDataAdapter(
                        "SELECT * FROM Clubs",
                        connection);

                clubsTable.Clear();

                adapter.Fill(clubsTable);

                ClubsGrid.ItemsSource =
                    clubsTable.DefaultView;
            }
        }

        private void AddButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command =
                    new SqlCommand(
                        @"INSERT INTO Clubs
                        (ClubName, TeacherID, ScheduleID)

                        VALUES
                        ('Новый кружок',1,1)",
                        connection);

                command.ExecuteNonQuery();
            }

            LoadClubs();

            MessageBox.Show(
                "Кружок добавлен");
        }

        private void DeleteButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (ClubsGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите кружок");

                return;
            }

            DataRowView row =
                (DataRowView)ClubsGrid.SelectedItem;

            int id =
                (int)row["ClubID"];

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command =
                    new SqlCommand(
                        "DELETE FROM Clubs WHERE ClubID=@id",
                        connection);

                command.Parameters.AddWithValue(
                    "@id",
                    id);

                command.ExecuteNonQuery();
            }

            LoadClubs();

            MessageBox.Show(
                "Кружок удалён");
        }

        private void SearchBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            clubsTable.DefaultView.RowFilter =
                $"ClubName LIKE '%{SearchBox.Text}%'";
        }
    }
}