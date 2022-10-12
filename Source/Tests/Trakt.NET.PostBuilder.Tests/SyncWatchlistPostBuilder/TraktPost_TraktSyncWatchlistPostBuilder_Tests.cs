﻿namespace TraktNet.PostBuilder.Tests
{
    using FluentAssertions;
    using System;
    using Trakt.NET.Tests.Utility.Traits;
    using TraktNet.Objects.Post.Syncs.Watchlist;
    using Xunit;

    [Category("PostBuilder")]
    public partial class TraktPost_SyncWatchlistPostBuilder_Tests
    {
        [Fact]
        public void Test_TraktPost_SyncWatchlistPostBuilder_Empty_Build()
        {
            Func<ITraktSyncWatchlistPost> act = () => TraktPost.NewSyncWatchlistPost().Build();
            act.Should().Throw<ArgumentException>();
        }
    }
}
