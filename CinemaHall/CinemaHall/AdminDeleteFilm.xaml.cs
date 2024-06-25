//AdminDeleteFilm.xaml.cs
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
    /// Логика взаимодействия для AdminDeleteFilm.xaml
    /// </summary>
    public partial class AdminDeleteFilm : UserControl
    {
        public AdminDeleteFilm()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            errorLabel.Foreground = Brushes.Red;

            if (FilmComboBox.SelectedItem == null)
            {
                errorLabel.Content = "Ошибка: Необходимо выбрать фильм для удаления";
                return;
            }

            string selectedFilm = FilmComboBox.SelectedItem.ToString();
            string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";

            // Удаляем билеты с соответствующим ID сеанса из таблицы SoldTicket
            string deleteTicketsQuery = "DELETE FROM SoldTickets WHERE Session_Id IN (SELECT Session_Id FROM Sessions WHERE NameFilm = @NameFilm)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteTicketsQuery, connection))
                {
                    command.Parameters.AddWithValue("@NameFilm", selectedFilm);
                    connection.Open();
                    int ticketsResult = command.ExecuteNonQuery();
                    if (ticketsResult < 0)
                    {
                        errorLabel.Content = "Ошибка: Не удалось удалить билеты из базы данных.";
                        return;
                    }
               
                }
            }

            string deletePlaceQuery = "DELETE FROM BusyPlaces WHERE Session_Id IN (SELECT Session_Id FROM Sessions WHERE NameFilm = @NameFilm)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(deletePlaceQuery, connection))
                {
                    command.Parameters.AddWithValue("@NameFilm", selectedFilm);
                    connection.Open();
                    int ticketsResult = command.ExecuteNonQuery();
                    if (ticketsResult < 0)
                    {
                        errorLabel.Content = "Ошибка: Не удалось удалить место из базы данных.";
                        return;
                    }
                }
            }

            // Теперь удаляем сеансы с соответствующим названием фильма
            string deleteSessionsQuery = "DELETE FROM Sessions WHERE NameFilm = @NameFilm";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteSessionsQuery, connection))
                {
                    command.Parameters.AddWithValue("@NameFilm", selectedFilm);
                    connection.Open();
                    int sessionsResult = command.ExecuteNonQuery();
                    if (sessionsResult < 0)
                    {
                        errorLabel.Content = "Ошибка: Не удалось удалить сеансы из базы данных.";
                        return;
                    }
                }
            }

            // Теперь удаляем сам фильм
            string deleteFilmQuery = "DELETE FROM Films WHERE NameFilm = @NameFilm";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteFilmQuery, connection))
                {
                    command.Parameters.AddWithValue("@NameFilm", selectedFilm);
                    connection.Open();
                    int filmResult = command.ExecuteNonQuery();
                    if (filmResult < 0)
                    {
                        errorLabel.Content = "Ошибка: Не удалось удалить фильм из базы данных.";
                    }
                    else
                    {
                        errorLabel.Foreground = Brushes.Green;
                        errorLabel.Content = "Фильм успешно удален!";
                        FilmComboBox.Items.Remove(selectedFilm);
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
        }
    }
}
