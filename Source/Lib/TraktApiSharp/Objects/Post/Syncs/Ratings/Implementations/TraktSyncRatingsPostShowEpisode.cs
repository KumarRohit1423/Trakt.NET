﻿namespace TraktApiSharp.Objects.Post.Syncs.Ratings.Implementations
{
    using System;

    /// <summary>
    /// A Trakt ratings post episode, containing the required episode number,
    /// an optional rating and an optional datetime, when the episode was rated.
    /// </summary>
    public class TraktSyncRatingsPostShowEpisode : ITraktSyncRatingsPostShowEpisode
    {
        /// <summary>Gets or sets the optional UTC datetime, when the Trakt episode was rated.</summary>
        public DateTime? RatedAt { get; set; }

        /// <summary>Gets or sets an optional rating for the episode.</summary>
        public int? Rating { get; set; }

        /// <summary>Gets or sets the required season number of the Trakt episode.</summary>
        public int Number { get; set; }
    }
}