using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DoctorFinder.Mobile.Constants;
using DoctorFinder.Mobile.Globals;
using DoctorFinder.Mobile.Helpers;
using DoctorFinder.Mobile.Models;
using Acr.UserDialogs;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using DoctorFinder.Mobile.Views.DetailViews;

namespace DoctorFinder.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapListView : ContentView
	{
        #region Variable Declarations
        private HttpClient httpClient;
        private IProgressDialog progressDialog;
        #endregion

        #region Constructor
        public MapListView(bool isTrue = false)
        {
            InitializeComponent();

            if (!isTrue)
                listView.ItemsSource = GlobalVariables.ObservableEstablishments.OrderBy(x => x.Distance);
            else
            {
                new Action(async () =>
                {
                    progressDialog = UserDialogs.Instance.Loading("Please wait...");

                    await GetItems();
                    await GetDirection();
                }).Invoke();
            }

            SearchPlace.TextChanged += SearchPlace_TextChanged;
            listView.ItemSelected += ListView_OnItemSelected;
        }
        #endregion

        #region Events
        protected async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            else
            {
                var placeResult = e.SelectedItem as Establishment;

                await Navigation.PushAsync(new HospitalDetailPage(placeResult), true);

                listView.SelectedItem = null;
            }
        }

        protected void SearchPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchPlace.Text))
                listView.ItemsSource = GlobalVariables.ObservableEstablishments.OrderBy(x => x.Distance);
            else
            {
                var text = SearchPlace.Text;

                var myPlaceList = new ObservableCollection<Models.Establishment>(GlobalVariables.ObservableEstablishments.Where(x => x.Name.Contains(text) || x.Vicinity.Contains(text)));

                listView.ItemsSource = myPlaceList;
            }
        }
        #endregion

        #region Methods and Functions
        protected async Task GetItems()
        {
            try
            {
                httpClient = new HttpClient();

                var response = await httpClient.GetStringAsync(Common.GetLocationUri(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude, Convert.ToInt32(GlobalVariables.Radius), GlobalVariables.EstablishmentType, GlobalVariables.EstablishmentName));

                var deserializedJson = JsonConvert.DeserializeObject<Models.Places.RootObject>(response);

                var resultStatus = deserializedJson.status;

                if (resultStatus.Contains("ok".ToUpper()))
                {
                    var resultList = deserializedJson.results;

                    GlobalVariables.CurrentResults = resultList;
                }
            }
            catch (Exception)
            {
                progressDialog.Hide();

                await UserDialogs.Instance.AlertAsync("Unstable network connection. Please reconnect.", "Error", "OK");
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

                    var routes = deserializedJson.Routes.ToList();

                    establishmentList.Add(new Establishment()
                    {
                        Name = result.name,
                        Distance = routes.First().Legs.First().Distance.Text,
                        DistanceValue = Convert.ToInt32(routes.First().Legs.First().Duration.Value),
                        TravelTime = routes.First().Legs.First().Duration.Text,
                        Vicinity = result.vicinity,
                        Latitude = result.geometry.location.lat,
                        Longitude = result.geometry.location.lng,
                        PlaceId = result.place_id
                    });
                }

                GlobalVariables.ObservableEstablishments = new ObservableCollection<Establishment>(establishmentList);

                listView.ItemsSource = GlobalVariables.ObservableEstablishments.OrderBy(x => x.Distance);

                progressDialog.Hide();
            }
            catch (HttpRequestException)
            {
                progressDialog.Hide();

                await UserDialogs.Instance.AlertAsync("Unstable network connection. Please reconnect.", "Error", "OK");
            }
        }
        #endregion
    }
}