﻿namespace TraktApiSharp.Tests.Objects.Get.Calendars.Interfaces
{
    using FluentAssertions;
    using System;
    using System.Linq;
    using Traits;
    using TraktApiSharp.Objects.Get.Calendars;
    using TraktApiSharp.Objects.Get.Episodes;
    using TraktApiSharp.Objects.Get.Shows;
    using Xunit;

    [Category("Objects.Get.Calendars.Interfaces")]
    public class ITraktCalendarShow_Tests
    {
        [Fact]
        public void Test_ITraktCalendarShow_Is_Interface()
        {
            typeof(ITraktCalendarShow).IsInterface.Should().BeTrue();
        }

        [Fact]
        public void Test_ITraktCalendarShow_Inherits_ITraktShow_Interface()
        {
            typeof(ITraktCalendarShow).GetInterfaces().Should().Contain(typeof(ITraktShow));
        }

        [Fact]
        public void Test_ITraktCalendarShow_Inherits_ITraktCalendarEpisode_Interface()
        {
            typeof(ITraktCalendarShow).GetInterfaces().Should().Contain(typeof(ITraktCalendarEpisode));
        }

        [Fact]
        public void Test_ITraktCalendarShow_Has_FirstAiredInCalendar_Property()
        {
            var propertyInfo = typeof(ITraktCalendarShow).GetProperties().FirstOrDefault(p => p.Name == "FirstAiredInCalendar");

            propertyInfo.CanRead.Should().BeTrue();
            propertyInfo.CanWrite.Should().BeTrue();
            propertyInfo.PropertyType.Should().Be(typeof(DateTime?));
        }

        [Fact]
        public void Test_ITraktCalendarShow_Has_Show_Property()
        {
            var propertyInfo = typeof(ITraktCalendarShow).GetProperties().FirstOrDefault(p => p.Name == "Show");

            propertyInfo.CanRead.Should().BeTrue();
            propertyInfo.CanWrite.Should().BeTrue();
            propertyInfo.PropertyType.Should().Be(typeof(ITraktShow));
        }

        [Fact]
        public void Test_ITraktCalendarShow_Has_Episode_Property()
        {
            var propertyInfo = typeof(ITraktCalendarShow).GetProperties().FirstOrDefault(p => p.Name == "Episode");

            propertyInfo.CanRead.Should().BeTrue();
            propertyInfo.CanWrite.Should().BeTrue();
            propertyInfo.PropertyType.Should().Be(typeof(ITraktEpisode));
        }
    }
}
