﻿namespace TraktApiSharp.Objects.Basic.Implementations
{
    using Newtonsoft.Json;

    /// <summary>Represents a Trakt error response.</summary>
    public class TraktError : ITraktError
    {
        /// <summary>Gets or sets the error name.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        /// <summary>Gets or sets the error description.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "error_description")]
        public string Description { get; set; }
    }
}
