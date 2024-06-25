using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaHall
{
    class SoldTickets
    {
        private int ticketNumber;
        private int sessionID;
        private string cashierName;
        private DateTime dateTime;
        private string payMethod;
        private double price;
        private int cashierId;

        public int TicketNumber { get { return ticketNumber; } set { ticketNumber = value; } }
        public int SessionID { get {  return sessionID; } set {  sessionID = value; } }
        public string CashierName { get {  return cashierName; } set {  cashierName = value; } }
        public DateTime DateTime { get { return dateTime; } set { dateTime = value; } }
        public string PayMethod { get {  return payMethod; } set {  payMethod = value; } }
        public double Price { get { return price; } set { price = value; } }
        public int Cashier_Id { get { return cashierId; } set { cashierId = value; } }

        public SoldTickets(int ticketNumber, int sessionID, string cashierName, DateTime dateTime, string payMethod, double price, int cashierId)
        {
            TicketNumber = ticketNumber;
            SessionID = sessionID;
            CashierName = cashierName;
            DateTime = dateTime;
            PayMethod = payMethod;
            Price = price;
            Cashier_Id = cashierId;
           
        }
    }
}
