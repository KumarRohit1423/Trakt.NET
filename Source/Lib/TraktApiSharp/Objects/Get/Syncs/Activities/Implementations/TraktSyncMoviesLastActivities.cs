﻿namespace TraktApiSharp.Objects.Get.Syncs.Activities.Implementations
{
    using Newtonsoft.Json;
    using System;

    /// <summary>A collection of UTC datetimes of last activities for movies.</summary>
    public class TraktSyncMoviesLastActivities : ITraktSyncMoviesLastActivities
    {
        /// <summary>Gets or sets the UTC datetime, when a movie was lastly watched.</summary>
        [JsonProperty(PropertyName = "watched_at")]
        public DateTime? WatchedAt { get; set; }

        /// <summary>Gets or sets the UTC datetime, when a movie was lastly collected.</summary>
        [JsonProperty(PropertyName = "collected_at")]
        public DateTime? CollectedAt { get; set; }

        /// <summary>Gets or sets the UTC datetime, when a movie was lastly rated.</summary>
        [JsonProperty(PropertyName = "rated_at")]
        public DateTime? RatedAt { get; set; }

        /// <summary>Gets or sets the UTC datetime, when a movie was lastly added to the watchlist.</summary>
        [JsonProperty(PropertyName = "watchlisted_at")]
        public DateTime? WatchlistedAt { get; set; }

        /// <summary>Gets or sets the UTC datetime, when a movie was lastly commented.</summary>
        [JsonProperty(PropertyName = "commented_at")]
        public DateTime? CommentedAt { get; set; }

        /// <summary>Gets or sets the UTC datetime, when a movie was lastly paused.</summary>
        [JsonProperty(PropertyName = "paused_at")]
        public DateTime? PausedAt { get; set; }

        /// <summary>Gets or sets the UTC datetime, when a movie was lastly hidden.</summary>
        [JsonProperty(PropertyName = "hidden_at")]
        public DateTime? HiddenAt { get; set; }
    }
}
