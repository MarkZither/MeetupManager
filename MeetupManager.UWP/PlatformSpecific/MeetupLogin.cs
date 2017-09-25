using MeetupManager.Portable.Interfaces;
using MeetupManager.Portable.Services;
using MeetupManager.UWP.PlatformSpecific;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(MeetupLogin))]
namespace MeetupManager.UWP.PlatformSpecific
{
    public class MeetupLogin : ILogin
    {
        #region ILogin implementation

        readonly OAuth2Authenticator auth = new OAuth2Authenticator(MeetupService.ClientId, MeetupService.ClientSecret, string.Empty, new Uri(MeetupService.AuthorizeUrl), new Uri(MeetupService.RedirectUrl), new Uri(MeetupService.AccessTokenUrl));

        public void LoginAsync(Action<bool, Dictionary<string, string>> loginCallback)
        {
            //var activity = Xamarin.Forms.Forms.Context as Activity;

            auth.AllowCancel = true;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += (s, ee) =>
            {
                if (loginCallback != null)
                {
                    loginCallback(ee.IsAuthenticated, ee.Account == null ? null : ee.Account.Properties);
                }
            };


            Type page_type = auth.GetUI();
            //Frame rootFrame = Window.Current.Content as Frame;
            //Windows.UI.Xaml.Controls.Page this_page = this;
            //this_page.Frame.Navigate(page_type, auth);
        }

        #endregion
    }

}
