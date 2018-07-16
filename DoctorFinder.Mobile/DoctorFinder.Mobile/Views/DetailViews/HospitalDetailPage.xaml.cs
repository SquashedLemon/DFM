using DoctorFinder.Mobile.Globals;
using DoctorFinder.Mobile.Helpers;
using DoctorFinder.Mobile.Models.Directions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace DoctorFinder.Mobile.Views.DetailViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HospitalDetailPage : ContentPage
	{
        #region Variables
        private HttpClient httpClient;
        //private List<Route> routeList;
        //private Route route;
        private Models.Establishment result;
        private RootObject rootObject;
        #endregion

        #region Constructor
        public HospitalDetailPage(Models.Establishment result)
        {
            InitializeComponent();

            this.result = result;

            txtName.Text = this.result.Name;
            txtVicinity.Text = this.result.Vicinity;
            txtDistance.Text = String.Format("Distance (km): {0}", this.result.Distance);
            txtTravelTime.Text = String.Format("Travel Time: {0}", this.result.TravelTime);
            
            Title = this.result.Name;

            //var isTrue = this.result.opening_hours.open_now;

            //if (isTrue)
            //{
            //    txtOpeningHours.Text = "Open Now";
            //}

            GlobalVariables.DestinationLatitude = this.result.Latitude;
            GlobalVariables.DestinationLongitude = this.result.Longitude;

            double currLt = GlobalVariables.CurrentLocationLatitude;
            double currLn = GlobalVariables.CurrentLocationLongitude;
            double destLt = GlobalVariables.DestinationLatitude;
            double destLn = GlobalVariables.DestinationLongitude;

            new Action(async () =>
            {
                await GetDirection();
            }).Invoke();

            Pin myPin = new Pin()
            {
                Type = PinType.Place,
                Label = "Your destination",
                Position = new Xamarin.Forms.GoogleMaps.Position(GlobalVariables.DestinationLatitude, GlobalVariables.DestinationLongitude)
            };

            myMap.UiSettings.MapToolbarEnabled = true;
            myMap.Pins.Add(myPin);
            myMap.SelectedPin = myPin;
            myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.GoogleMaps.Position(destLt, destLn), Xamarin.Forms.GoogleMaps.Distance.FromMeters(1000)), true);
        }
        #endregion

        #region Events

        #endregion

        #region Methods and Functions
        private List<Location> DecodePolylinePoints(string encodedPoints)
        {
            if (encodedPoints == null || encodedPoints == "")
                return null;
            else
            {
                List<Location> polyLocation = new List<Location>();
                char[] polylineChars = encodedPoints.ToCharArray();
                int index = 0;

                double currentLat = 0;
                double currentLon = 0;
                int nextFiveBits;
                int sum;
                int shifter;

                try
                {
                    while (index < polylineChars.Length)
                    {
                        sum = 0;
                        shifter = 0;

                        do
                        {
                            nextFiveBits = (int)polylineChars[index++] - 63;
                            sum |= (nextFiveBits & 31) << shifter;
                            shifter += 5;
                        }
                        while (nextFiveBits >= 32 && index < polylineChars.Length);

                        if (index >= polylineChars.Length)
                            break;

                        currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                        sum = 0;
                        shifter = 0;

                        do
                        {
                            nextFiveBits = (int)polylineChars[index++] - 63;
                            sum |= (nextFiveBits & 31) << shifter;
                            shifter += 5;
                        }
                        while (nextFiveBits >= 32 && index < polylineChars.Length);

                        if (index >= polylineChars.Length)
                            break;

                        currentLon += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                        Location pointLocation = new Location()
                        {
                            lat = currentLat / 100000.0,
                            lng = currentLon / 100000.0
                        };

                        polyLocation.Add(pointLocation);
                    }
                }
                catch (Exception)
                {
                    //Debug.Write(ex.Message);
                }

                return polyLocation;
            }
        }

        private async Task GetDirection()
        {
            httpClient = new HttpClient();

            double currLat = GlobalVariables.CurrentLocationLatitude;
            double currLon = GlobalVariables.CurrentLocationLongitude;
            double destLat = GlobalVariables.DestinationLatitude;
            double destLon = GlobalVariables.DestinationLongitude;

            var response = await httpClient.GetStringAsync(Common.GetRouteUri(currLat, currLon, destLat, destLon));

            var deserializedJson = JsonConvert.DeserializeObject<RootObject>(response);

            rootObject = deserializedJson;

            List<Route> myRoute = rootObject.routes;

            if (rootObject.status.Contains("ok".ToUpper()))
            {
                //route = rootObject.

                var overviewPolyline = myRoute[0].overview_polyline.points;

                Xamarin.Forms.GoogleMaps.Polyline polyline = new Xamarin.Forms.GoogleMaps.Polyline();

                polyline.Positions.Add(new Xamarin.Forms.GoogleMaps.Position(currLat, currLon));

                foreach (var item in DecodePolylinePoints(overviewPolyline))
                {
                    polyline.Positions.Add(new Xamarin.Forms.GoogleMaps.Position(item.lat, item.lng));
                }

                polyline.Positions.Add(new Xamarin.Forms.GoogleMaps.Position(destLat, destLon));
                polyline.StrokeWidth = 4;

                myMap.Polylines.Add(polyline);
            }
            //else
            //    Debug.Write("Request failed.");
        }
        #endregion
    }
}