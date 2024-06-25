//Registration.xaml.cs
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
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Diagnostics;


namespace CinemaHall
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : UserControl
    {
        public Registration()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Login();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            errorLabel.Foreground = Brushes.Red;
            if (surname.Text == "" || name.Text == "" || login.Text == "" || email.Text == "" || pass.Password == "" || rep_pass.Password == "" || post.SelectedItem == null)
            {
                errorLabel.Content = "Ошибка: заполните все поля";
            }
            else
            {
                string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
                Regex regex = new Regex(pattern);
                if (!regex.IsMatch(email.Text))
                {
                    errorLabel.Content = "Ошибка: Неправильно введена почта";
                }
                else
                {
                    if (pass.Password.Length < 8)
                    {
                        errorLabel.Content = "Ошибка: Длина пароля не может быть меньше 8 символов";
                    }
                    else
                    {
                        if (pass.Password != rep_pass.Password)
                        {
                            errorLabel.Content = "Ошибка: Пароли не совпадают";
                        }
                        else
                        {
                            string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();
                                string sqlCheckLogin = "SELECT COUNT(*) FROM Worker WHERE Login = @Login";
                                using (SqlCommand commandCheckLogin = new SqlCommand(sqlCheckLogin, connection))
                                {
                                    commandCheckLogin.Parameters.AddWithValue("@Login", login.Text);
                                   
                                    int userCountLogin = (int)commandCheckLogin.ExecuteScalar();
                                    if (userCountLogin > 0)
                                    {
                                        errorLabel.Content = "Ошибка: Пользователь с таким логином уже существует в базе данных.";
                                    }
                                    else
                                    {
                                        Worker w=new Worker(surname.Text, name.Text, login.Text, pass.Password, email.Text, post.Text);
                                        try
                                        {
                                            string sqlInsert = "INSERT INTO Worker (Surname, Name, Login, Password, Email, Post) VALUES (@Surname, @Name, @Login, @Password, @Email, @Post)";
                                            using (SqlCommand commandInsert = new SqlCommand(sqlInsert, connection))
                                            {
                                                commandInsert.Parameters.AddWithValue("@Surname", w.Surname);
                                                commandInsert.Parameters.AddWithValue("@Name", w.Name);
                                                commandInsert.Parameters.AddWithValue("@Login", w.Login);
                                                commandInsert.Parameters.AddWithValue("@Password", w.Password);
                                                commandInsert.Parameters.AddWithValue("@Email", w.Email);
                                                commandInsert.Parameters.AddWithValue("@Post", w.Post);
                                                commandInsert.ExecuteNonQuery();
                                                errorLabel.Foreground = Brushes.Green;
                                                errorLabel.Content="Регистрация прошла успешно!";
                                                this.Content = new Login();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            errorLabel.Content = "Ошибка: Не удалость добавить пользователя в базу данных!";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }   
    }
}
