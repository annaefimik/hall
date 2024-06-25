//Cashier.xaml.cs
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CinemaHall
{
    /// <summary>
    /// Логика взаимодействия для Cashier.xaml
    /// </summary>
    public partial class Cashier : UserControl
    {
        private string username;
        private string usersurname;
        public Cashier(string un, string us)
        {
            username = un;
            usersurname = us;
            InitializeComponent();
        }
        public Cashier()
        {
            InitializeComponent();
        }
        public void Payment(string method)
        {
            approve.Text = method;
            this.UpdateLayout();

        }
        public int GetSessionId(string selectedSession)
        {

            if (SessionComboBox.SelectedItem != null)
            {
                string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string[] sessionDetails = selectedSession.Split(',');
                    string filmName = sessionDetails[0].Trim();
                    DateTime dateAndTime = DateTime.Parse(sessionDetails[1].Trim());
                    string hall = sessionDetails[2].Split(':')[1].Trim();
                    using (SqlCommand command = new SqlCommand("SELECT Session_ID FROM Sessions WHERE NameFilm = @NameFilm AND DateAndTime = @DateAndTime AND Hall = @Hall", connection))
                    {
                        command.Parameters.AddWithValue("@NameFilm", filmName);
                        command.Parameters.AddWithValue("@DateAndTime", dateAndTime);
                        command.Parameters.AddWithValue("@Hall", hall);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        else
                        {
                            throw new Exception("Сеанс не найден");
                        }
                    }
                }
            }
            else { return 0; }
        }
        public int GetCashierId()
        {
            string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Id FROM Worker WHERE Surname = @Surname AND Name = @Name", connection))
                {
                    command.Parameters.AddWithValue("@Surname", username);
                    command.Parameters.AddWithValue("@Name", usersurname);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    else
                    {
                        throw new Exception("Кассир не найден");
                    }
                }
            }
        }

        public bool IsSeatFree(Button b)
        {
            if (SessionComboBox.SelectedItem != null)
            {
                string NumOfPlace = Convert.ToString(b.Content);
                string[] parts = NumOfPlace.Split('-');
                string row = parts[0];
                string seat = parts[1];
                string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";
                int Session_ID = GetSessionId(Convert.ToString(SessionComboBox.SelectedValue));
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM BusyPlaces WHERE Session_ID = @Session_ID AND NumOfPlace = @NumOfPlace", connection))
                    {
                        command.Parameters.AddWithValue("@Session_ID", Session_ID);
                        command.Parameters.AddWithValue("@NumOfPlace", NumOfPlace);
                        int count = (int)command.ExecuteScalar();
                        if (count > 0)
                        {
                            b.Visibility = Visibility.Hidden;
                            b.UpdateLayout();
                            return false;
                        }
                        else
                        {
                            b.Visibility = Visibility.Visible;
                            b.Background = Brushes.White;
                            b.Foreground = Brushes.Black;
                            b.UpdateLayout();
                            return true;
                        }
                    }
                }
            }
            else { return false; }
        }

        private void seat1_1_Click(object sender, RoutedEventArgs e)
        {
            if (SessionComboBox.SelectedItem != null)
            {
                Button seatButton = (Button)sender;
                if (IsSeatFree(seatButton))
                {
                    string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        int Session_ID = GetSessionId(Convert.ToString(SessionComboBox.SelectedValue));
                        using (SqlCommand command = new SqlCommand("SELECT Price FROM Sessions WHERE Session_ID = @Session_ID", connection))
                        {
                            command.Parameters.AddWithValue("@Session_ID", Session_ID);
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                price.Content = reader["Price"].ToString() + " byn";
                            }
                        }
                    }
                    string NumOfPlace = Convert.ToString(seatButton.Content);
                    string[] parts = NumOfPlace.Split('-');
                    string row = parts[0];
                    string seat = parts[1];
                    SeatsListBox.Content = $"Ряд {row} Место {seat}";
                    pay.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MessageBox.Show("Необходимо выбрать сеанс");
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            message.Foreground = Brushes.Red;
            if (FilmComboBox.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать фильм");
                return;
            }
            if (SessionComboBox.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать сеанс");
                return;
            }
            string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";
            int Session_Id = GetSessionId(Convert.ToString(SessionComboBox.SelectedValue));
            string listBoxItem = Convert.ToString(SeatsListBox.Content);
            string[] parts = listBoxItem.Split(' ');
            string row = parts[1];
            string seat = parts[3];
            string NumOfPlace = $"{row}-{seat}";
            string CashierName = Convert.ToString(cashierName.Content);
            if (approve.Text == "Не оплачен")
            { message.Content = "Ошибка! не произведена оплата билета"; return; }
            string PayMethod = Convert.ToString(approve.Text);
            DateTime DateAndTimeOfSale = DateTime.Now;
            string priceWithCurrency = Convert.ToString(price.Content);
            string priceWithoutCurrency = priceWithCurrency.Replace(" byn", "");
            double Price = Convert.ToDouble(priceWithoutCurrency);
            Random random = new Random();
            bool numberExists = true;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int TicketNumber = 0;
                while (numberExists)
                {
                    TicketNumber = random.Next(10000, 100000);
                    using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM SoldTickets WHERE TicketNumber = @TicketNumber", connection))
                    {
                        command.Parameters.AddWithValue("@TicketNumber", TicketNumber);
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        connection.Close();
                        if (count == 0)
                        {
                            numberExists = false;
                        }
                    }
                }
                int Cashier_Id = GetCashierId();
                SoldTickets st = new SoldTickets(TicketNumber, Session_Id, CashierName, DateAndTimeOfSale, PayMethod, Price, Cashier_Id);
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO SoldTickets (TicketNumber, Session_ID, CashierName, DateAndTimeOfSale, PayMethod, Price, Cashier_Id) VALUES (@TicketNumber, @Session_ID, @CashierName, @DateAndTimeOfSale, @PayMethod, @Price, @Cashier_Id)", connection))
                {
                    command.Parameters.AddWithValue("@TicketNumber", st.TicketNumber);
                    command.Parameters.AddWithValue("@Session_ID", st.SessionID);
                    command.Parameters.AddWithValue("@CashierName", st.CashierName);
                    command.Parameters.AddWithValue("@DateAndTimeOfSale", st.DateTime);
                    command.Parameters.AddWithValue("@PayMethod", st.PayMethod);
                    command.Parameters.AddWithValue("@Price", st.Price);
                    command.Parameters.AddWithValue("@Cashier_Id", st.Cashier_Id);
                    command.ExecuteNonQuery();
                }
                BusyPlaces bp = new BusyPlaces(NumOfPlace, Session_Id);
                using (SqlCommand command = new SqlCommand("INSERT INTO BusyPlaces (NumOfPlace, Session_ID) VALUES (@NumOfPlace, @Session_ID)", connection))
                {
                    command.Parameters.AddWithValue("@NumOfPlace", bp.NumOfPlace);
                    command.Parameters.AddWithValue("@Session_ID", bp.SessionID);
                    int result = command.ExecuteNonQuery();
                    if (result < 0)
                    {
                        Console.WriteLine("Ошибка при добавлении в базу данных.");
                    }
                    else
                    {
                        Document document = new Document(PageSize.A7); // Используем формат страницы A7
                        string outputPath = @"C:\Users\annae\Desktop\чек" + Convert.ToString(st.TicketNumber) + ".pdf";
                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
                        document.Open();
                        BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                        Font titleFont = new Font(baseFont, 14, Font.BOLD);
                        Font contentFont = new Font(baseFont, 12, Font.NORMAL);
                        PdfPTable table = new PdfPTable(2);
                        table.TotalWidth = 200f; // Уменьшаем ширину таблицы
                        table.LockedWidth = true;
                        table.AddCell(new PdfPCell(new Phrase("Фильм:", titleFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(Convert.ToString(FilmComboBox.SelectedItem), contentFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase("Сеанс:", titleFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(Convert.ToString(SessionComboBox.SelectedItem), contentFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase("Выбранное место:", titleFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(Convert.ToString(SeatsListBox.Content), contentFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase("Стоимость:", titleFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(Convert.ToString(price.Content), contentFont)) { Border = Rectangle.NO_BORDER });
                        document.Add(table);
                        Paragraph greeting = new Paragraph("ПРИЯТНОГО ПРОСМОТРА!", titleFont);
                        greeting.Alignment = Element.ALIGN_CENTER;
                        document.Add(greeting);
                        document.Close();
                        message.Foreground = Brushes.Green;
                        message.Content = "Билет успешно куплен и сохранен на рабочий стол";
                    }
                }
                connection.Close();
            }
        }
        string FormatSessionTime(object sessionTime)
        {
            if (sessionTime is DateTime dateTime)
            {
               
                return dateTime.ToString("d MMMM yyyy, HH:mm");
            }
            else
            {
                return "Неверный формат времени сеанса";
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            date.Content = DateTime.Now;
            cashierName.Content = username + " " + usersurname;

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
        private void FilmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilmComboBox.SelectedItem != null)
            {
                if (SessionComboBox.SelectedItem != null)
                {
                    SessionComboBox.SelectedItem = null;
                }
                string selectedFilm = FilmComboBox.SelectedItem.ToString();
                string connectionString = "Data Source=anica;Initial Catalog=CinemaHall;Integrated Security=True";

                // Получаем текущую дату и время
                DateTime currentTime = DateTime.Now;

                // Запрос для выборки сеансов, которые ещё не прошли
                string selectSessionsQuery = "SELECT NameFilm, DateAndTime, Hall FROM Sessions WHERE NameFilm = @NameFilm AND DateAndTime > @CurrentTime";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(selectSessionsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NameFilm", selectedFilm);
                        command.Parameters.AddWithValue("@CurrentTime", currentTime);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        // Очищаем SessionComboBox перед добавлением новых элементов
                        SessionComboBox.Items.Clear();

                        while (reader.Read())
                        {
                            string filmName = reader["NameFilm"] == DBNull.Value ? "" : reader["NameFilm"].ToString();
                            DateTime dateAndTime = reader["DateAndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DateAndTime"]);
                            string hall = reader["Hall"] == DBNull.Value ? "" : reader["Hall"].ToString();
                            SessionComboBox.Items.Add($"{filmName}, {dateAndTime}, Зал: {hall}");
                        }
                    }
                }
            }
        }




        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FilmComboBox.SelectedItem = null;
            SessionComboBox.SelectedItem = null;
            SeatsListBox.Content = "";
            price.Content = "";
            pay.Visibility = Visibility.Hidden;
            approve.Text = "Не оплачен";
            OK.Visibility = Visibility.Hidden;
            ClearButton.Visibility = Visibility.Hidden;
            message.Foreground = Brushes.Red;
            message.Content="";

        }

        private void SessionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SessionComboBox.SelectedItem != null)
            {
                SeatsListBox.Content = "";
                price.Content = "";
                foreach (StackPanel row in seats.Children)
                {
                    foreach (Button seatButton in row.Children)
                    {
                        IsSeatFree(seatButton);
                    }
                }
            }
            else { return; }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            this.Content = l;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string priceWithCurrency = Convert.ToString(price.Content);
            string priceWithoutCurrency = priceWithCurrency.Replace(" byn", "");
            double Price = Convert.ToDouble(priceWithoutCurrency);
            Cashbox c = new Cashbox(Price, this);
            c.Show();
        }
        private void approve_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (approve.Text != "Не оплачен")
            {
                OK.Visibility = Visibility.Visible;
                ClearButton.Visibility = Visibility.Visible;
            }
        }
       
        private void CreatePdf(string num)
        {
            try
            {
                // Создаем новый документ
                var doc = new iTextSharp.text.Document();
                // Укажите путь для сохранения PDF файла
                string outputPath = $@"C:/Users/annae/source/repos/CinemaHall/CinemaHall/{num}";
                // Инициализируем PdfWriter для записи в файл
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(outputPath, FileMode.Create));
                // Открываем документ для записи
                doc.Open();
                // Устанавливаем кодировку UTF-8
                BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL);
                // Добавляем содержимое в документ
                doc.Add(new iTextSharp.text.Paragraph("Билет:", font));
                doc.Add(new iTextSharp.text.Paragraph($"Фильм: {Convert.ToString(FilmComboBox.SelectedItem)}", font));
                doc.Add(new iTextSharp.text.Paragraph($"Сеанс: {Convert.ToString(SessionComboBox.SelectedItem)}", font));
                doc.Add(new iTextSharp.text.Paragraph($"Выбранное место: {place.Content}", font));
                doc.Add(new iTextSharp.text.Paragraph($"Стоимость: {price.Content}", font));
                doc.Add(new iTextSharp.text.Paragraph($"ПРИЯТНОГО ПРОСМОТРА!", font));
                // Закрываем документ
                doc.Close();
            }
            catch (Exception ex)
            {
                // Обработка ошибок (например, логирование или возврат пустой строки)
                Console.WriteLine($"Ошибка при создании PDF: {ex.Message}");
            }
        }


    }
}
