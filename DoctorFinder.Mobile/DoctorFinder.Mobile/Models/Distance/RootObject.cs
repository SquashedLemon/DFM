using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorFinder.Mobile.Models.Distance
{
    public class Rootobject
    {
        public List<string> destination_addresses { get; set; }
        public List<string> origin_addresses { get; set; }
        public List<Row> rows { get; set; }
        public string status { get; set; }
    }

    public class Row
    {
        public List<Element> elements { get; set; }
    }

    public class Element
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Distance distance { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Duration duration { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Fare fare { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string status { get; set; }
    }

    public class Distance
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string text { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int value { get; set; }
    }

    public class Duration
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string text { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int value { get; set; }
    }

    public class Fare
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string currency { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string text { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int value { get; set; }
    }
}
