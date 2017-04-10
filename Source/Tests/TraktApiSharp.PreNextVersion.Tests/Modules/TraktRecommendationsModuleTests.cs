﻿namespace TraktApiSharp.Tests.Modules
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using TraktApiSharp.Exceptions;
    using TraktApiSharp.Modules;
    using TraktApiSharp.Objects.Get.Movies;
    using TraktApiSharp.Objects.Get.Movies.Implementations;
    using TraktApiSharp.Objects.Get.Shows;
    using TraktApiSharp.Objects.Get.Shows.Implementations;
    using TraktApiSharp.Requests.Parameters;
    using TraktApiSharp.Responses;
    using Utils;

    [TestClass]
    public class TraktRecommendationsModuleTests
    {
        [TestMethod]
        public void TestTraktRecommendationsModuleIsModule()
        {
            typeof(TraktBaseModule).IsAssignableFrom(typeof(TraktRecommendationsModule)).Should().BeTrue();
        }

        [ClassInitialize]
        public static void InitializeTests(TestContext context)
        {
            TestUtility.SetupMockHttpClient();
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

        #region UserMovieRecommendations

        [TestMethod]
        public void TestTraktRecommendationsModuleGetUserMovieRecommendations()
        {
            var movies = TestUtility.ReadFileContents(@"Objects\Get\Recommendations\MovieRecommendations.json");
            movies.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockPaginationResponseWithOAuth($"recommendations/movies", movies, 1, 10);

            var response = TestUtility.MOCK_TEST_CLIENT.Recommendations.GetMovieRecommendationsAsync().Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().HaveValue().And.Be(1);
            response.Limit.Should().HaveValue().And.Be(10);
        }

        [TestMethod]
        public void TestTraktRecommendationsModuleGetUserMovieRecommendationsWithLimit()
        {
            var movies = TestUtility.ReadFileContents(@"Objects\Get\Recommendations\MovieRecommendations.json");
            movies.Should().NotBeNullOrEmpty();

            var limit = 4U;

            TestUtility.SetupMockPaginationResponseWithOAuth($"recommendations/movies?limit={limit}", movies, 1, (int)limit);

            var response = TestUtility.MOCK_TEST_CLIENT.Recommendations.GetMovieRecommendationsAsync(limit).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().HaveValue().And.Be(1);
            response.Limit.Should().HaveValue().And.Be((int)limit);
        }

        [TestMethod]
        public void TestTraktRecommendationsModuleGetUserMovieRecommendationsWithExtendedInfo()
        {
            var movies = TestUtility.ReadFileContents(@"Objects\Get\Recommendations\MovieRecommendations.json");
            movies.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockPaginationResponseWithOAuth($"recommendations/movies?extended={extendedInfo.ToString()}",
                                                             movies, 1, 10);

            var response = TestUtility.MOCK_TEST_CLIENT.Recommendations.GetMovieRecommendationsAsync(null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().HaveValue().And.Be(1);
            response.Limit.Should().HaveValue().And.Be(10);
        }

        [TestMethod]
        public void TestTraktRecommendationsModuleGetUserMovieRecommendationsComplete()
        {
            var movies = TestUtility.ReadFileContents(@"Objects\Get\Recommendations\MovieRecommendations.json");
            movies.Should().NotBeNullOrEmpty();

            var limit = 4U;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockPaginationResponseWithOAuth(
                $"recommendations/movies?extended={extendedInfo.ToString()}&limit={limit}",
                movies, 1, (int)limit);

            var response = TestUtility.MOCK_TEST_CLIENT.Recommendations.GetMovieRecommendationsAsync(limit, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().HaveValue().And.Be(1);
            response.Limit.Should().HaveValue().And.Be((int)limit);
        }

        [TestMethod]
        public void TestTraktRecommendationsModuleGetUserMovieRecommendationsExceptions()
        {
            var uri = $"recommendations/movies";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);

            Func<Task<TraktPagedResponse<TraktMovie>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Recommendations.GetMovieRecommendationsAsync();
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

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region HiderUserMovieRecommentation

        [TestMethod]
        public void TestTraktRecommendationsModuleHideUserMovieRecommendation()
        {
            var movieId = "94024";

            TestUtility.SetupMockResponseWithOAuth($"recommendations/movies/{movieId}", HttpStatusCode.NoContent);

            var response = TestUtility.MOCK_TEST_CLIENT.Recommendations.HideMovieRecommendationAsync(movieId).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktRecommendationsModuleHideUserMovieRecommendationExceptions()
        {
            var movieId = "94024";
            var uri = $"recommendations/movies/{movieId}";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);

            Func<Task<TraktNoContentResponse>> act = async () => await TestUtility.MOCK_TEST_CLIENT.Recommendations.HideMovieRecommendationAsync(movieId);
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktMovieNotFoundException>();

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
        public void TestTraktRecommendationsModuleHideUserMovieRecommendationArgumentExceptions()
        {
            var movieId = "94024";

            TestUtility.SetupMockResponseWithOAuth($"recommendations/movies/{movieId}", HttpStatusCode.NoContent);

            Func<Task<TraktNoContentResponse>> act = async () => await TestUtility.MOCK_TEST_CLIENT.Recommendations.HideMovieRecommendationAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Recommendations.HideMovieRecommendationAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Recommendations.HideMovieRecommendationAsync("movie id");
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region UserShowRecommendations

        [TestMethod]
        public void TestTraktRecommendationsModuleGetUserShowRecommendations()
        {
            var shows = TestUtility.ReadFileContents(@"Objects\Get\Recommendations\ShowRecommendations.json");
            shows.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockPaginationResponseWithOAuth($"recommendations/shows", shows, 1, 10);

            var response = TestUtility.MOCK_TEST_CLIENT.Recommendations.GetShowRecommendationsAsync().Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().HaveValue().And.Be(1);
            response.Limit.Should().HaveValue().And.Be(10);
        }

        [TestMethod]
        public void TestTraktRecommendationsModuleGetUserShowRecommendationsWithLimit()
        {
            var shows = TestUtility.ReadFileContents(@"Objects\Get\Recommendations\ShowRecommendations.json");
            shows.Should().NotBeNullOrEmpty();

            var limit = 4U;

            TestUtility.SetupMockPaginationResponseWithOAuth($"recommendations/shows?limit={limit}", shows, 1, (int)limit);

            var response = TestUtility.MOCK_TEST_CLIENT.Recommendations.GetShowRecommendationsAsync(limit).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().HaveValue().And.Be(1);
            response.Limit.Should().HaveValue().And.Be((int)limit);
        }

        [TestMethod]
        public void TestTraktRecommendationsModuleGetUserShowRecommendationsWithExtendedInfo()
        {
            var shows = TestUtility.ReadFileContents(@"Objects\Get\Recommendations\ShowRecommendations.json");
            shows.Should().NotBeNullOrEmpty();

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockPaginationResponseWithOAuth($"recommendations/shows?extended={extendedInfo.ToString()}",
                                                             shows, 1, 10);

            var response = TestUtility.MOCK_TEST_CLIENT.Recommendations.GetShowRecommendationsAsync(null, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().HaveValue().And.Be(1);
            response.Limit.Should().HaveValue().And.Be(10);
        }

        [TestMethod]
        public void TestTraktRecommendationsModuleGetUserShowRecommendationsComplete()
        {
            var shows = TestUtility.ReadFileContents(@"Objects\Get\Recommendations\ShowRecommendations.json");
            shows.Should().NotBeNullOrEmpty();

            var limit = 4U;

            var extendedInfo = new TraktExtendedInfo { Full = true };

            TestUtility.SetupMockPaginationResponseWithOAuth(
                $"recommendations/shows?extended={extendedInfo.ToString()}&limit={limit}",
                shows, 1, (int)limit);

            var response = TestUtility.MOCK_TEST_CLIENT.Recommendations.GetShowRecommendationsAsync(limit, extendedInfo).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().HaveValue().And.Be(1);
            response.Limit.Should().HaveValue().And.Be((int)limit);
        }

        [TestMethod]
        public void TestTraktRecommendationsModuleGetUserShowRecommendationsExceptions()
        {
            var uri = $"recommendations/shows";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);

            Func<Task<TraktPagedResponse<TraktShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Recommendations.GetShowRecommendationsAsync();
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

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region HiderUserShowRecommentation

        [TestMethod]
        public void TestTraktRecommendationsModuleHideUserShowRecommendation()
        {
            var showId = "1390";

            TestUtility.SetupMockResponseWithOAuth($"recommendations/shows/{showId}", HttpStatusCode.NoContent);

            var response = TestUtility.MOCK_TEST_CLIENT.Recommendations.HideShowRecommendationAsync(showId).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
        }

        [TestMethod]
        public void TestTraktRecommendationsModuleHideUserShowRecommendationExceptions()
        {
            var showId = "1390";
            var uri = $"recommendations/shows/{showId}";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);

            Func<Task<TraktNoContentResponse>> act = async () => await TestUtility.MOCK_TEST_CLIENT.Recommendations.HideShowRecommendationAsync(showId);
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktShowNotFoundException>();

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
        public void TestTraktRecommendationsModuleHideUserShowRecommendationArgumentExceptions()
        {
            var showId = "1390";

            TestUtility.SetupMockResponseWithOAuth($"recommendations/shows/{showId}", HttpStatusCode.NoContent);

            Func<Task<TraktNoContentResponse>> act = async () => await TestUtility.MOCK_TEST_CLIENT.Recommendations.HideShowRecommendationAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Recommendations.HideShowRecommendationAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Recommendations.HideShowRecommendationAsync("show id");
            act.ShouldThrow<ArgumentException>();
        }

        #endregion
    }
}
