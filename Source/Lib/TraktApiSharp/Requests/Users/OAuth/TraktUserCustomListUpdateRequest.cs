﻿namespace TraktApiSharp.Requests.Users.OAuth
{
    using Base;
    using Extensions;
    using Interfaces;
    using Objects.Get.Users.Lists.Implementations;
    using Objects.Post.Users;
    using System;
    using System.Collections.Generic;

    internal sealed class TraktUserCustomListUpdateRequest : ATraktPutRequest<TraktList, TraktUserCustomListPost>, ITraktHasId
    {
        internal string Username { get; set; }

        public string Id { get; set; }

        public override TraktUserCustomListPost RequestBody { get; set; }

        public TraktRequestObjectType RequestObjectType => TraktRequestObjectType.Lists;

        public override string UriTemplate => "users/{username}/lists/{id}";

        public override IDictionary<string, object> GetUriPathParameters()
            => new Dictionary<string, object>
            {
                ["username"] = Username,
                ["id"] = Id
            };

        public override void Validate()
        {
            base.Validate();

            if (Username == null)
                throw new ArgumentNullException(nameof(Username));

            if (Username == string.Empty || Username.ContainsSpace())
                throw new ArgumentException("username not valid", nameof(Username));

            if (Id == null)
                throw new ArgumentNullException(nameof(Id));

            if (Id == string.Empty || Id.ContainsSpace())
                throw new ArgumentException("list id not valid", nameof(Id));
        }
    }
}
