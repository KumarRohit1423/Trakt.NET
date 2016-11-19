﻿namespace TraktApiSharp.Tests.Experimental.Requests.Seasons
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TraktApiSharp.Experimental.Requests.Base.Get;
    using TraktApiSharp.Experimental.Requests.Seasons;
    using TraktApiSharp.Objects.Basic;

    [TestClass]
    public class TraktSeasonStatisticsRequestTests
    {
        [TestMethod, TestCategory("Requests"), TestCategory("Seasons")]
        public void TestTraktSeasonStatisticsRequestIsNotAbstract()
        {
            typeof(TraktSeasonStatisticsRequest).IsAbstract.Should().BeFalse();
        }

        [TestMethod, TestCategory("Requests"), TestCategory("Seasons")]
        public void TestTraktSeasonStatisticsRequestIsSealed()
        {
            typeof(TraktSeasonStatisticsRequest).IsSealed.Should().BeTrue();
        }

        [TestMethod, TestCategory("Requests"), TestCategory("Seasons")]
        public void TestTraktSeasonStatisticsRequestIsSubclassOfATraktSingleItemGetByIdRequest()
        {
            typeof(TraktSeasonStatisticsRequest).IsSubclassOf(typeof(ATraktSingleItemGetByIdRequest<TraktStatistics>)).Should().BeTrue();
        }
    }
}
