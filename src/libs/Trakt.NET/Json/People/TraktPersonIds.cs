﻿using System.Globalization;
using System.Text.Json.Serialization;

namespace TraktNET
{
    public class TraktPersonIds : ITraktIds
    {
        public uint? Trakt { get; set; }

        public string? Slug { get; set; }

        public string? IMDB { get; set; }

        public uint? TMDB { get; set; }

        [JsonIgnore]
        public bool HasAnyID => Trakt.HasValue && Trakt.Value > 0 || !string.IsNullOrWhiteSpace(Slug)
            || !string.IsNullOrWhiteSpace(IMDB) || TMDB.HasValue && TMDB.Value > 0;

        [JsonIgnore]
        public string BestID
        {
            get
            {
                if (!HasAnyID)
                    return string.Empty;

                if (Trakt.HasValue && Trakt.Value > 0)
                    return Trakt.Value.ToString(CultureInfo.InvariantCulture);

                if (!string.IsNullOrWhiteSpace(Slug))
                    return Slug!;

                if (!string.IsNullOrWhiteSpace(IMDB))
                    return IMDB!;

                if (TMDB.HasValue && TMDB.Value > 0)
                    return TMDB.Value.ToString(CultureInfo.InvariantCulture);

                return string.Empty;
            }
        }
    }
}
