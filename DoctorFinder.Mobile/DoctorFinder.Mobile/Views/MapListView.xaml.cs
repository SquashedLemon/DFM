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
        #endregion

        #region Constructor
        public MapListView()
        {
            InitializeComponent();

            if (GlobalVariables.ObservableEstablishments != null)
            {
                listView.ItemsSource = GlobalVariables.ObservableEstablishments;
            }
            else
            {

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

        #endregion
    }
}