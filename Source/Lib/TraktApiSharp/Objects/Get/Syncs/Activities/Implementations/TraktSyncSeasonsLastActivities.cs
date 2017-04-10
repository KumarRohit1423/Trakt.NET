﻿namespace TraktApiSharp.Objects.Get.Syncs.Activities.Implementations
{
    using Newtonsoft.Json;
    using System;

    /// <summary>A collection of UTC datetimes of last activities for seasons.</summary>
    public class TraktSyncSeasonsLastActivities : ITraktSyncSeasonsLastActivities
    {
        /// <summary>Gets or sets the UTC datetime, when a season was lastly rated.</summary>
        [JsonProperty(PropertyName = "rated_at")]
        public DateTime? RatedAt { get; set; }

        /// <summary>Gets or sets the UTC datetime, when a season was lastly added to the watchlist.</summary>
        [JsonProperty(PropertyName = "watchlisted_at")]
        public DateTime? WatchlistedAt { get; set; }

        /// <summary>Gets or sets the UTC datetime, when a season was lastly commented.</summary>
        [JsonProperty(PropertyName = "commented_at")]
        public DateTime? CommentedAt { get; set; }

        /// <summary>Gets or sets the UTC datetime, when a season was lastly hidden.</summary>
        [JsonProperty(PropertyName = "hidden_at")]
        public DateTime? HiddenAt { get; set; }
    }
}
