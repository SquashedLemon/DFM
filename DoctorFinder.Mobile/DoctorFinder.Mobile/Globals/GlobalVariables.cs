using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DoctorFinder.Mobile.Globals
{
    public class GlobalVariables
    {
        public static double CurrentLocationLatitude;
        public static double CurrentLocationLongitude;
        public static double DestinationLatitude;
        public static double DestinationLongitude;
        public static double Radius = 2000;
        public static string EstablishmentType = "hospital";
        public static string EstablishmentName = "Hospital";
        public static List<Models.Places.Result> CurrentResults;
        public static ObservableCollection<Models.Establishment> ObservableEstablishments;
    }
}