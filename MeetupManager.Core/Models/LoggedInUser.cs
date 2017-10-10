
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace MeetupManager.Portable.Models
{
    public class LoggedInUserMM
    {
        [JsonProperty("lon")]
        public double Longitude { get; set; }
        [JsonProperty("hometown")]
        public string Hometown { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("self")]
        public Self Self { get; set; }
        [JsonProperty("photo")]
        public Photo Photo { get; set; }
        [JsonProperty("lang")]
        public string Langitude { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("visited")]
        public long Visited { get; set; }
        [JsonProperty("topics")]
        public List<Topic> Topics { get; set; }
        [JsonProperty("joined")]
        public long Joined { get; set; }
        [JsonProperty("bio")]
        public string Bio { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("other_services")]
        public OtherServices OtherServices { get; set; }
        [JsonProperty("lat")]
        public double Lat { get; set; }
    }

    public class LoggedInUser
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string nick_name { get; set; }
        public string company { get; set; }
        public string email_address { get; set; }
        public string phone_number { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string postcode { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public string facebook { get; set; }
        public string twitter { get; set; }
        public string details { get; set; }
        public bool subscribed { get; set; }
        public object metadata { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public object emergency_contact_person { get; set; }
        public object emergency_contact_number { get; set; }
        public object profile_image { get; set; }
        public string status { get; set; }
        public string[] custom_fields { get; set; }
    }

}

