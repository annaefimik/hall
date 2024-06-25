//Otchet.xaml.cs
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
using System.Data.SqlClient;

using System.Data;


namespace CinemaHall
{
    /// <summary>
    /// Логика взаимодействия для Otchet.xaml
    /// </summary>
    public partial class Otchet : UserControl
    {
        string tabl;
        string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";
        private SqlDataAdapter _dataAdapter;
        private DataTable _dataTable;

        public Otchet(string table)
        {
            InitializeComponent();
            tabl=table; 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (tabl == "Количество")
            {
                name.Content = "Отчёт по количеству";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    int filmCount = GetRecordCount("Films", connection);
                    int sessionCount = GetRecordCount("Sessions", connection);
                    int ticketCount = GetRecordCount("SoldTickets", connection);
                    int workerCount = GetRecordCount("Worker", connection);

                    _dataTable = new DataTable();
                    _dataTable.Columns.Add("Количество фильмов", typeof(int));
                    _dataTable.Columns.Add("Количество сеансов", typeof(int));
                    _dataTable.Columns.Add("Количество проданных билетов", typeof(int));
                    _dataTable.Columns.Add("Количество работников", typeof(int));

                    _dataTable.Rows.Add(filmCount, sessionCount, ticketCount, workerCount);

                    dataGrid.ItemsSource = _dataTable.DefaultView;
                }
            }
            if (tabl == "Билеты")
            {
                name.Content = "Отчёт по проданным билетам";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    const string sql = @"
        SELECT TicketNumber AS 'Номер билета', 
               Session_Id AS 'ID сеанса', 
               CashierName AS 'Имя кассира', 
               DateAndTimeOfSale AS 'Дата и время продажи', 
               PayMethod AS 'Способ оплаты', 
               Price AS 'Цена' 
        FROM SoldTickets";

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGrid.ItemsSource = dataTable.DefaultView;
                }


            }
            if (tabl == "Работа")
            {
                name.Content = "Отчёт по работе кассиров";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
        SELECT CashierName AS 'Имя кассира', 
               COUNT(*) AS 'Количество проданных билетов', 
               SUM(Price) AS 'Сумма выручки' 
        FROM SoldTickets 
        GROUP BY CashierName";

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGrid.ItemsSource = dataTable.DefaultView;
                }

            }
        }
        public int GetRecordCount(string tableName, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM {tableName}", connection))
            {
                return (int)command.ExecuteScalar();
            }
        }
    }
}
