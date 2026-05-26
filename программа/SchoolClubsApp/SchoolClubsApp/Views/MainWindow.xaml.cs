using System.Windows;

namespace SchoolClubsApp.Views
{
    public partial class MainWindow : Window
    {
        private string currentRole;

        public MainWindow(string role)
        {
            InitializeComponent();

            currentRole = role;

            ConfigureAccess();
        }

        private void ConfigureAccess()
        {
            if (currentRole == "Ученик")
            {
                AttendanceButton.IsEnabled = false;
            }
        }

        private void StudentsButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            StudentsWindow window =
                new StudentsWindow(currentRole);

            window.ShowDialog();
        }

        private void ClubsButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            ClubsWindow window =
                new ClubsWindow(currentRole);

            window.ShowDialog();
        }

        private void AttendanceButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            AttendanceWindow window =
                new AttendanceWindow(currentRole);

            window.ShowDialog();
        }

        private void TeachersButton_Click(object sender, RoutedEventArgs e)
        {
            TeachersWindow window = new TeachersWindow(currentRole);
            window.ShowDialog();
        }

        private void LogoutButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            LoginWindow login =
                new LoginWindow();

            login.Show();

            this.Close();
        }
    }
}