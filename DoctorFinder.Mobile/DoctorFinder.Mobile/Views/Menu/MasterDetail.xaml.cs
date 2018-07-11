using DoctorFinder.Mobile.Models;
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
	public partial class MasterDetail : MasterDetailPage
	{
        #region Constructor
        public MasterDetail()
        {
            InitializeComponent();

            masterPage.ListView.ItemSelected += ListView_OnItemSelectedChanged;
        }
        #endregion

        #region Events
        protected void ListView_OnItemSelectedChanged(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterMenuItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
        #endregion
    }
}