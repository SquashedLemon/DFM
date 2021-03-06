﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DoctorFinder.Mobile.Models
{
    public class Establishment
    {
        private static ObservableCollection<Establishment> establishments;

        public ObservableCollection<Establishment> establishmentssss
        {
            get => establishments;
            set => establishments = value;
        }

        public string PlaceId { get; set; }
        public string Name { get; set; }
        public string Vicinity { get; set; }
        public string Distance { get; set; }
        public int DistanceValue { get; set; }
        public string TravelTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FormattedPhoneNumber { get; set; }
        public string Rating { get; set; }
        public string EstablishmentTitle
        {
            get
            {
                return String.Format("({0}) {1}", Distance, Name);
            }
        }
    }

    public class EstablishmentInformation
    {
        public string Review { get; set; }
        public string PhotoUrl { get; set; }
        public string AuthorName { get; set; }
        public int Rating { get; set; }
        public string RelativeTimeDescription { get; set; }

        Establishment est = new Establishment();

        private void getdata()
        {
            ObservableCollection<Establishment> example = new ObservableCollection<Establishment>()
            {
                new Establishment() { Distance = "dsasd" },
                new Establishment() {}
            };
        }
    }
}