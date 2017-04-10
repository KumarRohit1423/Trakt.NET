﻿namespace TraktApiSharp.Objects.Get.People.Implementations
{
    using Extensions;
    using Newtonsoft.Json;
    using System;

    /// <summary>A Trakt person.</summary>
    public class TraktPerson : ITraktPerson
    {
        /// <summary>Gets or sets the person name.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of ids for the person for various web services.
        /// See also <seealso cref="ITraktPersonIds" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "ids")]
        public ITraktPersonIds Ids { get; set; }

        /// <summary>Gets or sets the biography of the person.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "biography")]
        public string Biography { get; set; }

        /// <summary>Gets or sets the UTC datetime when the person was born.</summary>
        [JsonProperty(PropertyName = "birthday")]
        public DateTime? Birthday { get; set; }

        /// <summary>Gets or sets the UTC datetime when the person died.</summary>
        [JsonProperty(PropertyName = "death")]
        public DateTime? Death { get; set; }

        /// <summary>Returns the age of the person, if <see cref="Birthday" /> is set, otherwise zero.</summary>
        public int Age => Birthday.HasValue ? (Death.HasValue ? Birthday.YearsBetween(Death) : Birthday.YearsBetween(DateTime.Now)) : 0;

        /// <summary>Gets or sets the birthplace of the person.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "birthplace")]
        public string Birthplace { get; set; }

        /// <summary>Gets or sets the web address of the homepage of the person.<para>Nullable</para></summary>
        [JsonProperty(PropertyName = "homepage")]
        public string Homepage { get; set; }
    }
}
