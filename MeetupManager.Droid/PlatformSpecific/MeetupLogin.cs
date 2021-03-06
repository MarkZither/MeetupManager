
using System;
using System.Collections.Generic;
using MeetupManager.Portable.Interfaces;
using MeetupManager.Portable.Services;
using Xamarin.Auth;
using Android.App;
using Xamarin.Forms;
using MeetupManager.Droid.PlatformSpecific;
using MeetupManager.Portable.Helpers;

[assembly:Dependency(typeof(MeetupLogin))]
namespace MeetupManager.Droid.PlatformSpecific
{
    public class MeetupLogin : ILogin
    {
        #region ILogin implementation

        readonly OAuth2Authenticator2 auth = new OAuth2Authenticator2(MeetupService.ClientId, MeetupService.ClientSecret
            , string.Empty, new Uri(MeetupService.AuthorizeUrl), new Uri(MeetupService.RedirectUrl)
            , new Uri(MeetupService.AccessTokenUrl), null, true);

        public void LoginAsync(Action<bool, Dictionary<string, string>> loginCallback)
        {
            var activity = Xamarin.Forms.Forms.Context as Activity;

            auth.AllowCancel = true;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += (s, ee) =>
            {
                if (loginCallback != null)
                {
                    loginCallback(ee.IsAuthenticated, ee.Account == null ? null : ee.Account.Properties);
                }
            };

            AuthenticationState.Authenticator = auth;
            var intent = auth.GetUI(activity);
            activity.StartActivity(intent);
        }
        #endregion
    }
}

