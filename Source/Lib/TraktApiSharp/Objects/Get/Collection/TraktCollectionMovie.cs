﻿namespace TraktApiSharp.Objects.Get.Collection
{
    using Basic;
    using Movies;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    /// <summary>Contains information about a collected Trakt movie.</summary>
    public class TraktCollectionMovie : ITraktCollectionMovie
    {
        /// <summary>Gets or sets the UTC datetime, when the movie was collected.</summary>
        [JsonProperty(PropertyName = "collected_at")]
        public DateTime? CollectedAt { get; set; }

        /// <summary>
        /// Gets or sets the collected Trakt movie. See also <seealso cref="TraktMovie" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "movie")]
        public ITraktMovie Movie { get; set; }

        /// <summary>
        /// Gets or sets metadata about the collected movie. See also <seealso cref="TraktMetadata" />.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "metadata")]
        public TraktMetadata Metadata { get; set; }

        [JsonIgnore]
        public string Title
        {
            get { return Movie?.Title; }

            set
            {
                if (Movie != null)
                    Movie.Title = value;
            }
        }

        [JsonIgnore]
        public int? Year
        {
            get { return Movie?.Year; }

            set
            {
                if (Movie != null)
                    Movie.Year = value;
            }
        }

        [JsonIgnore]
        public TraktMovieIds Ids
        {
            get { return Movie?.Ids; }

            set
            {
                if (Movie != null)
                    Movie.Ids = value;
            }
        }

        [JsonIgnore]
        public string Tagline
        {
            get { return Movie?.Tagline; }

            set
            {
                if (Movie != null)
                    Movie.Tagline = value;
            }
        }

        [JsonIgnore]
        public string Overview
        {
            get { return Movie?.Overview; }

            set
            {
                if (Movie != null)
                    Movie.Overview = value;
            }
        }

        [JsonIgnore]
        public DateTime? Released
        {
            get { return Movie?.Released; }

            set
            {
                if (Movie != null)
                    Movie.Released = value;
            }
        }

        [JsonIgnore]
        public int? Runtime
        {
            get { return Movie?.Runtime; }

            set
            {
                if (Movie != null)
                    Movie.Runtime = value;
            }
        }

        [JsonIgnore]
        public string Trailer
        {
            get { return Movie?.Trailer; }

            set
            {
                if (Movie != null)
                    Movie.Trailer = value;
            }
        }

        [JsonIgnore]
        public string Homepage
        {
            get { return Movie?.Homepage; }

            set
            {
                if (Movie != null)
                    Movie.Homepage = value;
            }
        }

        [JsonIgnore]
        public float? Rating
        {
            get { return Movie?.Rating; }

            set
            {
                if (Movie != null)
                    Movie.Rating = value;
            }
        }

        [JsonIgnore]
        public int? Votes
        {
            get { return Movie?.Votes; }

            set
            {
                if (Movie != null)
                    Movie.Votes = value;
            }
        }

        [JsonIgnore]
        public DateTime? UpdatedAt
        {
            get { return Movie?.UpdatedAt; }

            set
            {
                if (Movie != null)
                    Movie.UpdatedAt = value;
            }
        }

        [JsonIgnore]
        public string LanguageCode
        {
            get { return Movie?.LanguageCode; }

            set
            {
                if (Movie != null)
                    Movie.LanguageCode = value;
            }
        }

        [JsonIgnore]
        public IEnumerable<string> AvailableTranslationLanguageCodes
        {
            get { return Movie?.AvailableTranslationLanguageCodes; }

            set
            {
                if (Movie != null)
                    Movie.AvailableTranslationLanguageCodes = value;
            }
        }

        [JsonIgnore]
        public IEnumerable<string> Genres
        {
            get { return Movie?.Genres; }

            set
            {
                if (Movie != null)
                    Movie.Genres = value;
            }
        }

        [JsonIgnore]
        public string Certification
        {
            get { return Movie?.Certification; }

            set
            {
                if (Movie != null)
                    Movie.Certification = value;
            }
        }
    }
}
