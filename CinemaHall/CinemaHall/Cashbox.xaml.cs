//CashBox.xaml.cs
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
using System.Windows.Shapes;

namespace CinemaHall
{
    /// <summary>
    /// Логика взаимодействия для Cashbox.xaml
    /// </summary>
    public partial class Cashbox : Window
    {
        private Cashier _cashier;
        double Sum;
        public Cashbox(double summa, Cashier cashier)
        {
            Sum = summa;
            _cashier = cashier;
            InitializeComponent();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            nal n = new nal(Sum,_cashier);
            this.MainContent.Content = n;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            card c = new card(_cashier);
            this.MainContent.Content = c;
        }
    }
}
