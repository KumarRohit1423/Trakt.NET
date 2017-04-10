﻿namespace TraktApiSharp.Requests.Movies
{
    using Interfaces;
    using Objects.Get.Users.Implementations;
    using Parameters;
    using System.Collections.Generic;

    internal sealed class TraktMovieWatchingUsersRequest : ATraktMovieRequest<TraktUser>, ITraktSupportsExtendedInfo
    {
        public TraktExtendedInfo ExtendedInfo { get; set; }

        public override string UriTemplate => "movies/{id}/watching{?extended}";

        public override IDictionary<string, object> GetUriPathParameters()
        {
            var uriParams = base.GetUriPathParameters();

            if (ExtendedInfo != null && ExtendedInfo.HasAnySet)
                uriParams.Add("extended", ExtendedInfo.ToString());

            return uriParams;
        }
    }
}
