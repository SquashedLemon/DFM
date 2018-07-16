using Acr.UserDialogs;
using DoctorFinder.Mobile.Globals;
using DoctorFinder.Mobile.Helpers;
using DoctorFinder.Mobile.Models.Places;
using Newtonsoft.Json;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace DoctorFinder.Mobile.Views.DetailViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapViewPage : ContentPage
    {
        #region Variables
        private HttpClient httpClient;
        private RootObject rootObject;
        private List<Result> resultsList;
        private Pin CurrentLocation;
        private String CurrentAddress = "";
        #endregion

        #region Constructor
        public MapViewPage()
        {
            InitializeComponent();

            new Action(async () =>
            {
                var loading = UserDialogs.Instance.Loading("Please wait...");

                mySlider.Value = Convert.ToInt32((GlobalVariables.Radius / 5000) * 100);

                await GetLocation();


                if (GlobalVariables.Radius > 2000 || GlobalVariables.Radius < 2000)
                {
                    if (GlobalVariables.EstablishmentName == "Hospital")
                    {
                        frmHospital.BackgroundColor = Color.FromHex("#c5cddb");
                        frmClinic.BackgroundColor = Color.White;
                        frmPharmacy.BackgroundColor = Color.White;
                        frmDoctor.BackgroundColor = Color.White;

                        myMap.Pins.Clear();

                        await GetLocation();
                        //await PinNearbyPharmacies(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(CurrentRadius));
                        await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "hospital", GlobalVariables.Radius, "Hospital");

                    }
                    else if (GlobalVariables.EstablishmentName == "Clinic")
                    {
                        frmClinic.BackgroundColor = Color.FromHex("#c5cddb");
                        frmHospital.BackgroundColor = Color.White;
                        frmPharmacy.BackgroundColor = Color.White;
                        frmDoctor.BackgroundColor = Color.White;

                        myMap.Pins.Clear();

                        await GetLocation();
                        //await PinNearbyPharmacies(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(CurrentRadius));
                        await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "clinic", GlobalVariables.Radius, "Clinic");
                    }
                    else if (GlobalVariables.EstablishmentName == "Pharmacy")
                    {
                        frmPharmacy.BackgroundColor = Color.FromHex("#c5cddb");
                        frmHospital.BackgroundColor = Color.White;
                        frmClinic.BackgroundColor = Color.White;
                        frmDoctor.BackgroundColor = Color.White;

                        myMap.Pins.Clear();

                        await GetLocation();
                        //await PinNearbyPharmacies(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(CurrentRadius));
                        await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "pharmacy", GlobalVariables.Radius, "Pharmacy");
                    }
                    else if (GlobalVariables.EstablishmentName == "Doctor")
                    {
                        frmDoctor.BackgroundColor = Color.FromHex("#c5cddb");
                        frmHospital.BackgroundColor = Color.White;
                        frmClinic.BackgroundColor = Color.White;
                        frmPharmacy.BackgroundColor = Color.White;

                        myMap.Pins.Clear();

                        await GetLocation();
                        //await PinNearbyPharmacies(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(CurrentRadius));
                        await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "health", GlobalVariables.Radius, "Doctor");
                    }
                }
                else
                {
                    frmHospital.BackgroundColor = Color.FromHex("#c5cddb");
                    frmClinic.BackgroundColor = Color.White;
                    frmPharmacy.BackgroundColor = Color.White;
                    frmDoctor.BackgroundColor = Color.White;

                    await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "hospital", GlobalVariables.Radius, "Hospital");

                }

                loading.Hide();
            }).Invoke();

            myMap.PinClicked += MyMap_PinClicked;
            myMap.SelectedPinChanged += MyMap_SelectedPinChanged;
            myMap.InfoWindowClicked += MyMap_InfoWindowClicked;
            myMap.InfoWindowLongClicked += MyMap_InfoWindowLongClicked;
            mySlider.ValueChanged += MySlider_ValueChanged;
        }
        #endregion

        #region Events
        protected void MyMap_PinClicked(object sender, PinClickedEventArgs e)
        {

            myMap.SelectedPin = e.Pin;
            myMap.MoveToRegion(MapSpan.FromCenterAndRadius(e.Pin.Position, Xamarin.Forms.GoogleMaps.Distance.FromKilometers(4)));
        }

        protected void MyMap_SelectedPinChanged(object sender, SelectedPinChangedEventArgs e)
        {

        }

        protected void MyMap_InfoWindowClicked(object sender, InfoWindowClickedEventArgs e)
        {
            Navigation.PushAsync(new HospitalDetailPage(new Models.Establishment() { Distance = "kdasd", DistanceValue = 222, Latitude = 23, Longitude = 113, Name = "asda", TravelTime = "dasd", Vicinity = "adsada" }));
        }

        protected void MyMap_InfoWindowLongClicked(object sender, InfoWindowLongClickedEventArgs e)
        {

        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        protected async void MeterPicker_SelectedIndexChanged(object sender, EventArgs e)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            //if (e == null)
            //    return;
            //else
            //{
            //    var loading = UserDialogs.Instance.Loading("Please wait...");

            //    myMap.Pins.Clear();
            //    myMap.Circles.Clear();

            //    await GetLocation();
            //    await PinNearbyHospitals(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(meterPicker.SelectedItem));

            //    loading.Hide();
            //}
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        protected async void CategoryPicker_SelectedIndexChanged(object sender, EventArgs e)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            //if (e == null)
            //    return;
            //else
            //{
            //    var loading = UserDialogs.Instance.Loading("Please wait...");

            //    myMap.Pins.Clear();
            //    myMap.Circles.Clear();

            //    await GetLocation();

            //    if (categoryPicker.SelectedItem.ToString() == "Hospital")
            //    {
            //        GlobalVariables.Establishment = categoryPicker.SelectedItem.ToString();
            //        await PinNearbyHospitals(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(meterPicker.SelectedItem));
            //    }
            //    else if (categoryPicker.SelectedItem.ToString() == "Pharmacy")
            //    {
            //        GlobalVariables.Establishment = categoryPicker.SelectedItem.ToString();
            //        await PinNearbyPharmacies(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(meterPicker.SelectedItem));
            //    }

            //    loading.Hide();
            //}
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        protected async void MySlider_ValueChanged(object sender, ValueChangedEventArgs e)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            GlobalVariables.Radius = ((e.NewValue / 100) * 5000);
            //await Task.Delay(3000);

            //var loading = UserDialogs.Instance.Loading("Please wait...");

            //myMap.Pins.Clear();
            //myMap.Circles.Clear();

            //await GetLocation();

            //if (categoryPicker.SelectedItem.ToString() == "Hospital")
            //{
            //    GlobalVariables.Establishment = categoryPicker.SelectedItem.ToString();
            //    await PinNearbyHospitals(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(e.NewValue));
            //}
            //else if (categoryPicker.SelectedItem.ToString() == "Pharmacy")
            //{
            //    GlobalVariables.Establishment = categoryPicker.SelectedItem.ToString();
            //    await PinNearbyPharmacies(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(e.NewValue));
            //}

            //loading.Hide();
            double toDisplay = GlobalVariables.Radius / 1000;
            lblMeter.Text = toDisplay.ToString() + "km";

            Circle circle = new Circle()
            {
                Center = new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude),
                Radius = Xamarin.Forms.GoogleMaps.Distance.FromMeters(GlobalVariables.Radius + 500),
                StrokeColor = Color.Blue,
                FillColor = Color.FromRgba(0, 0, 255, 32),
                StrokeWidth = 2f
            };

            myMap.Circles.Clear();
            myMap.Circles.Add(circle);
            myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude), Xamarin.Forms.GoogleMaps.Distance.FromMeters((GlobalVariables.Radius + 500))), true);
        }

        protected async void FrmHospital_Tapped(object sender, EventArgs e)
        {
            frmHospital.BackgroundColor = Color.FromHex("#c5cddb");
            frmClinic.BackgroundColor = Color.White;
            frmPharmacy.BackgroundColor = Color.White;
            frmDoctor.BackgroundColor = Color.White;

            var loading = UserDialogs.Instance.Loading("Please wait...");

            myMap.Pins.Clear();

            await GetLocation();
            //await PinNearbyPharmacies(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(CurrentRadius));
            await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "hospital", GlobalVariables.Radius);

            loading.Hide();
        }

        protected async void FrmClinic_Tapped(object sender, EventArgs e)
        {
            frmClinic.BackgroundColor = Color.FromHex("#c5cddb");
            frmHospital.BackgroundColor = Color.White;
            frmPharmacy.BackgroundColor = Color.White;
            frmDoctor.BackgroundColor = Color.White;

            var loading = UserDialogs.Instance.Loading("Please wait...");

            myMap.Pins.Clear();

            await GetLocation();
            //await PinNearbyPharmacies(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(CurrentRadius));
            await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "clinic", GlobalVariables.Radius, "Clinic");

            loading.Hide();
        }

        protected async void FrmPharmacy_Tapped(object sender, EventArgs e)
        {
            frmPharmacy.BackgroundColor = Color.FromHex("#c5cddb");
            frmHospital.BackgroundColor = Color.White;
            frmClinic.BackgroundColor = Color.White;
            frmDoctor.BackgroundColor = Color.White;

            var loading = UserDialogs.Instance.Loading("Please wait...");

            myMap.Pins.Clear();

            await GetLocation();
            //await PinNearbyPharmacies(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(CurrentRadius));
            await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "pharmacy", GlobalVariables.Radius, "Pharmacy");

            loading.Hide();
        }

        protected async void FrmDoctor_Tapped(object sender, EventArgs e)
        {
            frmDoctor.BackgroundColor = Color.FromHex("#c5cddb");
            frmHospital.BackgroundColor = Color.White;
            frmClinic.BackgroundColor = Color.White;
            frmPharmacy.BackgroundColor = Color.White;

            var loading = UserDialogs.Instance.Loading("Please wait...");

            myMap.Pins.Clear();

            await GetLocation();
            //await PinNearbyPharmacies(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, Convert.ToInt32(CurrentRadius));
            await PinNearbyEstablishmentByType(CurrentLocation.Position.Latitude, CurrentLocation.Position.Longitude, "health", GlobalVariables.Radius, "Doctor");

            loading.Hide();
        }
        #endregion

        #region Methods and Functions
        //Get Current Location of the User based on latitude and longitude respectively
        protected async Task GetLocation()
        {
            StringBuilder strbuilder;
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 100;

            var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(10000));

            GlobalVariables.CurrentLocationLatitude = Convert.ToDouble(position.Latitude.ToString());
            GlobalVariables.CurrentLocationLongitude = Convert.ToDouble(position.Longitude.ToString());

            var address = await locator.GetAddressesForPositionAsync(new Plugin.Geolocator.Abstractions.Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude));

            strbuilder = new StringBuilder(CurrentAddress);

            strbuilder.Append(address.FirstOrDefault().SubThoroughfare + " ");
            strbuilder.Append(address.FirstOrDefault().Thoroughfare + ", ");
            strbuilder.Append(address.FirstOrDefault().SubLocality + ", ");
            strbuilder.Append(address.FirstOrDefault().Locality + ", ");
            strbuilder.Append(address.FirstOrDefault().SubAdminArea + ", ");
            strbuilder.Append(address.FirstOrDefault().AdminArea + ", ");
            strbuilder.Append(address.FirstOrDefault().CountryName + ", ");
            strbuilder.Append(address.FirstOrDefault().CountryCode);

            CurrentLocation = new Pin()
            {
                Icon = BitmapDescriptorFactory.DefaultMarker(Color.Blue),
                Type = PinType.Place,
                Label = "Current Location",
                Position = new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude)
            };

            myMap.UiSettings.MapToolbarEnabled = true;
            myMap.UiSettings.MyLocationButtonEnabled = true;
            myMap.UiSettings.RotateGesturesEnabled = true;
            myMap.UiSettings.MyLocationButtonEnabled = true;
            myMap.UiSettings.IndoorLevelPickerEnabled = true;
            myMap.UiSettings.MapToolbarEnabled = true;
            myMap.UiSettings.ScrollGesturesEnabled = true;
            myMap.UiSettings.ZoomControlsEnabled = true;
            myMap.UiSettings.CompassEnabled = true;
            myMap.Pins.Add(CurrentLocation);
            myMap.SelectedPin = CurrentLocation;
            myMap.MoveToRegion(MapSpan.FromCenterAndRadius(CurrentLocation.Position, Xamarin.Forms.GoogleMaps.Distance.FromMeters(2500)), true);
            //mySlider.Value = 2500;
        }

        //Get the list of hospitals within 2 kilometer range
        protected async Task PinNearbyHospitals(double lat, double lon, int radius = 2000)
        {
            httpClient = new HttpClient();

            GlobalVariables.Radius = radius;

            var response = await httpClient.GetStringAsync(Common.GetLocationUri(lat, lon, Convert.ToInt32(GlobalVariables.Radius)));

            var deserializedJson = JsonConvert.DeserializeObject<RootObject>(response);

            rootObject = deserializedJson;

            var resultStatus = rootObject.status;

            if (resultStatus.Contains("ok".ToUpper()))
            {
                resultsList = rootObject.results;

                foreach (var item in resultsList)
                {
                    double tempLat = item.geometry.location.lat;
                    double tempLon = item.geometry.location.lng;

                    Pin myPin = new Pin()
                    {
                        Type = PinType.Place,
                        Label = item.name,
                        Address = item.vicinity,
                        Position = new Position(tempLat, tempLon),
                    };

                    myMap.Pins.Add(myPin);

                    double temp = radius + 500;

                    myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude), Xamarin.Forms.GoogleMaps.Distance.FromMeters(temp)), true);
                }

                Circle circle = new Circle()
                {
                    Center = new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude),
                    Radius = Xamarin.Forms.GoogleMaps.Distance.FromMeters(radius),
                    StrokeColor = Color.Blue,
                    FillColor = Color.FromRgba(0, 0, 255, 32),
                    StrokeWidth = 2f
                };

                myMap.Circles.Add(circle);
            }
            else if (resultStatus.Contains("zero_results".ToUpper()))
                await UserDialogs.Instance.AlertAsync("No results found", "", "OK");
        }

        //Get the list of Pharmacies within 2 kilometer range
        protected async Task PinNearbyPharmacies(double lat, double lon, double radius = 2000)
        {
            httpClient = new HttpClient();

            GlobalVariables.Radius = radius;

            var response = await httpClient.GetStringAsync(Common.GetLocationUri(lat, lon, Convert.ToInt32(GlobalVariables.Radius), "pharmacy"));

            var deserializedJson = JsonConvert.DeserializeObject<RootObject>(response);

            rootObject = deserializedJson;

            var resultStatus = rootObject.status;

            if (resultStatus.Contains("ok".ToUpper()))
            {
                resultsList = rootObject.results;

                foreach (var item in resultsList)
                {
                    double tempLat = item.geometry.location.lat;
                    double tempLon = item.geometry.location.lng;

                    Pin myPin = new Pin()
                    {
                        Type = PinType.Place,
                        Label = item.name,
                        Address = item.vicinity,
                        Position = new Position(tempLat, tempLon)
                    };

                    myMap.Pins.Add(myPin);

                    double temp = radius + 500;

                    myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude), Xamarin.Forms.GoogleMaps.Distance.FromMeters(temp)), true);
                }

                Circle circle = new Circle()
                {
                    Center = new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude),
                    Radius = Xamarin.Forms.GoogleMaps.Distance.FromMeters(radius),
                    StrokeColor = Color.Blue,
                    FillColor = Color.FromRgba(0, 0, 255, 32),
                    StrokeWidth = 2f
                };

                myMap.Circles.Add(circle);
            }
            else if (resultStatus.Contains("zero_results".ToUpper()))
                await UserDialogs.Instance.AlertAsync("No results found", "", "OK");
        }

        protected async Task PinNearbyEstablishmentByType(double lat, double lon, string establishmentType = "hospital", double radius = 2000, string name = "Hospital")
        {
            httpClient = new HttpClient();

            GlobalVariables.Radius = radius;
            GlobalVariables.EstablishmentType = establishmentType;
            GlobalVariables.EstablishmentName = name;

            var response = await httpClient.GetStringAsync(Common.GetLocationUri(lat, lon, Convert.ToInt32(GlobalVariables.Radius), GlobalVariables.EstablishmentType, GlobalVariables.EstablishmentName));

            var deserializedJson = JsonConvert.DeserializeObject<RootObject>(response);

            rootObject = deserializedJson;

            var resultStatus = rootObject.status;

            if (resultStatus.Contains("ok".ToUpper()))
            {
                resultsList = rootObject.results;

                foreach (var item in resultsList)
                {
                    double tempLat = item.geometry.location.lat;
                    double tempLon = item.geometry.location.lng;

                    Pin myPin = new Pin()
                    {
                        Type = PinType.Place,
                        Label = item.name,
                        Address = item.vicinity,
                        Position = new Position(tempLat, tempLon)
                    };

                    myMap.Pins.Add(myPin);

                    double temp = radius + 500;

                    myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude), Xamarin.Forms.GoogleMaps.Distance.FromMeters(temp)), true);
                }

                //Circle circle = new Circle()
                //{
                //    Center = new Position(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude),
                //    Radius = Xamarin.Forms.GoogleMaps.Distance.FromMeters(radius),
                //    StrokeColor = Color.Blue,
                //    FillColor = Color.FromRgba(0, 0, 255, 32),
                //    StrokeWidth = 2f
                //};

                //myMap.Circles.Add(circle);
            }
            else if (resultStatus.Contains("zero_results".ToUpper()))
                await UserDialogs.Instance.AlertAsync("No results found", "", "OK");
        }
        #endregion
    }
}