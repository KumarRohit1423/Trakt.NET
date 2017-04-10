﻿namespace TraktApiSharp.Objects.Get.Seasons.Implementations
{
    using Episodes;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>Represents the collection progress of a Trakt season.</summary>
    public class TraktSeasonCollectionProgress : TraktSeasonProgress, ITraktSeasonCollectionProgress
    {
        /// <summary>
        /// Gets or sets the collected episodes. See also <seealso cref="ITraktEpisodeCollectionProgress" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "episodes")]
        public IEnumerable<ITraktEpisodeCollectionProgress> Episodes { get; set; }
    }
}
