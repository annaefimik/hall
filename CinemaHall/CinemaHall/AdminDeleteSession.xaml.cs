//AdminDeleteSession.xaml.cs
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
    /// Логика взаимодействия для AdminDeleteSession.xaml
    /// </summary>
    public partial class AdminDeleteSession : UserControl
    {
        public AdminDeleteSession()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            errorLabel.Foreground = Brushes.Red;

            if (SessionComboBox.SelectedItem == null)
            {
                errorLabel.Content = "Необходимо выбрать сеанс";
                return;
            }

            string selectedSession = SessionComboBox.SelectedItem.ToString();
            string[] sessionDetails = selectedSession.Split(',');
            if (sessionDetails.Length != 3)
            {
                errorLabel.Content = "Выбранный сеанс имеет неверный формат!";
                return;
            }

            string filmName = sessionDetails[0].Trim();
            DateTime dateAndTime = DateTime.Parse(sessionDetails[1].Trim());
            string hall = sessionDetails[2].Split(':')[1].Trim();

            string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";

            // Удаление билетов
            string deleteTicketsQuery = "DELETE FROM SoldTickets WHERE Session_Id IN (SELECT Session_Id FROM Sessions WHERE NameFilm = @NameFilm AND DateAndTime = @DateAndTime AND Hall = @Hall)";

            // Удаление сеанса
            string deleteSessionQuery = "DELETE FROM Sessions WHERE NameFilm = @NameFilm AND DateAndTime = @DateAndTime AND Hall = @Hall";

            string deletePlaceQuery = "DELETE FROM BusyPlaces WHERE Session_Id IN (SELECT Session_Id FROM Sessions WHERE NameFilm = @NameFilm AND DateAndTime = @DateAndTime AND Hall = @Hall)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand deleteTicketsCommand = new SqlCommand(deleteTicketsQuery, connection))
                {
                    deleteTicketsCommand.Parameters.AddWithValue("@NameFilm", filmName);
                    deleteTicketsCommand.Parameters.AddWithValue("@DateAndTime", dateAndTime);
                    deleteTicketsCommand.Parameters.AddWithValue("@Hall", hall);
                    deleteTicketsCommand.ExecuteNonQuery();
                }

                using (SqlCommand deletePlaceCommand = new SqlCommand(deletePlaceQuery, connection))
                {
                    deletePlaceCommand.Parameters.AddWithValue("@NameFilm", filmName);
                    deletePlaceCommand.Parameters.AddWithValue("@DateAndTime", dateAndTime);
                    deletePlaceCommand.Parameters.AddWithValue("@Hall", hall);
                    deletePlaceCommand.ExecuteNonQuery();
                }

                using (SqlCommand deleteSessionCommand = new SqlCommand(deleteSessionQuery, connection))
                {
                    deleteSessionCommand.Parameters.AddWithValue("@NameFilm", filmName);
                    deleteSessionCommand.Parameters.AddWithValue("@DateAndTime", dateAndTime);
                    deleteSessionCommand.Parameters.AddWithValue("@Hall", hall);
                    int result = deleteSessionCommand.ExecuteNonQuery();

                    if (result < 0)
                        errorLabel.Content = "Ошибка: Не удалось удалить сеанс из базы данных";
                    else
                    {
                        errorLabel.Foreground = Brushes.Green;
                        errorLabel.Content = "Сеанс успешно удален!";
                        SessionComboBox.Items.Remove(selectedSession);
                    }
                }
            }
        }



        private void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True");
            using (connection)
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT NameFilm, DateAndTime, Hall FROM Sessions", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string filmName = reader["NameFilm"].ToString();
                        DateTime dateAndTime = Convert.ToDateTime(reader["DateAndTime"]);
                        string hall = reader["Hall"].ToString();
                        SessionComboBox.Items.Add($"{filmName}, {dateAndTime}, Зал: {hall}");
                    }
                }
                connection.Close();
            }
        }

    }
}
