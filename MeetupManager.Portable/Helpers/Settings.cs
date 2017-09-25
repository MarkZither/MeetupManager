
using System;
using Plugin.Settings.Abstractions;
using Plugin.Settings;

namespace MeetupManager.Portable.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Simply setup your settings once when it is initialized.
        /// </summary>
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

#region Setting Constants

		const string AccessTokenKey = "access_token";
		static string AccessTokenDefault = string.Empty;

		const string RefreshTokenKey = "refresh_token";
		static string RefreshTokenDefault = string.Empty;

		const string KeyValidUntilKey = "key_valid";
		const long KeyValidUntilDefault = 0;

		const string InsightsKey = "insights";
		const bool InsightsDefault = true;

        const string ShowAllEventsKey = "show_all_events";
        const bool ShowAllEventsDefault = false;

        const string UserIdKey = "user_id";
        static string UserIdDefault = string.Empty;

        const string UserNameKey = "user_name";
        static string UserNameDefault = string.Empty;

		const string OrganizerModeKey = "organizer_mode";
		const bool OrganizerModeDefault = false;



#endregion

 
		public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(AccessTokenKey, AccessTokenDefault);
            }
            set
            {
               AppSettings.AddOrUpdateValue(AccessTokenKey, value);
            }
        }

		public static string RefreshToken
		{
			get
			{
                return AppSettings.GetValueOrDefault(RefreshTokenKey, RefreshTokenDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(RefreshTokenKey, value);
			}
		}


		public static bool Insights
		{
			get
			{
                return AppSettings.GetValueOrDefault(InsightsKey, InsightsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(InsightsKey, value);
			}
		}

        public static bool ShowAllEvents
        {
            get
            {
                return AppSettings.GetValueOrDefault(ShowAllEventsKey, ShowAllEventsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ShowAllEventsKey, value);
            }
        }

        public static string UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserIdKey, UserIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserIdKey, value);
            }
        }


        public static string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserNameKey, UserNameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserNameKey, value);
            }
        }

		public static long KeyValidUntil
		{
			get
			{
                return AppSettings.GetValueOrDefault(KeyValidUntilKey, KeyValidUntilDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(KeyValidUntilKey, value);
			}
		}

		public static bool OrganizerMode
		{
			get
			{
                return AppSettings.GetValueOrDefault(OrganizerModeKey, OrganizerModeDefault);
			}
			set
			{
                AppSettings.AddOrUpdateValue(OrganizerModeKey, value);
			}
		}

    }
}
