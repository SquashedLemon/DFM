using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorFinder.Mobile.Models.PlaceReview
{
    public class Rootobject
    {
        [JsonProperty("html_attributions")]
        public List<object> html_attributions { get; set; }
        [JsonProperty("result")]
        public Result result { get; set; }
        [JsonProperty("status")]
        public string status { get; set; }
    }

    public class Result
    {
        [JsonProperty(PropertyName = "formatted_address", NullValueHandling = NullValueHandling.Ignore)]
        public string formatted_address { get; set; }
        [JsonProperty(PropertyName = "formatted_phone_number", NullValueHandling = NullValueHandling.Ignore)]
        public string formatted_phone_number { get; set; }
        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }
        [JsonProperty(PropertyName = "rating", NullValueHandling = NullValueHandling.Ignore)]
        public float rating { get; set; }
        [JsonProperty(PropertyName = "reviews", NullValueHandling = NullValueHandling.Ignore)]
        public List<Review> reviews { get; set; }
    }

    public class Review
    {
        [JsonProperty("author_name")]
        public string author_name { get; set; }
        [JsonProperty("author_url")]
        public string author_url { get; set; }
        [JsonProperty("language")]
        public string language { get; set; }
        [JsonProperty("profile_photo_url")]
        public string profile_photo_url { get; set; }
        [JsonProperty("rating")]
        public int rating { get; set; }
        [JsonProperty("relative_time_description")]
        public string relative_time_description { get; set; }
        [JsonProperty("text")]
        public string text { get; set; }
    }
}
