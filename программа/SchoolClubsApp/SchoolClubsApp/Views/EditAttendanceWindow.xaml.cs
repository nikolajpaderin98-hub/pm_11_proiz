using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace SchoolClubsApp.Views
{
    public partial class EditAttendanceWindow : Window
    {
        string connectionString =
            @"Server=pc\SQLEXPRESS;
              Database=SchoolClubsDB;
              Trusted_Connection=True;";

        private int attendanceId;

        public EditAttendanceWindow(
            int id,
            int studentId,
            DateTime lessonDate,
            bool presence)
        {
            InitializeComponent();

            attendanceId = id;

            LoadStudents();

            StudentComboBox.SelectedValue =
                studentId;

            LessonDatePicker.SelectedDate =
                lessonDate;

            PresenceCheckBox.IsChecked =
                presence;
        }

        private void LoadStudents()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter adapter =
                    new SqlDataAdapter(
                        @"SELECT
                            StudentID,
                            LastName + ' ' + FirstName
                            AS FullName
                          FROM Students",
                        connection);

                DataTable table =
                    new DataTable();

                adapter.Fill(table);

                StudentComboBox.ItemsSource =
                    table.DefaultView;

                StudentComboBox.DisplayMemberPath =
                    "FullName";

                StudentComboBox.SelectedValuePath =
                    "StudentID";
            }
        }

        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (StudentComboBox.SelectedValue == null)
            {
                MessageBox.Show(
                    "Выберите ученика");

                return;
            }

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command =
                    new SqlCommand(
                        @"UPDATE Attendance

                          SET
                            StudentID=@studentId,
                            LessonDate=@lessonDate,
                            Presence=@presence

                          WHERE AttendanceID=@id",
                        connection);

                command.Parameters.AddWithValue(
                    "@studentId",
                    StudentComboBox.SelectedValue);

                command.Parameters.AddWithValue(
                    "@lessonDate",
                    LessonDatePicker.SelectedDate
                    ?? DateTime.Now);

                command.Parameters.AddWithValue(
                    "@presence",
                    PresenceCheckBox.IsChecked
                    ?? false);

                command.Parameters.AddWithValue(
                    "@id",
                    attendanceId);

                command.ExecuteNonQuery();
            }

            MessageBox.Show(
                "Посещаемость обновлена");

            this.Close();
        }
    }
}