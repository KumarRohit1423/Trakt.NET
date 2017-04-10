﻿namespace TraktApiSharp.Requests.Comments
{
    using Base;
    using Extensions;
    using Interfaces;
    using Objects.Basic.Implementations;
    using System;
    using System.Collections.Generic;

    internal sealed class TraktCommentSummaryRequest : ATraktGetRequest<TraktComment>, ITraktHasId
    {
        public string Id { get; set; }

        public TraktRequestObjectType RequestObjectType => TraktRequestObjectType.Comments;

        public override string UriTemplate => "comments/{id}";

        public override IDictionary<string, object> GetUriPathParameters() => new Dictionary<string, object> { ["id"] = Id };

        public override void Validate()
        {
            if (Id == null)
                throw new ArgumentNullException(nameof(Id));

            if (Id == string.Empty || Id.ContainsSpace())
                throw new ArgumentException("comment id not valid", nameof(Id));
        }
    }
}
