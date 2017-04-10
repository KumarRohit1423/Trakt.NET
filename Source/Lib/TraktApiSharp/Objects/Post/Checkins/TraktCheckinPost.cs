﻿namespace TraktApiSharp.Objects.Post.Checkins
{
    using Basic.Implementations;
    using Newtonsoft.Json;

    public abstract class TraktCheckinPost
    {
        /// <summary>
        /// Gets or sets the sharing options for the checkin post.
        /// See also <seealso cref="TraktSharing" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "sharing")]
        public TraktSharing Sharing { get; set; }

        /// <summary>Gets or sets the message for the checkin post.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>Gets or sets the app version for the checkin post.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "app_version")]
        public string AppVersion { get; set; }

        /// <summary>Gets or sets the app build date for the checkin post.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "app_date")]
        public string AppDate { get; set; }

        /// <summary>Gets or sets the Foursquare Venue Id for the checkin post.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "venue_id")]
        public string FoursquareVenueId { get; set; }

        /// <summary>Gets or sets the Foursquare Venue Name for the checkin post.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "venue_name")]
        public string FoursquareVenueName { get; set; }
    }
}
