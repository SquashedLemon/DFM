using Acr.UserDialogs;
using DoctorFinder.Mobile.Globals;
using DoctorFinder.Mobile.Helpers;
using DoctorFinder.Mobile.Models.Places;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoctorFinder.Mobile.Views.DetailViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HospitalListPage : ContentPage
	{
        #region Variables
        ObservableCollection<Result> observableResulstList;
        ObservableCollection<Models.Establishment> observableEstablishments;
        ObservableCollection<Models.Establishment> orderedObservableEstablishments;
        List<DoctorFinder.Mobile.Models.Places.Result> resultsList;
        HttpClient httpClient;
        DoctorFinder.Mobile.Models.Places.RootObject rootObject;
        #endregion

        #region Constructor
        public HospitalListPage()
        {
            InitializeComponent();

            listView.ItemSelected += ListView_OnItemSelected;
            SearchPlace.TextChanged += SearchPlace_TextChanged;
            //listView.Focused += ListView_Focused;

            new Action(async () =>
            {
                var loading = UserDialogs.Instance.Loading("Please wait...");

                //await ListNearbyHospitals();

                await GetDirection();
                loading.Hide();
            }).Invoke();
        }
        #endregion

        #region Events
        protected async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            else
            {
                var placeResult = e.SelectedItem as Models.Establishment;
                //await Navigation.PushAsync(new HospitalDetailsPage(e.SelectedItem as Result), true);

                await Navigation.PushAsync(new HospitalDetailPage(placeResult), true);

                listView.SelectedItem = null;
            }
        }

        protected void SearchPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchPlace.Text))
                listView.ItemsSource = orderedObservableEstablishments;
            else
            {
                var text = SearchPlace.Text;

                var myPlaceList = new ObservableCollection<Models.Establishment>(observableEstablishments.Where(data => data.Name.Contains(SearchPlace.Text) || data.Vicinity.Contains(SearchPlace.Text)));

                listView.ItemsSource = myPlaceList;
            }
        }
        #endregion

        #region Methods and Functions
        private async Task ListNearbyEstablishments()
        {
            httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync(Common.GetLocationUri(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude));

            var deserializedJson = JsonConvert.DeserializeObject<DoctorFinder.Mobile.Models.Places.RootObject>(response);

            rootObject = deserializedJson;

            var resultStatus = rootObject.status;

            if (resultStatus.Contains("ok".ToUpper()))
            {
                resultsList = rootObject.results;

                observableResulstList = new ObservableCollection<DoctorFinder.Mobile.Models.Places.Result>(resultsList);

                var orderedResultsList = observableResulstList.OrderByDescending(result => result.rating);

                //listView.ItemsSource = orderedResultsList;
                //listView.ItemsSource = observableResulstList;
            }
            else if (resultStatus.Contains("zero_results".ToUpper()))
                await UserDialogs.Instance.AlertAsync("No results found.", "", "OK");
        }

        private async Task<List<DoctorFinder.Mobile.Models.Places.Result>> GetList()
        {
            httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync(Common.GetLocationUri(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude, Convert.ToInt32(GlobalVariables.Radius), GlobalVariables.EstablishmentType, GlobalVariables.EstablishmentName));

            var deserializatedJson = JsonConvert.DeserializeObject<DoctorFinder.Mobile.Models.Places.RootObject>(response);

            rootObject = deserializatedJson;

            var resultStatus = rootObject.status;

            if (resultStatus.Contains("ok".ToUpper()))
            {
                resultsList = rootObject.results;
            }
            else
                resultsList = null;

            return resultsList;
        }

        private async Task GetDirection()
        {
            List<DoctorFinder.Mobile.Models.Places.Result> mylist = await GetList();
            List<Models.Establishment> establishments = new List<Models.Establishment>();
            
            foreach (var item in mylist)
            {
                httpClient = new HttpClient();

                var desLt = item.geometry.location.lat;
                var desLn = item.geometry.location.lng;

                var response = await httpClient.GetStringAsync(Common.GetRouteUri(GlobalVariables.CurrentLocationLatitude, GlobalVariables.CurrentLocationLongitude, desLt, desLn));

                var deserializedJson = JsonConvert.DeserializeObject<Models.Directions.RootObject>(response);

                var routes = deserializedJson.routes.ToList();

                establishments.Add(new Models.Establishment()
                {
                    Name = item.name,
                    Distance = routes[0].legs[0].distance.text,
                    DistanceValue = routes[0].legs[0].distance.value,
                    TravelTime = routes[0].legs[0].duration.text,
                    Vicinity = item.vicinity,
                    Latitude = item.geometry.location.lat,
                    Longitude = item.geometry.location.lng
                });
            }

            observableEstablishments = new ObservableCollection<Models.Establishment>(establishments);

            orderedObservableEstablishments = new ObservableCollection<Models.Establishment>(observableEstablishments.OrderBy(x => x.Distance));

            listView.ItemsSource = orderedObservableEstablishments;
        }
        #endregion
    }
}