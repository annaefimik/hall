using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaHall
{
    class Sessions
    {
        private string nameFilm;
        private DateTime dateTime;
        private int hall;
        private double price;
        private double duration;

        public string NameFilm { get { return nameFilm; } set { nameFilm = value; } }
        public DateTime DateTime { get { return dateTime; } set {  dateTime = value; } }
        public int Hall { get {  return hall; } set {  hall = value; } }
        public double Price { get { return price; } set { price = value; } }
        public double Duration { get { return duration; } set { duration = value; } }

        public Sessions(string nameFilm, DateTime dateTime, int hall, double price, double duration)
        {
            NameFilm = nameFilm;
            DateTime = dateTime;
            Hall = hall;
            Price = price;
            Duration = duration;
          
        }


    }
}
