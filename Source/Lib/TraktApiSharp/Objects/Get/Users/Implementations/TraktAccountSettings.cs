﻿namespace TraktApiSharp.Objects.Get.Users.Implementations
{
    using Newtonsoft.Json;

    /// <summary>Represents Trakt user account settings.</summary>
    public class TraktAccountSettings : ITraktAccountSettings
    {
        /// <summary>Gets or sets the user's timezone.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "timezone")]
        public string TimeZoneId { get; set; }

        /// <summary>Gets or sets, whether an user uses the 24h time format.</summary>
        [JsonProperty(PropertyName = "time_24hr")]
        public bool? Time24Hr { get; set; }

        /// <summary>Gets or sets the user's cover image url.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "cover_image")]
        public string CoverImage { get; set; }
    }
}
