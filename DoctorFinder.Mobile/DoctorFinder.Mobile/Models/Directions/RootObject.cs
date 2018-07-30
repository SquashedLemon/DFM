using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorFinder.Mobile.Models.Directions
{
    public partial class RootObject
    {
        [JsonProperty("geocoded_waypoints", NullValueHandling = NullValueHandling.Ignore)]
        public List<GeocodedWaypoint> GeocodedWaypoints { get; set; }

        [JsonProperty("routes", NullValueHandling = NullValueHandling.Ignore)]
        public List<Route> Routes { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
    }

    public partial class GeocodedWaypoint
    {
        [JsonProperty("geocoder_status", NullValueHandling = NullValueHandling.Ignore)]
        public string GeocoderStatus { get; set; }

        [JsonProperty("place_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PlaceId { get; set; }

        [JsonProperty("types", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Types { get; set; }
    }

    public partial class Route
    {
        [JsonProperty("bounds", NullValueHandling = NullValueHandling.Ignore)]
        public Bounds Bounds { get; set; }

        [JsonProperty("copyrights", NullValueHandling = NullValueHandling.Ignore)]
        public string Copyrights { get; set; }

        [JsonProperty("legs", NullValueHandling = NullValueHandling.Ignore)]
        public List<Leg> Legs { get; set; }

        [JsonProperty("overview_polyline", NullValueHandling = NullValueHandling.Ignore)]
        public Polyline OverviewPolyline { get; set; }

        [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
        public string Summary { get; set; }

        [JsonProperty("warnings", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Warnings { get; set; }

        [JsonProperty("waypoint_order", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> WaypointOrder { get; set; }
    }

    public partial class Bounds
    {
        [JsonProperty("northeast", NullValueHandling = NullValueHandling.Ignore)]
        public Location Northeast { get; set; }

        [JsonProperty("southwest", NullValueHandling = NullValueHandling.Ignore)]
        public Location Southwest { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        public double Lat { get; set; }

        [JsonProperty("lng", NullValueHandling = NullValueHandling.Ignore)]
        public double Lng { get; set; }
    }

    public partial class Leg
    {
        [JsonProperty("arrival_time", NullValueHandling = NullValueHandling.Ignore)]
        public Time ArrivalTime { get; set; }

        [JsonProperty("departure_time", NullValueHandling = NullValueHandling.Ignore)]
        public Time DepartureTime { get; set; }

        [JsonProperty("distance", NullValueHandling = NullValueHandling.Ignore)]
        public Distance Distance { get; set; }

        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public Distance Duration { get; set; }

        [JsonProperty("end_address", NullValueHandling = NullValueHandling.Ignore)]
        public string EndAddress { get; set; }

        [JsonProperty("end_location", NullValueHandling = NullValueHandling.Ignore)]
        public Location EndLocation { get; set; }

        [JsonProperty("start_address", NullValueHandling = NullValueHandling.Ignore)]
        public string StartAddress { get; set; }

        [JsonProperty("start_location", NullValueHandling = NullValueHandling.Ignore)]
        public Location StartLocation { get; set; }

        [JsonProperty("steps", NullValueHandling = NullValueHandling.Ignore)]
        public List<Step> Steps { get; set; }

        [JsonProperty("traffic_speed_entry", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> TrafficSpeedEntry { get; set; }

        [JsonProperty("via_waypoint", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> ViaWaypoint { get; set; }
    }

    public partial class Time
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("time_zone", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeZone { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public long? Value { get; set; }
    }

    public partial class Distance
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public long? Value { get; set; }
    }

    public partial class Step
    {
        [JsonProperty("distance", NullValueHandling = NullValueHandling.Ignore)]
        public Distance Distance { get; set; }

        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public Distance Duration { get; set; }

        [JsonProperty("end_location", NullValueHandling = NullValueHandling.Ignore)]
        public Location EndLocation { get; set; }

        [JsonProperty("html_instructions", NullValueHandling = NullValueHandling.Ignore)]
        public string HtmlInstructions { get; set; }

        [JsonProperty("polyline", NullValueHandling = NullValueHandling.Ignore)]
        public Polyline Polyline { get; set; }

        [JsonProperty("start_location", NullValueHandling = NullValueHandling.Ignore)]
        public Location StartLocation { get; set; }

        [JsonProperty("steps", NullValueHandling = NullValueHandling.Ignore)]
        public List<Step> Substeps { get; set; }

        [JsonProperty("travel_mode", NullValueHandling = NullValueHandling.Ignore)]
        public string TravelMode { get; set; }

        [JsonProperty("transit_details", NullValueHandling = NullValueHandling.Ignore)]
        public TransitDetails TransitDetails { get; set; }

        [JsonProperty("maneuver", NullValueHandling = NullValueHandling.Ignore)]
        public string Maneuver { get; set; }
    }

    public partial class Polyline
    {
        [JsonProperty("points", NullValueHandling = NullValueHandling.Ignore)]
        public string Points { get; set; }
    }

    public partial class TransitDetails
    {
        [JsonProperty("arrival_stop", NullValueHandling = NullValueHandling.Ignore)]
        public Stop ArrivalStop { get; set; }

        [JsonProperty("arrival_time", NullValueHandling = NullValueHandling.Ignore)]
        public Time ArrivalTime { get; set; }

        [JsonProperty("departure_stop", NullValueHandling = NullValueHandling.Ignore)]
        public Stop DepartureStop { get; set; }

        [JsonProperty("departure_time", NullValueHandling = NullValueHandling.Ignore)]
        public Time DepartureTime { get; set; }

        [JsonProperty("headsign", NullValueHandling = NullValueHandling.Ignore)]
        public string Headsign { get; set; }

        [JsonProperty("line", NullValueHandling = NullValueHandling.Ignore)]
        public Line Line { get; set; }

        [JsonProperty("num_stops", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumStops { get; set; }
    }

    public partial class Stop
    {
        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public Location Location { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public partial class Line
    {
        [JsonProperty("agencies", NullValueHandling = NullValueHandling.Ignore)]
        public List<Agency> Agencies { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("vehicle", NullValueHandling = NullValueHandling.Ignore)]
        public Vehicle Vehicle { get; set; }
    }

    public partial class Agency
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }

    public partial class Vehicle
    {
        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }
}