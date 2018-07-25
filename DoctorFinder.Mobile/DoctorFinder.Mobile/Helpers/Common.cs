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

        public static String GetRouteUri(double currentLat, double currentLon, double destinationLat, double destinationLon, string mode = "driving")
        {
            String baseUrl = $"https://maps.googleapis.com/maps/api/directions/json";
            StringBuilder strBuilder = new StringBuilder(baseUrl);

            strBuilder.Append("?origin=" + currentLat.ToString() + "," + currentLon.ToString());
            strBuilder.Append("&destination=" + destinationLat.ToString() + "," + destinationLon.ToString());
            strBuilder.Append("&mode=" + mode);
            strBuilder.Append("&key=" + API_KEY);

            return strBuilder.ToString();
        }

        public static String GetDetails(string placeId)
        {
            String baseUrl = $"https://maps.googleapis.com/maps/api/place/details/json";
            StringBuilder strBuilder = new StringBuilder(baseUrl);

            strBuilder.Append("?placeid=" + placeId);
            strBuilder.Append("&fields=name,rating,formatted_phone_number,reviews,formatted_address");
            strBuilder.Append("&key=" + API_KEY);

            return strBuilder.ToString();
        }

        public static String GetDetails(double lat, double lon)
        {
            String baseUrl = $"https://maps.googleapis.com/maps/api/geocode/json";
            StringBuilder strBuilder = new StringBuilder(baseUrl);

            strBuilder.Append("?address=" + lat.ToString() + "," + lon.ToString());
            strBuilder.Append("&key=" + API_KEY);

            return strBuilder.ToString();
        }

        public static String GetTravelDetails(double currentLat, double currentLon, double destLat, double destLon, string mode)
        {
            String baseUrl = $"https://maps.googleapis.com/maps/api/distancematrix/json";
            StringBuilder strBuilder = new StringBuilder(baseUrl);

            strBuilder.Append("?origins=" + currentLat.ToString() + "," + currentLon.ToString());
            strBuilder.Append("&destinations=" + destLat.ToString() + "," + destLon.ToString());
            strBuilder.Append("&mode=" + mode);
            strBuilder.Append("&key=" + API_KEY);

            return strBuilder.ToString();
        }
    }
}