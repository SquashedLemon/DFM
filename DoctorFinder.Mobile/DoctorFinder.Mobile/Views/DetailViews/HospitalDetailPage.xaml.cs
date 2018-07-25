using Acr.UserDialogs;
using DoctorFinder.Mobile.Globals;
using DoctorFinder.Mobile.Helpers;
using DoctorFinder.Mobile.Models.Directions;
using Newtonsoft.Json;
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

namespace DoctorFinder.Mobile.Views.DetailViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HospitalDetailPage : ContentPage
	{
        #region Variables
        private HttpClient httpClient;
        //private List<Route> routeList;
        //private Route route;
        private ObservableCollection<Models.PlaceReview.Review> reviewCollection;
        private ObservableCollection<Models.EstablishmentInformation> establishmentInfoCollection;
        private Models.Establishment result;
        private RootObject rootObject;
        private string placeId;
        private IProgressDialog progressDialog = null;
        #endregion

        #region Constructor
        public HospitalDetailPage(Models.Establishment result)
        {
            InitializeComponent();
            
            this.result = result;

            txtName.Text = this.result.Name;
            txtVicinity.Text = this.result.Vicinity;
            txtDistance.Text = String.Format("Distance (km): {0}", this.result.Distance);
            placeId = this.result.PlaceId;

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
                await GetTravelDetails("driving");
                await GetTravelDetails("transit");
                await GetTravelDetails("walking");

                await GetDirection();
                await GetPlaceDetails(placeId);
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
        protected async void FrmDriving_Tapped(object sender, EventArgs e)
        {
            FrmDriving.BackgroundColor = Color.White;
            LblDrivingTime.TextColor = Color.Black;
            ImgCar.Source = "carblack.png";
            FrmTransit.BackgroundColor = Color.Red;
            LblTransitTime.TextColor = Color.White;
            ImgTransit.Source = "train.png";
            FrmWalking.BackgroundColor = Color.Red;
            LblWalkingTime.TextColor = Color.White;
            ImgWalking.Source = "walking.png";

            progressDialog = UserDialogs.Instance.Loading("Please wait...");

            myMap.Polylines.Clear();

            await GetDirection("driving");
        }

        protected async void FrmTransit_Tapped(object sender, EventArgs e)
        {
            FrmDriving.BackgroundColor = Color.Red;
            LblDrivingTime.TextColor = Color.White;
            ImgCar.Source = "car.png";
            FrmTransit.BackgroundColor = Color.White;
            LblTransitTime.TextColor = Color.Black;
            ImgTransit.Source = "trainblack.png";
            FrmWalking.BackgroundColor = Color.Red;
            LblWalkingTime.TextColor = Color.White;
            ImgWalking.Source = "walking.png";

            progressDialog = UserDialogs.Instance.Loading("Please wait...");

            myMap.Polylines.Clear();

            await GetDirection("transit");
        }

        protected async void FrmWalking_Tapped(object sender, EventArgs e)
        {
            FrmDriving.BackgroundColor = Color.Red;
            LblDrivingTime.TextColor = Color.White;
            ImgCar.Source = "car.png";
            FrmTransit.BackgroundColor = Color.Red;
            LblTransitTime.TextColor = Color.White;
            ImgTransit.Source = "train.png";
            FrmWalking.BackgroundColor = Color.White;
            LblWalkingTime.TextColor = Color.Black;
            ImgWalking.Source = "walkingblack.png";

            progressDialog = UserDialogs.Instance.Loading("Please wait...");

            myMap.Polylines.Clear();

            await GetDirection("walking");
        }
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

                }

                return polyLocation;
            }
        }

        private async Task GetDirection(string mode = "driving")
        {
            try
            {
                httpClient = new HttpClient();

                double currLat = GlobalVariables.CurrentLocationLatitude;
                double currLon = GlobalVariables.CurrentLocationLongitude;
                double destLat = GlobalVariables.DestinationLatitude;
                double destLon = GlobalVariables.DestinationLongitude;

                var response = await httpClient.GetStringAsync(Common.GetRouteUri(currLat, currLon, destLat, destLon, mode));

                var deserializedJson = JsonConvert.DeserializeObject<RootObject>(response);

                rootObject = deserializedJson;

                List<Route> myRoute = rootObject.routes;

                if (rootObject.status.Contains("ok".ToUpper()))
                {
                    var overviewPolyline = myRoute[0].overview_polyline.points;

                    Xamarin.Forms.GoogleMaps.Polyline polyline = new Xamarin.Forms.GoogleMaps.Polyline();

                    polyline.Positions.Add(new Position(currLat, currLon));

                    foreach (var item in DecodePolylinePoints(overviewPolyline))
                    {
                        polyline.Positions.Add(new Position(item.lat, item.lng));
                    }

                    polyline.Positions.Add(new Position(destLat, destLon));
                    polyline.StrokeWidth = 4;

                    myMap.Polylines.Add(polyline);
                }

                if (progressDialog != null)
                    progressDialog.Hide();
            }
            catch (NullReferenceException)
            {
                progressDialog.Hide();

                await UserDialogs.Instance.AlertAsync("Not available", "Error", "OK");
            }
            catch (HttpRequestException)
            {
                progressDialog.Hide();

                await UserDialogs.Instance.AlertAsync("Unstable network connection. Please reconnect.", "Error", "OK");
            }
        }

        public async Task GetPlaceDetails(string id)
        {
            httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync(Common.GetDetails(id));

            var deserializedJson = JsonConvert.DeserializeObject<Models.PlaceReview.Rootobject>(response);

            Models.PlaceReview.Rootobject placeReviewRootObject = deserializedJson;

            var reviews = placeReviewRootObject.result.reviews;
            
            if (reviews != null)
            {
                reviewCollection = new ObservableCollection<Models.PlaceReview.Review>();

                establishmentInfoCollection = new ObservableCollection<Models.EstablishmentInformation>();

                foreach (var review in reviews)
                {
                    establishmentInfoCollection.Add(new Models.EstablishmentInformation()
                    {
                        AuthorName = review.author_name,
                        PhotoUrl = review.profile_photo_url,
                        Rating = review.rating,
                        RelativeTimeDescription = review.relative_time_description,
                        Review = review.text
                    });
                }

                listView.ItemsSource = establishmentInfoCollection;
            }
        }

        public async Task GetTravelDetails(string mode)
        {
            try
            {
                httpClient = new HttpClient();

                var response = await httpClient.GetStringAsync(Common.GetTravelDetails(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude, GlobalVariables.DestinationLatitude, GlobalVariables.DestinationLongitude, mode));

                var deserializedJson = JsonConvert.DeserializeObject<Models.Distance.Rootobject>(response);

                if (deserializedJson.status.Contains("ok".ToUpper()))
                {
                    if (mode == "walking")
                    {
                        LblWalkingTime.Text = deserializedJson.rows[0].elements[0].duration.text;
                    }
                    else if (mode == "driving")
                    {
                        LblDrivingTime.Text = deserializedJson.rows[0].elements[0].duration.text;
                        txtTravelTime.Text = String.Format("Travel Time: {0} (Driving mode)", deserializedJson.rows[0].elements[0].duration.text);
                    }
                    else if (mode == "transit")
                    {
                        LblTransitTime.Text = deserializedJson.rows[0].elements[0].duration.text;
                    }
                }
                else
                    await UserDialogs.Instance.AlertAsync("No results found.", "Error", "OK");
            }
            catch(NullReferenceException)
            {
                if (mode == "walking")
                {
                    LblWalkingTime.Text = "Not Available";
                }
                else if (mode == "driving")
                {
                    LblDrivingTime.Text = "Not Available";
                }
                else if (mode == "transit")
                {
                    LblTransitTime.Text = "Not Available";
                }
            }
            catch (HttpRequestException)
            {
                await UserDialogs.Instance.AlertAsync("Unstable network connection. Please reconnect.", "Error", "OK");
            }
        }
        #endregion
    }
}