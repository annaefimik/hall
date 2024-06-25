//nal.xaml.cs
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
    /// Логика взаимодействия для nal.xaml
    /// </summary>
    public partial class nal : UserControl
    {
        private Cashier _cashier;
        double Sum;
        public nal(double summa, Cashier cashier)
        {
            Sum = summa;
            _cashier = cashier;
            InitializeComponent();
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            SummaBileta.Content = Convert.ToString(Sum)+" byn";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            message.Foreground = Brushes.Red;
            if (vnes.Text == "")
            {
                message.Content = "Необходиммо внести сумму в кассу";
            }
            else
            {
                try { 
                    double v=Convert.ToDouble(vnes.Text);
                    if (v < Sum)
                    {
                        message.Content = "Внесено недостаточно средств";

                    }
                    else
                    {
                        double s = Math.Abs(Sum - v);
                        sdacha.Content = Convert.ToString(s)+" byn";
                        _cashier.Payment("Наличные");
                        message.Foreground = Brushes.Green;
                        message.Content = "Сумма внесена. Билет оплачен";
                    }

                }
                catch {
                    message.Content = ("Внесенная сумма должна быть числом");
                }

            }
        }
    }
}
