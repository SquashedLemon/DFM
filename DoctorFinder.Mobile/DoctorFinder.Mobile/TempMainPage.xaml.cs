using DoctorFinder.Mobile.Globals;
using DoctorFinder.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoctorFinder.Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TempMainPage : ContentPage
	{
        #region Variable Declarations
        private bool isMap = true;
        private bool isSecond = false;
        #endregion

        #region Constructor
        public TempMainPage()
        {
            InitializeComponent();

            myStack.Children.Add(new MapView());
        }
        #endregion

        #region Events
        protected void FrmHospital_Tapped(object sender, EventArgs e)
        {
            GlobalVariables.EstablishmentName = "Hospital";

            myStack.Children.Clear();

            if (isMap)
            {
                myStack.Children.Add(new MapView());

                isSecond = false;
            }
            else
            {
                myStack.Children.Add(new MapListView(isSecond));

                isSecond = true;
            }
        }

        protected void FrmClinic_Tapped(object sender, EventArgs e)
        {
            GlobalVariables.EstablishmentName = "Clinic";

            myStack.Children.Clear();

            if (isMap)
            {
                myStack.Children.Add(new MapView());

                isSecond = false;
            }
            else
            {
                myStack.Children.Add(new MapListView(isSecond));

                isSecond = true;
            }
        }

        protected void FrmPharmacy_Tapped(object sender, EventArgs e)
        {
            GlobalVariables.EstablishmentName = "Pharmacy";

            myStack.Children.Clear();

            if (isMap)
            {
                myStack.Children.Add(new MapView());

                isSecond = false;
            }
            else
            {
                myStack.Children.Add(new MapListView(isSecond));

                isSecond = true;
            }
        }

        protected void FrmDoctor_Tapped(object sender, EventArgs e)
        {
            GlobalVariables.EstablishmentName = "Doctor";

            myStack.Children.Clear();

            if (isMap)
            {
                myStack.Children.Add(new MapView());

                isSecond = false;
            }
            else
            {
                myStack.Children.Add(new MapListView(isSecond));

                isSecond = true;
            }
        }

        protected void BtnMap_Clicked(object sender, EventArgs e)
        {
            isMap = true;

            myStack.Children.Clear();
            myStack.Children.Add(new MapView());
        }

        protected void BtnList_Clicked(object sender, EventArgs e)
        {
            isMap = false;

            myStack.Children.Clear();
            myStack.Children.Add(new MapListView());
        }
        #endregion

        #region Methods and Functions

        #endregion
    }
}