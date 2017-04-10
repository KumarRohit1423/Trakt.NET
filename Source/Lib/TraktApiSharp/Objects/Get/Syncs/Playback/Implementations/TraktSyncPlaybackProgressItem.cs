﻿namespace TraktApiSharp.Objects.Get.Syncs.Playback.Implementations
{
    using Enums;
    using Episodes;
    using Movies;
    using Newtonsoft.Json;
    using Shows;
    using System;

    /// <summary>Contains information about a Trakt playback progress, including the corresponding movie or episode.</summary>
    public class TraktSyncPlaybackProgressItem : ITraktSyncPlaybackProgressItem
    {
        /// <summary>Gets or sets the id of this progress item.</summary>
        [JsonProperty(PropertyName = "id")]
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets a value between 0 and 100 representing the watched progress percentage
        /// of the movie or episode.
        /// </summary>
        [JsonProperty(PropertyName = "progress")]
        public float? Progress { get; set; }

        /// <summary>Gets or sets the UTC datetime, when the watched movie or episode was paused.</summary>
        [JsonProperty(PropertyName = "paused_at")]
        public DateTime? PausedAt { get; set; }

        /// <summary>
        /// Gets or sets the object type, which this progress item contains.
        /// See also <seealso cref="TraktSyncType" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(TraktEnumerationConverter<TraktSyncType>))]
        public TraktSyncType Type { get; set; }

        /// <summary>
        /// Gets or sets the movie, if <see cref="Type" /> is <see cref="TraktSyncType.Movie" />.
        /// See also <seealso cref="ITraktMovie" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "movie")]
        public ITraktMovie Movie { get; set; }

        /// <summary>
        /// Gets or sets the episode, if <see cref="Type" /> is <see cref="TraktSyncType.Episode" />.
        /// See also <seealso cref="ITraktEpisode" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "episode")]
        public ITraktEpisode Episode { get; set; }

        /// <summary>
        /// Gets or sets the show, if <see cref="Type" /> is <see cref="TraktSyncType.Episode" />.
        /// See also <seealso cref="ITraktShow" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "show")]
        public ITraktShow Show { get; set; }
    }
}
