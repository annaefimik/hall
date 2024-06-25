//AdminAddSession.xaml.cs
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaHall
{

    /// <summary>
    /// Логика взаимодействия для AdminAddSession.xaml
    /// </summary>
    public partial class AdminAddSession : UserControl
    {
        public AdminAddSession()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            errorLabel.Foreground = Brushes.Red;

            if (FilmComboBox.Text == "" || datePicker.SelectedDate == null || timePicker.Value == null || this.hours.Text == "" || this.minutes.Text == "" || HallComboBox.Text == "" || priceSlider.Value == priceSlider.Minimum)
            {
                errorLabel.Content = "Ошибка! Необходимо заполнить все поля";
            }
            else {
                string film = FilmComboBox.Text;
                DateTime date;
                if (datePicker.SelectedDate.Value < DateTime.Now)
                {
                    errorLabel.Content = "Вы не можете добавить сеанс на сегодняшнюю дату или дату, которая уже прошла";
                    return;
                }
                
                date = datePicker.SelectedDate.Value;
                TimeSpan time;
                if (timePicker.Value.Value.TimeOfDay < DateTime.Now.TimeOfDay && date <= DateTime.Now)
                {
                    errorLabel.Content = "Вы не можете добавить сеанс на время, которое уже прошло";
                    return ;
                }
               
                time = timePicker.Value.Value.TimeOfDay;
                int hours;
                int minutes;
                if (!int.TryParse(this.hours.Text, out hours) || !int.TryParse(this.minutes.Text, out minutes))
                {
                    errorLabel.Content = "Время должно быть числом!";
                    return;
                }
                int hall = Convert.ToInt16(HallComboBox.Text);
                double price = priceSlider.Value;

                string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";
                Sessions s = new Sessions(film, date + time, hall, price, hours + minutes / 60.0);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Sessions WHERE Hall = @Hall AND @SelectedDateTime BETWEEN DateAndTime AND DATEADD(minute, Duration, DateAndTime)", connection))
                    {
                        command.Parameters.AddWithValue("@Hall", s.Hall);
                        command.Parameters.AddWithValue("@SelectedDateTime", s.DateTime);
                        int count = (int)command.ExecuteScalar();
                        if (count > 0)
                        {
                            errorLabel.Content = "Зал уже занят в это время!";
                            return;
                        }
                    }
                }

                string query = "INSERT INTO sessions (NameFilm, Hall, DateAndTime, Price, Duration) VALUES (@NameFilm, @Hall, @DateAndTime, @Price, @Duration)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NameFilm", s.NameFilm);
                        command.Parameters.AddWithValue("@Hall", s.Hall);
                        command.Parameters.AddWithValue("@DateAndTime", s.DateTime);
                        command.Parameters.AddWithValue("@Price", s.Price);
                        command.Parameters.AddWithValue("@Duration", s.Duration); // Продолжительность в часах

                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                            errorLabel.Content = "Ошибка при добавлении в базу данных.";
                        else
                            errorLabel.Foreground = Brushes.Green;
                            errorLabel.Content = "Сессия успешно добавлена!";
                    }
                }

            }
            
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FilmComboBox.Text = "";
            datePicker.SelectedDate = null;
            timePicker.Value = null;
            this.hours.Text = "";
            this.minutes.Text = "";
            HallComboBox.Text = "";
            priceSlider.Value = priceSlider.Minimum;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True");
            using (connection)
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT NameFilm FROM Films", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        FilmComboBox.Items.Add(reader["NameFilm"].ToString());
                    }
                }
                connection.Close();
                DateTime selectedDateTime = DateTime.Now;
            }

            for (int i = 1; i <= 10; i++)
            {
                HallComboBox.Items.Add(i.ToString());
            }
        }

       
    }
}
