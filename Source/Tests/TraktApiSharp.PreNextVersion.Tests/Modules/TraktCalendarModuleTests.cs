﻿namespace TraktApiSharp.Tests.Modules
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using TraktApiSharp.Exceptions;
    using TraktApiSharp.Extensions;
    using TraktApiSharp.Modules;
    using TraktApiSharp.Objects.Get.Calendars;
    using TraktApiSharp.Objects.Get.Calendars.Implementations;
    using TraktApiSharp.Requests.Parameters;
    using TraktApiSharp.Responses;
    using Utils;

    [TestClass]
    public class TraktCalendarModuleTests
    {
        private const string START_DATE = "Tue, 08 Mar 2016 00:00:00 GMT";
        private const string END_DATE = "Mon, 14 Mar 2016 23:59:59 GMT";

        private static DateTime DT_START_DATE;
        private static DateTime DT_END_DATE;

        [TestMethod]
        public void TestTraktCalendarModuleIsModule()
        {
            typeof(TraktBaseModule).IsAssignableFrom(typeof(TraktCalendarModule)).Should().BeTrue();
        }

        [ClassInitialize]
        public static void InitializeTests(TestContext context)
        {
            TestUtility.SetupMockHttpClient();

            if (DateTime.TryParse(START_DATE, out DT_START_DATE))
                DT_START_DATE = DT_START_DATE.ToUniversalTime();

            if (DateTime.TryParse(END_DATE, out DT_END_DATE))
                DT_END_DATE = DT_END_DATE.ToUniversalTime();
        }

        [ClassCleanup]
        public static void CleanupTests()
        {
            TestUtility.ResetMockHttpClient();
        }

        [TestCleanup]
        public void CleanupSingleTest()
        {
            TestUtility.ClearMockHttpClient();
        }

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region AllShows

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShows()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                "calendars/all/shows", calendarShowsJson,
                START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync().Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows?{filter.ToString()}", calendarShowsJson,
                START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(null, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithStartDate()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/{today.ToTraktDateString()}", calendarShowsJson,
                START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(today).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithStartDateFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/{today.ToTraktDateString()}?{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(today, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithDays()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/{today.ToTraktDateString()}/{days}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(null, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithDaysFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(null, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithStartDateAndDays()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/{today.ToTraktDateString()}/{days}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(today, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithStartDateAndDaysFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(today, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithExtendedInfo()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows?extended={extendedInfo.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(null, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithExtendedInfoFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(null, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithExtendedInfoAndStartDate()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(today, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithExtendedInfoAndStartDateFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(today, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithExtendedInfoAndDays()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(null, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithExtendedInfoAndDaysFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(null, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithExtendedInfoAndStartDateAndDays()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(today, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsWithExtendedInfoAndStartDateAndDaysFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/{today.ToTraktDateString()}/{days}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(today, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsExceptions()
        {
            var uri = $"calendars/all/shows";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);

            Func<Task<TraktListResponse<TraktCalendarShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync();
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.MethodNotAllowed);
            act.ShouldThrow<TraktMethodNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Conflict);
            act.ShouldThrow<TraktConflictException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.BadGateway);
            act.ShouldThrow<TraktBadGatewayException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)422);
            act.ShouldThrow<TraktValidationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllShowsArgumentExceptions()
        {
            Func<Task<TraktListResponse<TraktCalendarShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(null, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllShowsAsync(null, 32);
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region AllNewShows

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShows()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                "calendars/all/shows/new",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync().Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new?{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(null, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithStartDate()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new/{today.ToTraktDateString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(today).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithStartDateFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new/{today.ToTraktDateString()}?{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(today, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithDays()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new/{today.ToTraktDateString()}/{days}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(null, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithDaysFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(null, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithStartDateAndDays()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new/{today.ToTraktDateString()}/{days}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(today, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithStartDateAndDaysFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(today, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithExtendedInfo()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new?extended={extendedInfo.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(null, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithExtendedInfoFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(null, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithExtendedInfoAndStartDate()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(today, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithExtendedInfoAndStartDateFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new/{today.ToTraktDateString()}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(today, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithExtendedInfoAndDays()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(null, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithExtendedInfoAndDaysFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(null, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithExtendedInfoAndStartDateAndDays()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(today, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsWithExtendedInfoAndStartDateAndDaysFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/new/{today.ToTraktDateString()}/{days}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(today, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsExceptions()
        {
            var uri = $"calendars/all/shows/new";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);

            Func<Task<TraktListResponse<TraktCalendarShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync();
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.MethodNotAllowed);
            act.ShouldThrow<TraktMethodNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Conflict);
            act.ShouldThrow<TraktConflictException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.BadGateway);
            act.ShouldThrow<TraktBadGatewayException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)422);
            act.ShouldThrow<TraktValidationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllNewShowsArgumentExceptions()
        {
            Func<Task<TraktListResponse<TraktCalendarShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(null, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllNewShowsAsync(null, 32);
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region AllSeasonPremieres

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieres()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                "calendars/all/shows/premieres",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync().Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres?{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(null, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithStartDate()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres/{today.ToTraktDateString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(today).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithStartDateFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres/{today.ToTraktDateString()}?{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(today, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithDays()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres/{today.ToTraktDateString()}/{days}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(null, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithDaysFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(null, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithStartDateAndDays()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres/{today.ToTraktDateString()}/{days}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(today, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithStartDateAndDaysFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(today, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithExtendedInfo()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres?extended={extendedInfo.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(null, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithExtendedInfoFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(null, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithExtendedInfoAndStartDate()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(today, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithExtendedInfoAndStartDateFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres/{today.ToTraktDateString()}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(today, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithExtendedInfoAndDays()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(null, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithExtendedInfoAndDaysFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(null, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithExtendedInfoAndStartDateAndDays()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(today, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresWithExtendedInfoAndStartDateAndDaysFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/shows/premieres/{today.ToTraktDateString()}/{days}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(today, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresExceptions()
        {
            var uri = $"calendars/all/shows/premieres";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);

            Func<Task<TraktListResponse<TraktCalendarShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync();
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.MethodNotAllowed);
            act.ShouldThrow<TraktMethodNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Conflict);
            act.ShouldThrow<TraktConflictException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.BadGateway);
            act.ShouldThrow<TraktBadGatewayException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)422);
            act.ShouldThrow<TraktValidationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllSeasonPremieresArgumentExceptions()
        {
            Func<Task<TraktListResponse<TraktCalendarShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(null, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllSeasonPremieresAsync(null, 32);
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region AllMovies

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMovies()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                "calendars/all/movies",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync().Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies?{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(null, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithStartDate()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies/{today.ToTraktDateString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(today).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithStartDateFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies/{today.ToTraktDateString()}?{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(today, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithDays()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies/{today.ToTraktDateString()}/{days}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(null, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithDaysFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(null, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithStartDateAndDays()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies/{today.ToTraktDateString()}/{days}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(today, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithStartDateAndDaysFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(today, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithExtendedInfo()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies?extended={extendedInfo.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(null, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithExtendedInfoFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(null, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithExtendedInfoAndStartDate()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(today, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithExtendedInfoAndStartDateFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies/{today.ToTraktDateString()}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(today, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithExtendedInfoAndDays()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(null, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithExtendedInfoAndDaysFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(null, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithExtendedInfoAndStartDateAndDays()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(today, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesWithExtendedInfoAndStartDateAndDaysFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/movies/{today.ToTraktDateString()}/{days}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(today, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesExceptions()
        {
            var uri = $"calendars/all/movies";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);

            Func<Task<TraktListResponse<TraktCalendarMovie>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync();
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.MethodNotAllowed);
            act.ShouldThrow<TraktMethodNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Conflict);
            act.ShouldThrow<TraktConflictException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.BadGateway);
            act.ShouldThrow<TraktBadGatewayException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)422);
            act.ShouldThrow<TraktValidationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllMoviesArgumentExceptions()
        {
            Func<Task<TraktListResponse<TraktCalendarMovie>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(null, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllMoviesAsync(null, 32);
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region AllDVDMovies

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMovies()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                "calendars/all/dvd",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync().Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd?{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(null, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithStartDate()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd/{today.ToTraktDateString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(today).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithStartDateFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd/{today.ToTraktDateString()}?{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(today, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithDays()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd/{today.ToTraktDateString()}/{days}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(null, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithDaysFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(null, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithStartDateAndDays()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd/{today.ToTraktDateString()}/{days}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(today, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithStartDateAndDaysFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(today, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithExtendedInfo()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd?extended={extendedInfo.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(null, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithExtendedInfoFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(null, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithExtendedInfoAndStartDate()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(today, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithExtendedInfoAndStartDateFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd/{today.ToTraktDateString()}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(today, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithExtendedInfoAndDays()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(null, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithExtendedInfoAndDaysFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(null, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithExtendedInfoAndStartDateAndDays()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(today, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesWithExtendedInfoAndStartDateAndDaysFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithoutOAuthWithHeaders(
                $"calendars/all/dvd/{today.ToTraktDateString()}/{days}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(today, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesExceptions()
        {
            var uri = $"calendars/all/dvd";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);

            Func<Task<TraktListResponse<TraktCalendarMovie>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync();
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.MethodNotAllowed);
            act.ShouldThrow<TraktMethodNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Conflict);
            act.ShouldThrow<TraktConflictException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.BadGateway);
            act.ShouldThrow<TraktBadGatewayException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)422);
            act.ShouldThrow<TraktValidationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetAllDVDMoviesArgumentExceptions()
        {
            Func<Task<TraktListResponse<TraktCalendarMovie>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(null, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetAllDVDMoviesAsync(null, 32);
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region UserShows

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShows()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                "calendars/my/shows",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync().Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows?{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(null, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithStartDate()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/{today.ToTraktDateString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(today).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithStartDateFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/{today.ToTraktDateString()}?{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(today, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithDays()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/{today.ToTraktDateString()}/{days}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(null, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithDaysFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(null, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithStartDateAndDays()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/{today.ToTraktDateString()}/{days}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(today, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithStartDateAndDaysFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(today, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithExtendedInfo()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows?extended={extendedInfo.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(null, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithExtendedInfoFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(null, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithExtendedInfoAndStartDate()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(today, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithExtendedInfoAndStartDateFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(today, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithExtendedInfoAndDays()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(null, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithExtendedInfoAndDaysFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(null, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithExtendedInfoAndStartDateAndDays()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(today, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsWithExtendedInfoAndStartDateAndDaysFiltered()
        {
            var calendarShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(today, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsExceptions()
        {
            var uri = $"calendars/my/shows";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);

            Func<Task<TraktListResponse<TraktCalendarShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync();
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.MethodNotAllowed);
            act.ShouldThrow<TraktMethodNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.Conflict);
            act.ShouldThrow<TraktConflictException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.BadGateway);
            act.ShouldThrow<TraktBadGatewayException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)422);
            act.ShouldThrow<TraktValidationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserShowsArgumentExceptions()
        {
            Func<Task<TraktListResponse<TraktCalendarShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(null, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserShowsAsync(null, 32);
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region UserNewShows

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShows()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                "calendars/my/shows/new",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync().Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new?{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(null, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithStartDate()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new/{today.ToTraktDateString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(today).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithStartDateFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new/{today.ToTraktDateString()}?{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(today, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithDays()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new/{today.ToTraktDateString()}/{days}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(null, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithDaysFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(null, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithStartDateAndDays()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new/{today.ToTraktDateString()}/{days}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(today, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithStartDateAndDaysFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(today, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithExtendedInfo()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new?extended={extendedInfo.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(null, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithExtendedInfoFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(null, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithExtendedInfoAndStartDate()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(today, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithExtendedInfoAndStartDateFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(today, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithExtendedInfoAndDays()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(null, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithExtendedInfoAndDaysFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(null, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithExtendedInfoAndStartDateAndDays()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(today, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsWithExtendedInfoAndStartDateAndDaysFiltered()
        {
            var calendarNewShowsJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarNewShowsJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user new show")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/new/{today.ToTraktDateString()}/{days}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarNewShowsJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(today, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsExceptions()
        {
            var uri = $"calendars/my/shows/new";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);

            Func<Task<TraktListResponse<TraktCalendarShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync();
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.MethodNotAllowed);
            act.ShouldThrow<TraktMethodNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.Conflict);
            act.ShouldThrow<TraktConflictException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.BadGateway);
            act.ShouldThrow<TraktBadGatewayException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)422);
            act.ShouldThrow<TraktValidationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserNewShowsArgumentExceptions()
        {
            Func<Task<TraktListResponse<TraktCalendarShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(null, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserNewShowsAsync(null, 32);
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region UserSeasonPremieres

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieres()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                "calendars/my/shows/premieres",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync().Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres?{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(null, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithStartDate()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres/{today.ToTraktDateString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(today).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithStartDateFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres/{today.ToTraktDateString()}?{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(today, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithDays()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres/{today.ToTraktDateString()}/{days}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(null, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithDaysFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(null, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithStartDateAndDays()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres/{today.ToTraktDateString()}/{days}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(today, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithStartDateAndDaysFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(today, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithExtendedInfo()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres?extended={extendedInfo.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(null, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithExtendedInfoFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(null, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithExtendedInfoAndStartDate()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(today, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithExtendedInfoAndStartDateFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres/{today.ToTraktDateString()}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(today, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithExtendedInfoAndDays()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(null, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithExtendedInfoAndDaysFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(null, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithExtendedInfoAndStartDateAndDays()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(today, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresWithExtendedInfoAndStartDateAndDaysFiltered()
        {
            var calendarSeasonPremieresJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllShows.json");
            calendarSeasonPremieresJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user season premiere")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/shows/premieres/{today.ToTraktDateString()}/{days}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarSeasonPremieresJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(today, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(2);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresExceptions()
        {
            var uri = $"calendars/my/shows/premieres";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);

            Func<Task<TraktListResponse<TraktCalendarShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync();
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.MethodNotAllowed);
            act.ShouldThrow<TraktMethodNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.Conflict);
            act.ShouldThrow<TraktConflictException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.BadGateway);
            act.ShouldThrow<TraktBadGatewayException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)422);
            act.ShouldThrow<TraktValidationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserSeasonPremieresArgumentExceptions()
        {
            Func<Task<TraktListResponse<TraktCalendarShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(null, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserSeasonPremieresAsync(null, 32);
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region UserMovies

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMovies()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                "calendars/my/movies",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync().Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies?{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(null, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithStartDate()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies/{today.ToTraktDateString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(today).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithStartDateFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies/{today.ToTraktDateString()}?{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(today, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithDays()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies/{today.ToTraktDateString()}/{days}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(null, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithDaysFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(null, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithStartDateAndDays()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies/{today.ToTraktDateString()}/{days}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(today, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithStartDateAndDaysFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(today, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithExtendedInfo()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies?extended={extendedInfo.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(null, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithExtendedInfoFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(null, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithExtendedInfoAndStartDate()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(today, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithExtendedInfoAndStartDateFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies/{today.ToTraktDateString()}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(today, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithExtendedInfoAndDays()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(null, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithExtendedInfoAndDaysFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(null, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithExtendedInfoAndStartDateAndDays()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(today, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesWithExtendedInfoAndStartDateAndDaysFiltered()
        {
            var calendarMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarAllMovies.json");
            calendarMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/movies/{today.ToTraktDateString()}/{days}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(today, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesExceptions()
        {
            var uri = $"calendars/my/movies";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);

            Func<Task<TraktListResponse<TraktCalendarMovie>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync();
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.MethodNotAllowed);
            act.ShouldThrow<TraktMethodNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.Conflict);
            act.ShouldThrow<TraktConflictException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.BadGateway);
            act.ShouldThrow<TraktBadGatewayException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)422);
            act.ShouldThrow<TraktValidationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserMoviesArgumentExceptions()
        {
            Func<Task<TraktListResponse<TraktCalendarMovie>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(null, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserMoviesAsync(null, 32);
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region UserDVDMovies

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMovies()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                "calendars/my/dvd",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync().Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd?{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(null, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithStartDate()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd/{today.ToTraktDateString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(today).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithStartDateFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd/{today.ToTraktDateString()}?{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(today, null, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithDays()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd/{today.ToTraktDateString()}/{days}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(null, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithDaysFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(null, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithStartDateAndDays()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd/{today.ToTraktDateString()}/{days}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(today, days).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithStartDateAndDaysFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd/{today.ToTraktDateString()}/{days}?{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(today, days, null, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithExtendedInfo()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd?extended={extendedInfo.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(null, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithExtendedInfoFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(null, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithExtendedInfoAndStartDate()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd/{today.ToTraktDateString()}?extended={extendedInfo.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(today, null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithExtendedInfoAndStartDateFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd/{today.ToTraktDateString()}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(today, null, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithExtendedInfoAndDays()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(null, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithExtendedInfoAndDaysFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(null, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithExtendedInfoAndStartDateAndDays()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd/{today.ToTraktDateString()}/{days}?extended={extendedInfo.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(today, days, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesWithExtendedInfoAndStartDateAndDaysFiltered()
        {
            var calendarDVDMoviesJson = TestUtility.ReadFileContents(@"Objects\Get\Calendars\CalendarDVDMovies.json");
            calendarDVDMoviesJson.Should().NotBeNullOrEmpty();

            var today = DateTime.UtcNow;
            var days = 14;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            var filter = new TraktCalendarFilter()
                .WithQuery("calendar user movie")
                .WithStartYear(2016)
                .WithGenres("drama", "fantasy")
                .WithLanguages("en", "de")
                .WithCountries("us")
                .WithRuntimes(30, 60)
                .WithRatings(80, 95);

            TestUtility.SetupMockResponseWithOAuthWithHeaders(
                $"calendars/my/dvd/{today.ToTraktDateString()}/{days}" +
                $"?extended={extendedInfo.ToString()}&{filter.ToString()}",
                calendarDVDMoviesJson, START_DATE, END_DATE);

            var response = TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(today, days, extendedInfo, filter).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.StartDate.Should().HaveValue();
            response.StartDate.Equals(DT_START_DATE).Should().BeTrue();
            response.EndDate.Should().HaveValue();
            response.EndDate.Equals(DT_END_DATE).Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesExceptions()
        {
            var uri = $"calendars/my/dvd";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);

            Func<Task<TraktListResponse<TraktCalendarMovie>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync();
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.MethodNotAllowed);
            act.ShouldThrow<TraktMethodNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.Conflict);
            act.ShouldThrow<TraktConflictException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.BadGateway);
            act.ShouldThrow<TraktBadGatewayException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)422);
            act.ShouldThrow<TraktValidationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktCalendarModuleGetUserDVDMoviesArgumentExceptions()
        {
            Func<Task<TraktListResponse<TraktCalendarMovie>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(null, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Calendar.GetUserDVDMoviesAsync(null, 32);
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        #endregion
    }
}
