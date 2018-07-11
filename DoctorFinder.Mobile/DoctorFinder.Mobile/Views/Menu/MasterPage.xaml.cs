using DoctorFinder.Mobile.Models;
using DoctorFinder.Mobile.Views.DetailViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoctorFinder.Mobile.Views.Menu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : ContentPage
	{
        #region Variables
        public ListView ListView { get => listView; }
        public List<MasterMenuItem> items;
        #endregion

        #region Constructor
        public MasterPage()
        {
            InitializeComponent();

            SetItems();
        }
        #endregion

        #region Events
        protected void BtnLogout_Clicked(object sender, EventArgs e)
        {
            //App.Current.MainPage = new LoginPage();
        }
        #endregion

        #region Methods and Functions
        private void SetItems()
        {
            items = new List<MasterMenuItem>
            {
                new MasterMenuItem("Main", "homeBlack.png", Color.White, typeof(MapViewPage)),
                new MasterMenuItem("List", "listBlack.png", Color.White, typeof(HospitalListPage))
            };
            ListView.ItemsSource = items;
        }
        #endregion
    }
}