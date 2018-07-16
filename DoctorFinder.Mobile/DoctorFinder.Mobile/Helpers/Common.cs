using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorFinder.Mobile.Helpers
{
    public static class Common
    {
        private const String API_KEY = "AIzaSyB11ozn4Li8PC7vtwGPKBEM6yZwfNwtGQg";

        public static String GetLocationUri(double lat, double lon, int radius = 2000, string type = "hospital", string name = "Hospital")
        {
            String baseUrl = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json";
            StringBuilder strBuilder = new StringBuilder(baseUrl);

            strBuilder.Append("?location=" + lat + "%2C" + lon);
            strBuilder.Append("&keyword=" + name);
            strBuilder.Append("&radius=" + radius.ToString());
            strBuilder.Append("&type=" + type);
            strBuilder.Append("&key=" + API_KEY);

            return strBuilder.ToString();
        }

        public static String GetRouteUri(double currentLat, double currentLon, double destinationLat, double destinationLon)
        {
            String baseUrl = $"https://maps.googleapis.com/maps/api/directions/json";
            StringBuilder strBuilder = new StringBuilder(baseUrl);

            strBuilder.Append("?origin=" + currentLat.ToString() + "," + currentLon.ToString());
            strBuilder.Append("&destination=" + destinationLat.ToString() + "," + destinationLon.ToString());
            strBuilder.Append("&mode=driving");
            strBuilder.Append("&key=" + API_KEY);

            return strBuilder.ToString();
        }
    }
}