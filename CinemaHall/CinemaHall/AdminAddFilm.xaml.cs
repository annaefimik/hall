//AdminAddFilm.xaml.cs
using System;
using System.Collections.Generic;
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
using System.IO;
using Microsoft.Win32;
using System.Data.SqlClient;

namespace CinemaHall
{
    /// <summary>
    /// Логика взаимодействия для AdminAddFilm.xaml
    /// </summary>
    public partial class AdminAddFilm : UserControl
    {
        public AdminAddFilm()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (int year = 2024; year >= 1900; year--)
            {
                YearComboBox.Items.Add(year.ToString());
            }
            for (double x = 10; x >= 0; x -= 0.5)
            {
                RateComboBox.Items.Add(x.ToString());
            }
            string[] countries = File.ReadAllLines("C:\\Users\\annae\\source\\repos\\CinemaHall\\CinemaHall\\Сountries.txt");
            foreach (string country in countries)
            {
                CountryComboBox.Items.Add(country);
            }
            string[] janres = File.ReadAllLines("C:\\Users\\annae\\source\\repos\\CinemaHall\\CinemaHall\\Janres.txt");
            foreach (string janre in janres)
            {
                JanreComboBox.Items.Add(janre);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            errorLabel.Foreground = Brushes.Red;

            string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";
            string NameFilm = name.Text;
            int Year = Convert.ToInt32(YearComboBox.SelectedValue);
            double Rate = Convert.ToDouble(RateComboBox.SelectedValue);
            string Direct = director.Text;
            string Country = Convert.ToString(CountryComboBox.SelectedValue);
            string Janre = Convert.ToString(JanreComboBox.SelectedValue);
            string Descript = descript.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlCheckName = "SELECT COUNT(*) FROM Films WHERE NameFilm = @NameFilm";
                using (SqlCommand commandCheckName = new SqlCommand(sqlCheckName, connection))
                {
                    commandCheckName.Parameters.AddWithValue("@NameFilm", NameFilm);
                    int CountName = (int)commandCheckName.ExecuteScalar();

                    if (CountName > 0)
                    {
                        errorLabel.Content = "Ошибка: Фильм с таким названием уже есть.";
                    }
                    else if (NameFilm == "" || Year == null || Rate == null || Direct == "" || Country == "" || Janre == "")
                    {
                        errorLabel.Content = "Ошибка: Заполните все поля.";
                    }
                    else
                    {
                        Films f = new Films(NameFilm, Year, Descript, Rate, Country, Janre, Descript);
                        try
                        {
                            string sqlInsert = "INSERT INTO Films (NameFilm, YearFilm, DescriptionFilm, RateFilm, CountryFilm, JanreFilm, DirectorFilm) VALUES (@NameFilm, @YearFilm, @DescriptionFilm, @RateFilm, @CountryFilm, @JanreFilm, @DirectorFilm)";
                            using (SqlCommand commandInsert = new SqlCommand(sqlInsert, connection))
                            {
                                commandInsert.Parameters.AddWithValue("@NameFilm", f.NameFilm);
                                commandInsert.Parameters.AddWithValue("@YearFilm", f.YearFilm);
                                commandInsert.Parameters.AddWithValue("@RateFilm", f.RateFilm);
                                commandInsert.Parameters.AddWithValue("@DescriptionFilm", f.DescriptionFilm);
                                commandInsert.Parameters.AddWithValue("@CountryFilm", f.CountryFilm);
                                commandInsert.Parameters.AddWithValue("@JanreFilm", f.JanreFilm);
                                commandInsert.Parameters.AddWithValue("@DirectorFilm", f.DirectorFilm);
                                commandInsert.ExecuteNonQuery();
                                errorLabel.Foreground = Brushes.Green;
                                errorLabel.Content = "Фильм успешно добавлен в базу данных.";
                            }
                        }
                        catch (Exception)
                        {
                            errorLabel.Content = "Ошибка: не удалость добавить фильм в базу данных";
                        }
                        

                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            name.Clear();
            YearComboBox.SelectedItem = null;
            RateComboBox.SelectedItem = null;
            director.Clear();
            CountryComboBox.SelectedItem = null;
            JanreComboBox.SelectedItem = null;
            descript.Clear();
        }
    }
}
