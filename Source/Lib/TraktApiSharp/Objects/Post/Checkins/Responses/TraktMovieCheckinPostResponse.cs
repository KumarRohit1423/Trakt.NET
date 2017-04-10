﻿namespace TraktApiSharp.Objects.Post.Checkins.Responses
{
    using Get.Movies.Implementations;
    using Newtonsoft.Json;

    /// <summary>Represents a movie checkin response.</summary>
    public class TraktMovieCheckinPostResponse : TraktCheckinPostResponse
    {
        /// <summary>
        /// Gets or sets the Trakt movie, which was checked in.
        /// See also <seealso cref="TraktMovie" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "movie")]
        public TraktMovie Movie { get; set; }
    }
}
