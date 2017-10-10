using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MeetupManager.Portable.Helpers;
using MeetupManager.Portable.Interfaces;
using MeetupManager.Portable.Models;
using MeetupManager.Portable.Services;
using MeetupManager.Portable.Services.Responses;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly:Dependency(typeof(MeetupService))]
namespace MeetupManager.Portable.Services
{
    public class MeetupService : IMeetupService
    {
        readonly IMessageDialog messageDialog;

        public MeetupService()
        {
            messageDialog = DependencyService.Get<IMessageDialog>();
        }

        HttpClient CreateClient()
        {
					
            return new HttpClient();
	     
        }

        #region IMeetupService implementation

        public static string ClientId = "b648f4c5a17e21d2bb148df32d1dd75d49ff88382e4cc875b125e416476d97b2";
        public static string ClientSecret = "918dd59b63bca92135d63747240f147a9f866ea57caa0fec284303fc50ead305";
        public static string AuthorizeUrl = "https://accounts.tidyhq.com/oauth/authorize";
        public static string RedirectUrl = "zithertidyhq://oauth2redirect/";
        public static string AccessTokenUrl = "https://accounts.tidyhq.com/oauth/token";


        const string GetGroupsUrl = @"https://api.tidyhq.com/v1/organization?access_token={0}";
        const string GetGroupsOrganizerUrl = @"https://api.meetup.com/2/groups?offset={0}&organizer_id={1}&page=100&order=name&access_token={2}&only=name,id,group_photo,members";
        const string GetEventsUrl = @"https://api.tidyhq.com/v1/events?offset={0}&limit={1}&page=100&access_token={2}";
        const string GetRSVPsUrl = @"https://api.meetup.com/2/rsvps?offset={0}&event_id={1}&page=100&order=name&rsvp=yes&access_token={2}&only=member,member_photo,guests";
        const string GetUserUrl = @"https://api.tidyhq.com/v1/contacts?access_token={0}&search_terms={1}";

		
        public async Task<EventsRootObject> GetEvents(string groupId, int skip)
        {
            var offset = skip / 100;
            if (!await RenewAccessToken())
            {
                messageDialog.SendToast("Unable to get events, please re-login.");
                return new EventsRootObject() { Events = new List<Event>() };
            }

            var client = CreateClient();
            if (client.DefaultRequestHeaders.CacheControl == null)
            {
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();
            }

            client.DefaultRequestHeaders.CacheControl.NoCache = true;
            client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
            client.DefaultRequestHeaders.CacheControl.NoStore = true;
            client.Timeout = new TimeSpan(0, 0, 30);
            var request = string.Format(GetEventsUrl, offset, groupId, Settings.AccessToken);
            if (!Settings.ShowAllEvents)
            {
                request += "&time=-100m,2m";
            }

            var response = await client.GetStringAsync(request);
            return await DeserializeObjectAsync<EventsRootObject>(response);
        }

        public async Task<RSVPsRootObject> GetRSVPs(string eventId, int skip)
        {
            var offset = skip / 100;
            if (!await RenewAccessToken())
            {
                messageDialog.SendToast("Unable to get RSVPs, please re-login.");
                return new RSVPsRootObject() { RSVPs = new List<RSVP>() };
            }


            var client = CreateClient();
            if (client.DefaultRequestHeaders.CacheControl == null)
            {
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();
            }

            client.DefaultRequestHeaders.CacheControl.NoCache = true;
            client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
            client.DefaultRequestHeaders.CacheControl.NoStore = true;
            client.Timeout = new TimeSpan(0, 0, 30);
            var request = string.Format(GetRSVPsUrl, offset, eventId, Settings.AccessToken);
            var response = await client.GetStringAsync(request);
            return await DeserializeObjectAsync<RSVPsRootObject>(response);

        }



        public async Task<RequestTokenObject> GetToken(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            using (var client = CreateClient())
            {
                try
                {
                    if (client.DefaultRequestHeaders.CacheControl == null)
                    {
                        client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();
                    }

                    client.DefaultRequestHeaders.CacheControl.NoCache = true;
                    client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
                    client.DefaultRequestHeaders.CacheControl.NoStore = true;
                    client.Timeout = new TimeSpan(0, 0, 30);

                    var content = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("client_id", ClientId),
                            new KeyValuePair<string, string>("client_secret", ClientSecret),
                            new KeyValuePair<string, string>("grant_type", "authorization_code"),
                            new KeyValuePair<string, string>("redirect_uri", RedirectUrl), 
                            new KeyValuePair<string, string>("code", code), 
                        });

                    var result = await client.PostAsync(AccessTokenUrl, content);
                    var response = await result.Content.ReadAsStringAsync();
                    var refreshResponse = await DeserializeObjectAsync<RequestTokenObject>(response);
                    return refreshResponse;
                }
                catch (Exception ex)
                {
                    if (Settings.Insights)
                    {
                        Xamarin.Insights.Report(ex);
                    }
                }

            }

            return null;
        }

        public class RequestTokenObject
        {
            public string access_token { get; set; }

            public string token_type { get; set; }

            public int expires_in { get; set; }

            public string refresh_token { get; set; }
        }

        public async Task<bool> RenewAccessToken()
        {
            if (string.IsNullOrWhiteSpace(Settings.AccessToken))
            {
                return false;
            }

            if (DateTime.UtcNow < new DateTime(Settings.KeyValidUntil))
            {
                return true;
            }

            using (var client = CreateClient())
            {
                try
                {
                    if (client.DefaultRequestHeaders.CacheControl == null)
                    {
                        client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();
                    }

                    client.DefaultRequestHeaders.CacheControl.NoCache = true;
                    client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
                    client.DefaultRequestHeaders.CacheControl.NoStore = true;
                    client.Timeout = new TimeSpan(0, 0, 30);
          
                    var content = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("client_id", ClientId),
                            new KeyValuePair<string, string>("client_secret", ClientSecret),
                            new KeyValuePair<string, string>("grant_type", "refresh_token"),
                            new KeyValuePair<string, string>("refresh_token", Settings.RefreshToken), 
                        });

                    var result = await client.PostAsync(AccessTokenUrl, content);
                    var response = await result.Content.ReadAsStringAsync();
                    var refreshResponse = await DeserializeObjectAsync<RefreshRootObject>(response);
                    Settings.AccessToken = refreshResponse.AccessToken;
                    var nextTime = DateTime.UtcNow.AddSeconds(refreshResponse.ExpiresIn).Ticks;
                    Settings.KeyValidUntil = nextTime;
                    Settings.RefreshToken = refreshResponse.RefreshToken;
                }
                catch (Exception ex)
                {
                    if (Settings.Insights)
                    {
                        Xamarin.Insights.Report(ex);
                    }

                    return false;
                }
                
            }

            return true;
        }


        public async Task<LoggedInUser> GetCurrentMember()
        {
            await RenewAccessToken();
            var client = CreateClient();
            if (client.DefaultRequestHeaders.CacheControl == null)
            {
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();
            }

            client.DefaultRequestHeaders.CacheControl.NoCache = true;
            client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
            client.DefaultRequestHeaders.CacheControl.NoStore = true;
            client.Timeout = new TimeSpan(0, 0, 30);
            var request = string.Format(GetUserUrl, Settings.AccessToken, "mark.burton@zither-it.co.uk");
            var response = await client.GetStringAsync(request);
		  
            //should use async, but has issue for some reason and throws exception
            return DeserializeObject<LoggedInUser>(response);
		        
        }

        #endregion




        public async Task<GroupsRootObject> GetGroups(string memberId, int skip)
        {
            /*
            var offset = skip / 100;
            if (!await RenewAccessToken())
            {
                messageDialog.SendToast("Unable to get groups, please re-login.");
                return new GroupsRootObject{ Groups = new List<Group>() };
            }

            var client = CreateClient();
            if (client.DefaultRequestHeaders.CacheControl == null)
            {
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();
            }

            client.DefaultRequestHeaders.CacheControl.NoCache = true;
            client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
            client.DefaultRequestHeaders.CacheControl.NoStore = true;
            client.Timeout = new TimeSpan(0, 0, 30);
            var request = string.Format(GetGroupsUrl, Settings.AccessToken);

            var response = await client.GetStringAsync(request);
            return await DeserializeObjectAsync<GroupsRootObject>(response);
            */
            List<Group> groups = new List<Group>();
            groups.Add(new Group() { Id = 1, Name = "test" });
            return new GroupsRootObject() { Groups = groups, Meta = new Meta() {Title = "title" } } ;
        }

        public  Task<T> DeserializeObjectAsync<T>(string value)
        {
            return Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value));
        }

        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}

