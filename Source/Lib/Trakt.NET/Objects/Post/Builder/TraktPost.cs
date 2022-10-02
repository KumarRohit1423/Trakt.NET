﻿namespace TraktNet.Objects.Post
{
    public sealed class TraktPost
    {
        private TraktPost()
        {
        }

        /// <summary>Creates a new <see cref="ITraktSyncCollectionPostBuilder"/>.</summary>
        /// <returns>An <see cref="ITraktSyncCollectionPostBuilder"/> instance.</returns>
        public static ITraktSyncCollectionPostBuilder NewSyncCollectionPost() => new SyncCollectionPostBuilder();

        /// <summary>Creates a new <see cref="ITraktSyncHistoryPostBuilder"/>.</summary>
        /// <returns>An <see cref="ITraktSyncHistoryPostBuilder"/> instance.</returns>
        public static ITraktSyncHistoryPostBuilder NewSyncHistoryPost() => new SyncHistoryPostBuilder();

        public static ITraktSyncHistoryRemovePostBuilder NewSyncHistoryRemovePost() => new SyncHistoryRemovePostBuilder();

        public static ITraktSyncRatingsPostBuilder NewSyncRatingsPost() => new SyncRatingsPostBuilder();

        public static ITraktSyncRecommendationsPostBuilder NewSyncRecommendationsPost() => new SyncRecommendationsPostBuilder();

        public static ITraktSyncWatchlistPostBuilder NewSyncWatchlistPost() => new SyncWatchlistPostBuilder();

        public static ITraktUserPersonalListItemsPostBuilder NewUserPersonalListItemsPost() => new UserPersonalListItemsPostBuilder();

        public static ITraktUserHiddenItemsPostBuilder NewUserHiddenItemsPost() => new UserHiddenItemsPostBuilder();
    }
}
