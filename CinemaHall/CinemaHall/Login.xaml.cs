//Login.cs
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        private string username;
        private string usersurname;
        public Login()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Registration();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            errorLabel.Foreground = Brushes.Red;

            string Pass = pass.Password;
            string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";
            string Login = login.Text;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlCheckLoginAndPassword = "SELECT COUNT(*) FROM Worker WHERE Login = @Login AND Password = @Password";
                using (SqlCommand commandCheckLoginAndPassword = new SqlCommand(sqlCheckLoginAndPassword, connection))
                {
                    commandCheckLoginAndPassword.Parameters.AddWithValue("@Login", Login);
                    commandCheckLoginAndPassword.Parameters.AddWithValue("@Password", Pass);
                    int userCountLoginAndPassword = (int)commandCheckLoginAndPassword.ExecuteScalar();

                    if (userCountLoginAndPassword > 0)
                    {
                        string sqlQueryName = "SELECT Surname, Name FROM Worker WHERE Login = @Login";
                        using (SqlCommand command = new SqlCommand(sqlQueryName, connection))
                        {
                            command.Parameters.AddWithValue("@Login", Login);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    username = reader.GetString(0);
                                    usersurname = reader.GetString(1);
                                    
                                }
                            }
                            string sqlQueryPost = "SELECT Post FROM Worker WHERE Login = @Login";
                            using (SqlCommand command2 = new SqlCommand(sqlQueryPost, connection))
                            {
                                command2.Parameters.AddWithValue("@Login", Login);
                                using (SqlDataReader reader2 = command2.ExecuteReader())
                                {
                                    if (reader2.Read())
                                    {
                                        string post = reader2.GetString(0);
                                        if (post == "Кассир")
                                        {
                                            this.Content = new Cashier(username, usersurname);
                                        }
                                        else if (post == "Администратор")
                                        {
                                            this.Content = new Admin();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        errorLabel.Content = "Ошибка: Неверный логин или пароль.";
                    }
                }
            }
        }
    }
}
