using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace SchoolClubsApp.Views
{
    public partial class AttendanceWindow : Window
    {
        string connectionString =
            @"Server=pc\SQLEXPRESS;
              Database=SchoolClubsDB;
              Trusted_Connection=True;";

        private string currentRole;

        private DataTable attendanceTable =
            new DataTable();

        public AttendanceWindow(string role)
        {
            InitializeComponent();

            currentRole = role;

            ConfigureAccess();

            LoadAttendance();
        }

        private void ConfigureAccess()
        {
            if (currentRole == "Ученик")
            {
                AddButton.Visibility =
                    Visibility.Collapsed;

                EditButton.Visibility =
                    Visibility.Collapsed;

                DeleteButton.Visibility =
                    Visibility.Collapsed;
            }
        }

        private void LoadAttendance()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter adapter =
    new SqlDataAdapter(
        @"SELECT
            Attendance.AttendanceID,

            Students.LastName + ' ' +
            Students.FirstName + ' ' +
            Students.MiddleName
            AS ФИО,

            Attendance.LessonDate
            AS [Дата занятия],

            Attendance.Presence
            AS [Присутствовал]

          FROM Attendance

          INNER JOIN Students
          ON Attendance.StudentID =
             Students.StudentID",
        connection);

                attendanceTable.Clear();

                adapter.Fill(attendanceTable);

                AttendanceGrid.ItemsSource =
                    attendanceTable.DefaultView;
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

                SqlCommand getStudent =
                    new SqlCommand(
                        "SELECT TOP 1 StudentID FROM Students",
                        connection);

                object result =
                    getStudent.ExecuteScalar();

                if (result == null)
                {
                    MessageBox.Show(
                        "Нет учеников");

                    return;
                }

                int studentId =
                    Convert.ToInt32(result);

                SqlCommand command =
                    new SqlCommand(
                        @"INSERT INTO Attendance
                        (
                            StudentID,
                            LessonDate,
                            Presence
                        )

                        VALUES
                        (
                            @studentId,
                            GETDATE(),
                            1
                        )",
                        connection);

                command.Parameters.AddWithValue(
                    "@studentId",
                    studentId);

                command.ExecuteNonQuery();
            }

            LoadAttendance();

            MessageBox.Show(
                "Посещаемость добавлена");
        }

        private void EditButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (AttendanceGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите запись");

                return;
            }

            DataRowView row =
                (DataRowView)AttendanceGrid.SelectedItem;

            int attendanceId =
                Convert.ToInt32(
                    row["AttendanceID"]);

            int studentId = 1; 

            DateTime lessonDate =
                Convert.ToDateTime(
                    row["LessonDate"]);

            bool presence =
                Convert.ToBoolean(
                    row["Presence"]);

            EditAttendanceWindow window =
                new EditAttendanceWindow(
                    attendanceId,
                    studentId,
                    lessonDate,
                    presence);

            window.ShowDialog();

            LoadAttendance();
        }

        private void DeleteButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (AttendanceGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите запись");

                return;
            }

            DataRowView row =
                (DataRowView)AttendanceGrid.SelectedItem;

            int id =
                Convert.ToInt32(
                    row["AttendanceID"]);

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command =
                    new SqlCommand(
                        @"DELETE FROM Attendance
                          WHERE AttendanceID=@id",
                        connection);

                command.Parameters.AddWithValue(
                    "@id",
                    id);

                command.ExecuteNonQuery();
            }

            LoadAttendance();

            MessageBox.Show(
                "Запись удалена");
        }

        private void SearchBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            attendanceTable.DefaultView.RowFilter =
                $"Convert(StudentID, 'System.String') LIKE '%{SearchBox.Text}%'";
        }
    }
}