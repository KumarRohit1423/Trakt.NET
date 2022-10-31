﻿namespace TraktNet.Objects.Post.Tests.Syncs.Recommendations.Responses.Json.Reader
{
    using FluentAssertions;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Trakt.NET.Tests.Utility;
    using Trakt.NET.Tests.Utility.Traits;
    using TraktNet.Objects.Post.Syncs.Recommendations;
    using TraktNet.Objects.Post.Syncs.Recommendations.Responses;
    using TraktNet.Objects.Post.Syncs.Recommendations.Responses.Json.Reader;
    using Xunit;

    [TestCategory("Objects.Post.Syncs.Recommendations.Responses.JsonReader")]
    public partial class SyncRecommendationsPostResponseNotFoundGroupObjectJsonReader_Tests
    {
        [Fact]
        public async Task Test_SyncRecommendationsPostResponseNotFoundGroupObjectJsonReader_ReadObject_From_Stream_Complete()
        {
            var traktJsonReader = new SyncRecommendationsPostResponseNotFoundGroupObjectJsonReader();

            using var stream = JSON_COMPLETE.ToStream();
            ITraktSyncRecommendationsPostResponseNotFoundGroup traktSyncRecommendationsPostResponseNotFoundGroup = await traktJsonReader.ReadObjectAsync(stream);

            traktSyncRecommendationsPostResponseNotFoundGroup.Should().NotBeNull();
            traktSyncRecommendationsPostResponseNotFoundGroup.Movies.Should().NotBeNull().And.HaveCount(1);

            ITraktSyncRecommendationsPostMovie[] notFoundMovies = traktSyncRecommendationsPostResponseNotFoundGroup.Movies.ToArray();

            notFoundMovies[0].Should().NotBeNull();
            notFoundMovies[0].Ids.Should().NotBeNull();
            notFoundMovies[0].Ids.Trakt.Should().Be(0U);
            notFoundMovies[0].Ids.Slug.Should().BeNull();
            notFoundMovies[0].Ids.Imdb.Should().Be("tt0000111");
            notFoundMovies[0].Ids.Tmdb.Should().BeNull();

            traktSyncRecommendationsPostResponseNotFoundGroup.Shows.Should().NotBeNull().And.HaveCount(1);

            ITraktSyncRecommendationsPostShow[] notFoundShows = traktSyncRecommendationsPostResponseNotFoundGroup.Shows.ToArray();

            notFoundShows[0].Should().NotBeNull();
            notFoundShows[0].Ids.Should().NotBeNull();
            notFoundShows[0].Ids.Trakt.Should().Be(0U);
            notFoundShows[0].Ids.Slug.Should().BeNull();
            notFoundShows[0].Ids.Imdb.Should().Be("tt0000222");
            notFoundShows[0].Ids.Tvdb.Should().BeNull();
            notFoundShows[0].Ids.Tmdb.Should().BeNull();
        }

        [Fact]
        public async Task Test_SyncRecommendationsPostResponseNotFoundGroupObjectJsonReader_ReadObject_From_Stream_Null()
        {
            var traktJsonReader = new SyncRecommendationsPostResponseNotFoundGroupObjectJsonReader();
            Func<Task<ITraktSyncRecommendationsPostResponseNotFoundGroup>> traktSyncRecommendationsPostResponseNotFoundGroup = () => traktJsonReader.ReadObjectAsync(default(Stream));
            await traktSyncRecommendationsPostResponseNotFoundGroup.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Test_SyncRecommendationsPostResponseNotFoundGroupObjectJsonReader_ReadObject_From_Stream_Empty()
        {
            var traktJsonReader = new SyncRecommendationsPostResponseNotFoundGroupObjectJsonReader();

            using var stream = string.Empty.ToStream();
            ITraktSyncRecommendationsPostResponseNotFoundGroup traktSyncRecommendationsPostResponseNotFoundGroup = await traktJsonReader.ReadObjectAsync(stream);
            traktSyncRecommendationsPostResponseNotFoundGroup.Should().BeNull();
        }
    }
}
