﻿namespace TraktApiSharp.Requests.Seasons
{
    using Base;
    using Extensions;
    using Interfaces;
    using System;
    using System.Collections.Generic;

    internal abstract class ATraktSeasonRequest<TResponseContentType> : AGetRequest<TResponseContentType>, ITraktHasId
    {
        public string Id { get; set; }

        internal uint SeasonNumber { get; set; }

        public virtual RequestObjectType RequestObjectType => RequestObjectType.Seasons;

        public override IDictionary<string, object> GetUriPathParameters()
            => new Dictionary<string, object>
            {
                ["id"] = Id,
                ["season"] = SeasonNumber.ToString()
            };

        public override void Validate()
        {
            if (Id == null)
                throw new ArgumentNullException(nameof(Id));

            if (Id == string.Empty || Id.ContainsSpace())
                throw new ArgumentException("show id not valid", nameof(Id));
        }
    }
}
