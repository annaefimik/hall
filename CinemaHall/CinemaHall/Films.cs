using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows;

namespace CinemaHall
{
    class Films
    {
        private string nameFilm;
        private int yearFilm;
        private string descriptionFilm;
        private double rateFilm;
        private string countryFilm;
        private string janreFilm;
        private string directorfilm;

        public string NameFilm { get { return nameFilm; } set { nameFilm = value; } }
        public int YearFilm { get { return yearFilm; } set {  yearFilm = value; } }
        public string DescriptionFilm { get { return descriptionFilm; } set {  descriptionFilm = value; } }
        public double RateFilm { get { return rateFilm; } set {  rateFilm = value; } }
        public string CountryFilm { get { return countryFilm; } set {  countryFilm = value; } }
        public string JanreFilm { get { return janreFilm; } set {  janreFilm = value; } }
        public string DirectorFilm { get { return directorfilm; } set {  directorfilm = value; } }

        public Films(string nameFilm, int yearFilm, string descriptionFilm, double rateFilm, string countryFilm, string janreFilm, string directorfilm)
        {
            NameFilm = nameFilm;
            YearFilm = yearFilm;
            DescriptionFilm = descriptionFilm;
            RateFilm = rateFilm;
            CountryFilm = countryFilm;
            JanreFilm = janreFilm; 
            DirectorFilm = directorfilm;
        }

        
    }
}
