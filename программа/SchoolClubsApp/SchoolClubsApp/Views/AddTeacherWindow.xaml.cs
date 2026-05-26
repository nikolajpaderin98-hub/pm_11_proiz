using System.Data.SqlClient;
using System.Windows;

namespace SchoolClubsApp.Views
{
    public partial class AddTeacherWindow : Window
    {
        private string connectionString =
            @"Server=pc\SQLEXPRESS;Database=SchoolClubsDB;Trusted_Connection=True;";

        public AddTeacherWindow()
        {
            InitializeComponent();
        }

        private void AddTeacher_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
INSERT INTO Teachers
(LastName, FirstName, MiddleName, Phone, Specialty)

VALUES
(@ln, @fn, @mn, @ph, @sp)";

                SqlCommand command =
                    new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ln", LastNameBox.Text);
                command.Parameters.AddWithValue("@fn", FirstNameBox.Text);
                command.Parameters.AddWithValue("@mn", MiddleNameBox.Text);
                command.Parameters.AddWithValue("@ph", PhoneBox.Text);
                command.Parameters.AddWithValue("@sp", SpecialtyBox.Text);

                command.ExecuteNonQuery();
            }

            MessageBox.Show("Преподаватель добавлен");

            Close();
        }
    }
}