﻿namespace TraktApiSharp.Tests.Objects.Get.Users.Statistics.Interfaces
{
    using FluentAssertions;
    using System.Linq;
    using Traits;
    using TraktApiSharp.Objects.Get.Users.Statistics;
    using Xunit;

    [Category("Objects.Get.Users.Statistics.Interfaces")]
    public class ITraktUserEpisodesStatistics_Tests
    {
        [Fact]
        public void Test_ITraktUserEpisodesStatistics_Is_Interface()
        {
            typeof(ITraktUserEpisodesStatistics).IsInterface.Should().BeTrue();
        }

        [Fact]
        public void Test_ITraktUserEpisodesStatistics_Has_Plays_Property()
        {
            var propertyInfo = typeof(ITraktUserEpisodesStatistics).GetProperties().FirstOrDefault(p => p.Name == "Plays");

            propertyInfo.CanRead.Should().BeTrue();
            propertyInfo.CanWrite.Should().BeTrue();
            propertyInfo.PropertyType.Should().Be(typeof(int?));
        }

        [Fact]
        public void Test_ITraktUserEpisodesStatistics_Has_Watched_Property()
        {
            var propertyInfo = typeof(ITraktUserEpisodesStatistics).GetProperties().FirstOrDefault(p => p.Name == "Watched");

            propertyInfo.CanRead.Should().BeTrue();
            propertyInfo.CanWrite.Should().BeTrue();
            propertyInfo.PropertyType.Should().Be(typeof(int?));
        }

        [Fact]
        public void Test_ITraktUserEpisodesStatistics_Has_Minutes_Property()
        {
            var propertyInfo = typeof(ITraktUserEpisodesStatistics).GetProperties().FirstOrDefault(p => p.Name == "Minutes");

            propertyInfo.CanRead.Should().BeTrue();
            propertyInfo.CanWrite.Should().BeTrue();
            propertyInfo.PropertyType.Should().Be(typeof(int?));
        }

        [Fact]
        public void Test_ITraktUserEpisodesStatistics_Has_Collected_Property()
        {
            var propertyInfo = typeof(ITraktUserEpisodesStatistics).GetProperties().FirstOrDefault(p => p.Name == "Collected");

            propertyInfo.CanRead.Should().BeTrue();
            propertyInfo.CanWrite.Should().BeTrue();
            propertyInfo.PropertyType.Should().Be(typeof(int?));
        }

        [Fact]
        public void Test_ITraktUserEpisodesStatistics_Has_Ratings_Property()
        {
            var propertyInfo = typeof(ITraktUserEpisodesStatistics).GetProperties().FirstOrDefault(p => p.Name == "Ratings");

            propertyInfo.CanRead.Should().BeTrue();
            propertyInfo.CanWrite.Should().BeTrue();
            propertyInfo.PropertyType.Should().Be(typeof(int?));
        }

        [Fact]
        public void Test_ITraktUserEpisodesStatistics_Has_Comments_Property()
        {
            var propertyInfo = typeof(ITraktUserEpisodesStatistics).GetProperties().FirstOrDefault(p => p.Name == "Comments");

            propertyInfo.CanRead.Should().BeTrue();
            propertyInfo.CanWrite.Should().BeTrue();
            propertyInfo.PropertyType.Should().Be(typeof(int?));
        }
    }
}
