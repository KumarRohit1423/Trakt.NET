﻿namespace TraktApiSharp.Objects.Post.Users.CustomListItems.Responses
{
    using Get.Episodes.Implementations;
    using Get.Movies.Implementations;
    using Get.People.Implementations;
    using Get.Seasons.Implementations;
    using Get.Shows.Implementations;
    using Newtonsoft.Json;
    using Syncs.Responses;
    using System.Collections.Generic;

    /// <summary>A collection containing the ids of movies, shows, seasons, episodes and people, which were not found.</summary>
    public class TraktUserCustomListItemsPostResponseNotFound
    {
        /// <summary>
        /// A list of <see cref="TraktSyncPostResponseNotFoundItem{T}" />, containing the ids of movies, which were not found.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "movies")]
        public IEnumerable<TraktSyncPostResponseNotFoundItem<TraktMovieIds>> Movies { get; set; }

        /// <summary>
        /// A list of <see cref="TraktSyncPostResponseNotFoundItem{T}" />, containing the ids of shows, which were not found.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "shows")]
        public IEnumerable<TraktSyncPostResponseNotFoundItem<TraktShowIds>> Shows { get; set; }

        /// <summary>
        /// A list of <see cref="TraktSyncPostResponseNotFoundItem{T}" />, containing the ids of seasons, which were not found.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "seasons")]
        public IEnumerable<TraktSyncPostResponseNotFoundItem<TraktSeasonIds>> Seasons { get; set; }

        /// <summary>
        /// A list of <see cref="TraktSyncPostResponseNotFoundItem{T}" />, containing the ids of episodes, which were not found.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "episodes")]
        public IEnumerable<TraktSyncPostResponseNotFoundItem<TraktEpisodeIds>> Episodes { get; set; }

        /// <summary>
        /// A list of <see cref="TraktSyncPostResponseNotFoundItem{T}" />, containing the ids of people, which were not found.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "people")]
        public IEnumerable<TraktSyncPostResponseNotFoundItem<TraktPersonIds>> People { get; set; }
    }
}
