﻿namespace TraktApiSharp.Requests.Syncs.OAuth
{
    using Enums;
    using Objects.Get.Syncs.Playback.Implementations;
    using System.Collections.Generic;

    internal sealed class TraktSyncPlaybackProgressRequest : ATraktSyncGetRequest<TraktSyncPlaybackProgressItem>
    {
        internal TraktSyncType Type { get; set; }

        internal int? Limit { get; set; }

        public override string UriTemplate => "sync/playback{/type}{?limit}";

        public override IDictionary<string, object> GetUriPathParameters()
        {
            var uriParams = new Dictionary<string, object>();

            if (Type != null && Type != TraktSyncType.Unspecified)
                uriParams.Add("type", Type.UriName);

            if (Limit.HasValue)
                uriParams.Add("limit", Limit.Value.ToString());

            return uriParams;
        }
    }
}
