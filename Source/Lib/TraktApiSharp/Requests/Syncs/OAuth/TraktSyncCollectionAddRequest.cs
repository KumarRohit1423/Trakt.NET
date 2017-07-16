﻿namespace TraktApiSharp.Requests.Syncs.OAuth
{
    using Objects.Post.Syncs.Collection;
    using Objects.Post.Syncs.Collection.Responses.Implementations;

    internal sealed class TraktSyncCollectionAddRequest : ATraktSyncPostRequest<TraktSyncCollectionPostResponse, TraktSyncCollectionPost>
    {
        public override string UriTemplate => "sync/collection";
    }
}
