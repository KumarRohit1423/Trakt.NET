﻿namespace TraktApiSharp.Requests.Syncs.OAuth
{
    using Enums;
    using Extensions;
    using Interfaces;
    using Objects.Get.History.Implementations;
    using Parameters;
    using System;
    using System.Collections.Generic;

    internal sealed class TraktSyncWatchedHistoryRequest : ATraktSyncGetRequest<TraktHistoryItem>, ITraktSupportsExtendedInfo, ITraktSupportsPagination
    {
        internal TraktSyncItemType Type { get; set; }

        internal uint? ItemId { get; set; }

        internal DateTime? StartAt { get; set; }

        internal DateTime? EndAt { get; set; }

        public TraktExtendedInfo ExtendedInfo { get; set; }

        public int? Page { get; set; }

        public int? Limit { get; set; }

        public override string UriTemplate => "sync/history{/type}{/item_id}{?start_at,end_at,extended,page,limit}";

        public override IDictionary<string, object> GetUriPathParameters()
        {
            var uriParams = new Dictionary<string, object>();

            var isTypeSetAndValid = Type != null && Type != TraktSyncItemType.Unspecified;

            if (isTypeSetAndValid)
                uriParams.Add("type", Type.UriName);

            if (isTypeSetAndValid && ItemId.HasValue && ItemId.Value > 0)
                uriParams.Add("item_id", ItemId.ToString());

            if (StartAt.HasValue)
                uriParams.Add("start_at", StartAt.Value.ToTraktLongDateTimeString());

            if (EndAt.HasValue)
                uriParams.Add("end_at", EndAt.Value.ToTraktLongDateTimeString());

            if (ExtendedInfo != null && ExtendedInfo.HasAnySet)
                uriParams.Add("extended", ExtendedInfo.ToString());

            if (Page.HasValue)
                uriParams.Add("page", Page.Value.ToString());

            if (Limit.HasValue)
                uriParams.Add("limit", Limit.Value.ToString());

            return uriParams;
        }
    }
}
