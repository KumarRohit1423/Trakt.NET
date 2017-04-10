﻿namespace TraktApiSharp.Objects.Get.Ratings.Implementations
{
    using Enums;
    using Episodes;
    using Movies;
    using Newtonsoft.Json;
    using Seasons;
    using Shows;
    using System;

    /// <summary>A Trakt rating item, containing a movie, show, season and / or episode and information about it.</summary>
    public class TraktRatingsItem : ITraktRatingsItem
    {
        /// <summary>Gets or sets the UTC datetime, when the movie, show, season and / or episode was rated.</summary>
        [JsonProperty(PropertyName = "rated_at")]
        public DateTime? RatedAt { get; set; }

        /// <summary>Gets or sets the rating of the movie, show, season and / or episode.</summary>
        [JsonProperty(PropertyName = "rating")]
        public int? Rating { get; set; }

        /// <summary>
        /// Gets or sets the object type, which this rating item contains.
        /// See also <seealso cref="TraktRatingsItemType" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(TraktEnumerationConverter<TraktRatingsItemType>))]
        public TraktRatingsItemType Type { get; set; }

        /// <summary>
        /// Gets or sets the movie, if <see cref="Type" /> is <see cref="TraktRatingsItemType.Movie" />.
        /// See also <seealso cref="ITraktMovie" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "movie")]
        public ITraktMovie Movie { get; set; }

        /// <summary>
        /// Gets or sets the show, if <see cref="Type" /> is <see cref="TraktRatingsItemType.Show" />.
        /// May also be set, if <see cref="Type" /> is <see cref="TraktRatingsItemType.Episode" /> or
        /// <see cref="TraktRatingsItemType.Season" />.
        /// <para>See also <seealso cref="ITraktShow" />.</para>
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "show")]
        public ITraktShow Show { get; set; }

        /// <summary>
        /// Gets or sets the season, if <see cref="Type" /> is <see cref="TraktRatingsItemType.Season" />.
        /// See also <seealso cref="ITraktSeason" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "season")]
        public ITraktSeason Season { get; set; }

        /// <summary>
        /// Gets or sets the episode, if <see cref="Type" /> is <see cref="TraktRatingsItemType.Episode" />.
        /// See also <seealso cref="ITraktEpisode" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "episode")]
        public ITraktEpisode Episode { get; set; }
    }
}
