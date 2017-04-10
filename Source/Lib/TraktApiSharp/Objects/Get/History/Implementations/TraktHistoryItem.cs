﻿namespace TraktApiSharp.Objects.Get.History.Implementations
{
    using Enums;
    using Episodes;
    using Movies;
    using Newtonsoft.Json;
    using Seasons;
    using Shows;
    using System;

    /// <summary>A Trakt history item, containing a movie, show, season and / or episode and information about it.</summary>
    public class TraktHistoryItem : ITraktHistoryItem
    {
        /// <summary>Gets or sets the id of this history item.</summary>
        [JsonProperty(PropertyName = "id")]
        public ulong Id { get; set; }

        /// <summary>Gets or sets the UTC datetime, when the movie, show, season and / or episode was watched.</summary>
        [JsonProperty(PropertyName = "watched_at")]
        public DateTime? WatchedAt { get; set; }

        /// <summary>
        /// Gets or sets the type of action. See also <seealso cref="TraktHistoryActionType" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "action")]
        [JsonConverter(typeof(TraktEnumerationConverter<TraktHistoryActionType>))]
        public TraktHistoryActionType Action { get; set; }

        /// <summary>
        /// Gets or sets the object type, which this history item contains.
        /// See also <seealso cref="TraktSyncItemType" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(TraktEnumerationConverter<TraktSyncItemType>))]
        public TraktSyncItemType Type { get; set; }

        /// <summary>
        /// Gets or sets the movie, if <see cref="Type" /> is <see cref="TraktSyncItemType.Movie" />.
        /// See also <seealso cref="ITraktMovie" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "movie")]
        public ITraktMovie Movie { get; set; }

        /// <summary>
        /// Gets or sets the show, if <see cref="Type" /> is <see cref="TraktSyncItemType.Show" />.
        /// May also be set, if <see cref="Type" /> is <see cref="TraktSyncItemType.Episode" /> or
        /// <see cref="TraktSyncItemType.Season" />.
        /// <para>See also <seealso cref="ITraktShow" />.</para>
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "show")]
        public ITraktShow Show { get; set; }

        /// <summary>
        /// Gets or sets the season, if <see cref="Type" /> is <see cref="TraktSyncItemType.Season" />.
        /// See also <seealso cref="ITraktSeason" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "season")]
        public ITraktSeason Season { get; set; }

        /// <summary>
        /// Gets or sets the episode, if <see cref="Type" /> is <see cref="TraktSyncItemType.Episode" />.
        /// See also <seealso cref="ITraktEpisode" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "episode")]
        public ITraktEpisode Episode { get; set; }
    }
}
