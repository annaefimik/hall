//card.xaml.cs
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

namespace CinemaHall
{
    /// <summary>
    /// Логика взаимодействия для card.xaml
    /// </summary>
    public partial class card : UserControl
    {
        private Cashier _cashier;
        public card(Cashier cashier)
        {
            InitializeComponent();
            _cashier = cashier;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            message.Foreground = Brushes.Red;
            if (number.Text == "" || fio.Text == "" || cvv.Password == "" || password.Password == "")
            {
                message.Content = "Заполните все поля!";
            }
            else
            {
                if (number.Text.Length != 16)
                {
                    message.Content = "Номер карты должен состоять из 16 цифер!";
                }
                else
                {
                    if (cvv.Password.Length != 3)
                    {
                        message.Content = "CVV должен состоять из 3 цифр!";

                    }
                    else
                    {
                        if (password.Password.Length != 4)
                        {
                            message.Content = "Код должен состоять из 4 цифр!";

                        }
                        else
                        {
                            try
                            {
                                long num = Convert.ToInt64(number.Text);
                            }
                            catch {
                                message.Content = "номер карты должен состоять из цифр!";
                                return;
                            }
                            try
                            {
                                string name = Convert.ToString(fio.Text);
                            }
                            catch
                            {
                                message.Content = "ФИО должно состоять из букв!";
                                return;
                            }
                            /*aaaa*/
                            try
                            {
                                int CVV = Convert.ToInt32(cvv.Password);
                            }
                            catch
                            {
                                message.Content = "cvv-код должен cостоять из цифр!";
                                return;
                            }
                            try
                            {
                                int code = Convert.ToInt32(password.Password);
                            }
                            catch
                            {
                                message.Content = "код карты должен состоять из цифр!";
                                return;
                            }
                            _cashier.Payment("Банковская карта");
                            message.Foreground = Brushes.Green;
                            message.Content = "Платёж зарегестрирован. Билет оплачен";
                        }
                    }
                }
            }
        }
    }
}
