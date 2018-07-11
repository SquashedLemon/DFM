using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorFinder.Mobile.Models
{
    public class Establishment
    {
        public string Name { get; set; }
        public string Vicinity { get; set; }
        public string Distance { get; set; }
        public int DistanceValue { get; set; }
        public string TravelTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string EstablishmentTitle
        {
            get
            {
                return String.Format("({0}) {1}", Distance, Name);
            }
        }
    }
}
