﻿namespace TraktApiSharp.Tests.Objects.Get.Episodes.Implementations
{
    using FluentAssertions;
    using System;
    using System.Threading.Tasks;
    using Traits;
    using TraktApiSharp.Objects.Get.Episodes;
    using TraktApiSharp.Objects.Get.Episodes.Implementations;
    using TraktApiSharp.Objects.Get.Episodes.JsonReader;
    using Xunit;

    [Category("Objects.Get.Episodes.Implementations")]
    public class TraktEpisodeCollectionProgress_Tests
    {
        [Fact]
        public void Test_TraktEpisodeCollectionProgress_Inherits_TraktEpisodeProgress()
        {
            typeof(TraktEpisodeCollectionProgress).IsSubclassOf(typeof(TraktEpisodeProgress)).Should().BeTrue();
        }

        [Fact]
        public void Test_TraktEpisodeCollectionProgress_Implements_ITraktEpisodeCollectionProgress_Interface()
        {
            typeof(TraktEpisodeCollectionProgress).GetInterfaces().Should().Contain(typeof(ITraktEpisodeCollectionProgress));
        }

        [Fact]
        public void Test_TraktEpisodeCollectionProgressCollectionProgress_Default_Constructor()
        {
            var episodeCollectionProgress = new TraktEpisodeCollectionProgress();

            episodeCollectionProgress.Number.Should().NotHaveValue();
            episodeCollectionProgress.Completed.Should().NotHaveValue();
            episodeCollectionProgress.CollectedAt.Should().NotHaveValue();
        }

        [Fact]
        public async Task Test_TraktEpisodeCollectionProgressCollectionProgress_From_Json()
        {
            var jsonReader = new ITraktEpisodeCollectionProgressObjectJsonReader();
            var episodeCollectionProgress = await jsonReader.ReadObjectAsync(JSON);

            episodeCollectionProgress.Should().NotBeNull();
            episodeCollectionProgress.Number.Should().Be(2);
            episodeCollectionProgress.Completed.Should().BeTrue();
            episodeCollectionProgress.CollectedAt.Should().Be(DateTime.Parse("2011-04-18T01:00:00.000Z").ToUniversalTime());
        }

        private const string JSON =
            @"{
                ""number"": 2,
                ""completed"": true,
                ""collected_at"": ""2011-04-18T01:00:00.000Z""
              }";
    }
}
