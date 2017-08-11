﻿namespace TraktApiSharp.Requests.Users.OAuth
{
    using Interfaces;
    using System.Collections.Generic;

    internal abstract class ATraktUsersPagedGetRequest<TResponseContentType> : ATraktUsersGetRequest<TResponseContentType>, ITraktSupportsPagination
    {
        public int? Page { get; set; }

        public int? Limit { get; set; }

        public override IDictionary<string, object> GetUriPathParameters()
        {
            var uriParams = base.GetUriPathParameters();

            if (Page.HasValue)
                uriParams.Add("page", Page.Value.ToString());

            if (Limit.HasValue)
                uriParams.Add("limit", Limit.Value.ToString());

            return uriParams;
        }
    }
}
