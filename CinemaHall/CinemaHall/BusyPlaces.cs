using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaHall
{
    class BusyPlaces
    {
        private string numOfPlace;
        private int sessionID;

        public string NumOfPlace { get { return numOfPlace; } set { numOfPlace = value; } }
        public int SessionID { get {  return sessionID; } set {  sessionID = value; } }

        public BusyPlaces(string numOfPlace, int sessionID)
        {
            NumOfPlace = numOfPlace;
            SessionID = sessionID;
        
        }
    }
}
