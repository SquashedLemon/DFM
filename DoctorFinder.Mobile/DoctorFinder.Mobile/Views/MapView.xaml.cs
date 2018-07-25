using Acr.UserDialogs;
using DoctorFinder.Mobile.Globals;
using DoctorFinder.Mobile.Helpers;
using DoctorFinder.Mobile.Models;
using DoctorFinder.Mobile.Models.Places;
using DoctorFinder.Mobile.Views.DetailViews;
using Newtonsoft.Json;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace DoctorFinder.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapView : ContentView
	{
        #region Variable Declarations
        private HttpClient httpClient;
        private RootObject rootObject;
        private Pin CurrentLocation;
        private IProgressDialog progressDialog;
        #endregion

        public MapView ()
		{
			InitializeComponent ();

            new Action(async () =>
            {
                progressDialog = UserDialogs.Instance.Loading("Please wait...");

                mySlider.Value = Convert.ToInt32((GlobalVariables.Radius / 5000) * 100);

                await GetCurrentLocation();

                if (GlobalVariables.Radius > 2000 || GlobalVariables.Radius < 2000)
                {
                    if (GlobalVariables.EstablishmentName == "Hospital")
                    {
                        await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "hospital", GlobalVariables.Radius, "Hospital");
                    }
                    else if (GlobalVariables.EstablishmentName == "Pharmacy")
                    {
                        await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "pharmacy", GlobalVariables.Radius, "Pharmacy");
                    }
                    else if (GlobalVariables.EstablishmentName == "Clinic")
                    {
                        await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "clinic", GlobalVariables.Radius, "Clinic");
                    }
                    else if (GlobalVariables.EstablishmentName == "Doctor")
                    {
                        await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "health", GlobalVariables.Radius, "Doctor");
                    }
                }
                else
                {
                    await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "hospital", GlobalVariables.Radius, "Hospital");
                }

                await GetDirection();

                progressDialog.Hide();
            }).Invoke();

            myMap.PinClicked += MyMap_PinClicked;
            myMap.InfoWindowClicked += MyMap_InfoWindowClicked;
            mySlider.ValueChanged += MySlider_ValueChanged;
            mySlider.StoppedDragging += MySlider_StoppedDragging;
        }

        #region Events
        protected async void MyMap_InfoWindowClicked(object sender, InfoWindowClickedEventArgs e)
        {
            if (GlobalVariables.ObservableEstablishments.Where(x => x.Name == e.Pin.Label).FirstOrDefault() is Establishment selectedPin)
                await Navigation.PushAsync(new HospitalDetailPage(selectedPin));

        }

        protected void MyMap_PinClicked(object sender, PinClickedEventArgs e)
        {
            myMap.SelectedPin = e.Pin;
            myMap.MoveToRegion(MapSpan.FromCenterAndRadius(e.Pin.Position, Distance.FromKilometers(4)));
        }

        protected void MySlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            GlobalVariables.Radius = ((e.NewValue / 100) * 5000);

            double toDisplay = GlobalVariables.Radius / 1000;
            lblMeter.Text = toDisplay.ToString() + "km";

            Circle circle = new Circle()
            {
                Center = new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude),
                Radius = Distance.FromMeters(GlobalVariables.Radius + 500),
                StrokeColor = Color.Blue,
                FillColor = Color.FromRgba(0, 0, 255, 32),
                StrokeWidth = 2f
            };

            myMap.Circles.Clear();
            myMap.Circles.Add(circle);
            myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude), Distance.FromMeters((GlobalVariables.Radius + 500))), true);
        }

        protected async void MySlider_StoppedDragging(object sender, EventArgs e)
        {
            progressDialog = UserDialogs.Instance.Loading("Please wait...");

            myMap.Pins.Clear();

            await GetCurrentLocation();

            if (GlobalVariables.EstablishmentName == "Hospital")
                await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "hospital", GlobalVariables.Radius, "Hospital");
            else if (GlobalVariables.EstablishmentName == "Clinic")
                await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "clinic", GlobalVariables.Radius, "Clinic");
            else if (GlobalVariables.EstablishmentName == "Pharmacy")
                await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "pharmacy", GlobalVariables.Radius, "Pharmacy");
            else if (GlobalVariables.EstablishmentName == "Doctor")
                await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "health", GlobalVariables.Radius, "Doctor");

            await GetDirection();

            progressDialog.Hide();
        }
        #endregion

        #region Methods and Functions
        protected async Task GetCurrentLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(10000));

                GlobalVariables.CurrentLocationLatitude = Convert.ToDouble(position.Latitude);
                GlobalVariables.CurrentLocationLongitude = Convert.ToDouble(position.Longitude);

                httpClient = new HttpClient();

                var response = await httpClient.GetStringAsync(Common.GetDetails(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude));

                var deserializedJson = JsonConvert.DeserializeObject<Models.Location.Rootobject>(response);

                CurrentLocation = new Pin()
                {
                    Icon = BitmapDescriptorFactory.DefaultMarker(Color.Blue),
                    Type = PinType.Place,
                    Label = "Current Location",
                    Address = deserializedJson.results[0].formatted_address,
                    Position = new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude)
                };

                myMap.UiSettings.MapToolbarEnabled = true;
                myMap.UiSettings.MyLocationButtonEnabled = true;
                myMap.UiSettings.MapToolbarEnabled = true;
                myMap.UiSettings.CompassEnabled = true;
                myMap.Pins.Add(CurrentLocation);
                myMap.MoveToRegion(MapSpan.FromCenterAndRadius(CurrentLocation.Position, Distance.FromMeters(2500)), true);
            }
            catch (Exception)
            {
                progressDialog.Hide();

                await UserDialogs.Instance.AlertAsync("Unstable network connection. Please reconnect.", "Error", "Dimiss");
            }
        }

        protected async Task PinNearbyEstablishmentByType(double lat, double lon, string establishmentType = "hospital", double radius = 2000, string name = "Hospital")
        {
            httpClient = new HttpClient();

            GlobalVariables.Radius = radius;
            GlobalVariables.EstablishmentType = establishmentType;
            GlobalVariables.EstablishmentName = name;

            try
            {
                var response = await httpClient.GetStringAsync(Common.GetLocationUri(lat, lon, Convert.ToInt32(GlobalVariables.Radius), GlobalVariables.EstablishmentType, GlobalVariables.EstablishmentName));

                var deserializedJson = JsonConvert.DeserializeObject<RootObject>(response);

                rootObject = deserializedJson;

                var resultStatus = rootObject.status;

                if (resultStatus.Contains("ok".ToUpper()))
                {
                    var resultList = rootObject.results;

                    GlobalVariables.CurrentResults = resultList;

                    foreach (var result in resultList)
                    {
                        double tempLat = result.geometry.location.lat;
                        double tempLon = result.geometry.location.lng;

                        Pin myPin = new Pin()
                        {
                            Type = PinType.Place,
                            Label = result.name,
                            Address = result.vicinity,
                            Position = new Position(tempLat, tempLon)
                        };

                        myMap.Pins.Add(myPin);
                    }

                    myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude), Distance.FromMeters(radius + 800)), true);
                }
            }
            catch (Exception)
            {
                progressDialog.Hide();

                await UserDialogs.Instance.AlertAsync("Unstable network connection. Please reconnect.", "Error", "Dismiss");
            }
        }

        protected async Task GetDirection()
        {
            List<Establishment> establishmentList = new List<Establishment>();

            try
            {
                httpClient = new HttpClient();

                foreach (var result in GlobalVariables.CurrentResults)
                {
                    var desLt = result.geometry.location.lat;
                    var destLn = result.geometry.location.lng;

                    var response = await httpClient.GetStringAsync(Common.GetRouteUri(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude, desLt, destLn));

                    var deserializedJson = JsonConvert.DeserializeObject<Models.Directions.RootObject>(response);

                    var routes = deserializedJson.routes.ToList();

                    establishmentList.Add(new Establishment()
                    {
                        Name = result.name,
                        Distance = routes[0].legs[0].distance.text,
                        DistanceValue = routes[0].legs[0].duration.value,
                        TravelTime = routes[0].legs[0].duration.text,
                        Vicinity = result.vicinity,
                        Latitude = result.geometry.location.lat,
                        Longitude = result.geometry.location.lng,
                        PlaceId = result.place_id
                    });
                }

                GlobalVariables.ObservableEstablishments = new ObservableCollection<Establishment>(establishmentList);
            }
            catch (HttpRequestException)
            {
                await UserDialogs.Instance.AlertAsync("Unstable network connection. Please reconnect.", "Error", "OK");
            }
        }
        #endregion
    }
}