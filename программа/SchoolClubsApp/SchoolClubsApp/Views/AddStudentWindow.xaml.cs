using System;
using System.Data.SqlClient;
using System.Windows;

namespace SchoolClubsApp.Views
{
    public partial class AddStudentWindow : Window
    {
        string connectionString =
            @"Server=localhost;
              Database=SchoolClubsDB;
              Trusted_Connection=True;";

        public AddStudentWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command =
                    new SqlCommand(
                        @"INSERT INTO Students
                        (
                            LastName,
                            FirstName,
                            MiddleName,
                            BirthDate,
                            Gender
                        )

                        VALUES
                        (
                            @lastName,
                            @firstName,
                            @middleName,
                            @birthDate,
                            @gender
                        )",
                        connection);

                command.Parameters.AddWithValue(
                    "@lastName",
                    LastNameBox.Text);

                command.Parameters.AddWithValue(
                    "@firstName",
                    FirstNameBox.Text);

                command.Parameters.AddWithValue(
                    "@middleName",
                    MiddleNameBox.Text);

                command.Parameters.AddWithValue(
                    "@birthDate",
                    BirthDatePicker.SelectedDate
                    ?? DateTime.Now);

                command.Parameters.AddWithValue(
                    "@gender",
                    GenderBox.Text);

                command.ExecuteNonQuery();
            }

            MessageBox.Show(
                "Ученик добавлен");

            this.Close();
        }
    }
}