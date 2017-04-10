﻿namespace TraktApiSharp.Objects.Get.People.Credits.Implementations
{
    using Movies;
    using Newtonsoft.Json;

    /// <summary>Contains information about a Trakt person's cast position.</summary>
    public class TraktPersonMovieCreditsCastItem : ITraktPersonMovieCreditsCastItem
    {
        /// <summary>Gets or sets the character name of the cast position.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "character")]
        public string Character { get; set; }

        /// <summary>
        /// Gets or sets the movie of the cast position. See also <seealso cref="ITraktMovie" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "movie")]
        public ITraktMovie Movie { get; set; }
    }
}
