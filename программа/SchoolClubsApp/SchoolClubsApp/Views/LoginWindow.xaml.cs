using System;
using System.Data.SqlClient;
using System.Windows;

namespace SchoolClubsApp.Views
{
    public partial class LoginWindow : Window
    {
        string connectionString =
            @"Server=pc\SQLEXPRESS;
              Database=SchoolClubsDB;
              Trusted_Connection=True;";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                string query =
                    "SELECT Role FROM Users " +
                    "WHERE Login=@login AND Password=@password";

                SqlCommand command =
                    new SqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@login",
                    LoginBox.Text);

                command.Parameters.AddWithValue(
                    "@password",
                    PasswordBox.Password);

                object result =
                    command.ExecuteScalar();

                if (result != null)
                {
                    string role =
                        result.ToString();

                    MainWindow main =
                        new MainWindow(role);

                    main.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Неверный логин или пароль");
                }
            }
        }

        private void CloseButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}