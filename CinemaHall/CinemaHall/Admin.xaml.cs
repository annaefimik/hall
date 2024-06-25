//Admin.xaml.cs
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
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace CinemaHall
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : UserControl
    {
        public Admin()
        {
            InitializeComponent();
        }
     

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new AdminAddFilm();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new AdminAddSession();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new AdminDeleteFilm();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content=new AdminDeleteSession();
        }
        

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            this.Content = l;   
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            Otchet o = new Otchet("Количество");
            this.MainContent.Content = o;
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            Otchet o = new Otchet("Билеты");
            this.MainContent.Content = o;
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            Otchet o = new Otchet("Работа");
            this.MainContent.Content = o;
        }
    }


}
