using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;

using DoctorFinder.Mobile.Globals;
using DoctorFinder.Mobile.Helpers;
using DoctorFinder.Mobile.Models;
using DoctorFinder.Mobile.Views.DetailViews;
using Newtonsoft.Json;
using Acr.UserDialogs;
using Plugin.Geolocator;

namespace DoctorFinder.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        #region Variables
        private HttpClient httpClient;
        private Models.Places.RootObject placesRootObject;
        private List<Models.Places.Result> placesResultList;
        private Pin CurrentLocation;
        private String CurrentAddress = "";
        private double CurrentRadius = 2000;
        #endregion

        #region Constructor
        public MainTabbedPage()
        {
            InitializeComponent();

            var mytask1 = Task.Run(() => Children.Add(new MapViewPage() { Title = "Map View", Icon = "map.png" }));
            var mytask2 = Task.Run(() => Children.Add(new HospitalListPage() { Title = "List View", Icon = "listBlack.png" }));

            new Action(async () =>
            {
                await mytask1;
                await mytask2;
            }).Invoke();
            //Children.Add(new MapViewPage() { Title = "Map View", Icon = "map.png" });
            //Children.Add(new HospitalListPage() { Title = "List View", Icon = "listBlack.png" });
        }
        #endregion

        #region Methods and Functions
        protected async Task GetLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 100;

            var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(10000));

            GlobalVariables.CurrentLocationLatitude = Convert.ToDouble(position.Latitude.ToString());
            GlobalVariables.CurrentLocationLongitude = Convert.ToDouble(position.Longitude.ToString());

            var address = await locator.GetAddressesForPositionAsync(new Plugin.Geolocator.Abstractions.Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude));

            CurrentLocation = new Pin()
            {
                Icon = BitmapDescriptorFactory.DefaultMarker(Color.Blue),
                Type = PinType.Place,
                Label = "Current Location",
                Position = new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude)
            };
        }

        protected async Task<List<Pin>> PinListNearbyEstablishments(double lat, double lon, string establishmentType = "hospital", double radius = 2000, string name = "Hospital")
        {
            List<Pin> listPin = new List<Pin>();
            httpClient = new HttpClient();

            GlobalVariables.Radius = radius;
            GlobalVariables.EstablishmentType = establishmentType;
            GlobalVariables.EstablishmentName = name;

            var response = await httpClient.GetStringAsync(Common.GetLocationUri(lat, lon, Convert.ToInt32(GlobalVariables.Radius), GlobalVariables.EstablishmentType, GlobalVariables.EstablishmentName));

            var deserializedJson = JsonConvert.DeserializeObject<Models.Places.RootObject>(response);

            placesRootObject = deserializedJson;

            var resultStatus = placesRootObject.status;

            if (resultStatus.Contains("ok".ToUpper()))
            {
                placesResultList = placesRootObject.results;

                foreach (var item in placesResultList)
                {
                    double tempLat = item.geometry.location.lat;
                    double tempLon = item.geometry.location.lng;

                    listPin.Add(new Pin()
                    {
                        Type = PinType.Place,
                        Label = item.name,
                        Address = item.vicinity,
                        Position = new Position(tempLat, tempLon)
                    });
                }
            }
            else if (resultStatus.Contains("zero_results".ToUpper()))
            {
                await UserDialogs.Instance.AlertAsync("No results found", "", "OK");
            }

            return listPin;
        }
        #endregion
    }
}