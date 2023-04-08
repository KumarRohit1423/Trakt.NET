﻿namespace TraktNet.Modules.Tests.TraktRecommendationsModule
{
    using FluentAssertions;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Trakt.NET.Tests.Utility;
    using Trakt.NET.Tests.Utility.Traits;
    using TraktNet.Exceptions;
    using TraktNet.Objects.Get.Recommendations;
    using TraktNet.Responses;
    using Xunit;

    [TestCategory("Modules.Recommendations")]
    public partial class TraktRecommendationsModule_Tests
    {
        private const string GET_SHOW_RECOMMENDATIONS_URI = "recommendations/shows";

        [Fact]
        public async Task Test_TraktRecommendationsModule_GetShowRecommendations()
        {
            TraktClient client = TestUtility.GetOAuthMockClient(GET_SHOW_RECOMMENDATIONS_URI,
                                                                SHOW_RECOMMENDATIONS_JSON, 1, 10);

            TraktPagedResponse<ITraktRecommendedShow> response = await client.Recommendations.GetShowRecommendationsAsync();

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().Be(1u);
            response.Limit.Should().Be(10u);
        }

        [Fact]
        public async Task Test_TraktRecommendationsModule_GetShowRecommendations_With_Limit()
        {
            TraktClient client = TestUtility.GetOAuthMockClient($"{GET_SHOW_RECOMMENDATIONS_URI}?limit={LIMIT}",
                                                                SHOW_RECOMMENDATIONS_JSON, 1, LIMIT);

            TraktPagedResponse<ITraktRecommendedShow> response = await client.Recommendations.GetShowRecommendationsAsync(LIMIT);

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().Be(1u);
            response.Limit.Should().Be(LIMIT);
        }

        [Fact]
        public async Task Test_TraktRecommendationsModule_GetShowRecommendations_With_IgnoreCollected()
        {
            TraktClient client = TestUtility.GetOAuthMockClient($"{GET_SHOW_RECOMMENDATIONS_URI}?ignore_collected=true",
                                                                SHOW_RECOMMENDATIONS_JSON, 1, 10);

            TraktPagedResponse<ITraktRecommendedShow> response = await client.Recommendations.GetShowRecommendationsAsync(null, true);

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().Be(1u);
            response.Limit.Should().Be(10u);
        }

        [Fact]
        public async Task Test_TraktRecommendationsModule_GetShowRecommendations_With_IgnoreWatchlisted()
        {
            TraktClient client = TestUtility.GetOAuthMockClient($"{GET_SHOW_RECOMMENDATIONS_URI}?ignore_watchlisted=true",
                                                                SHOW_RECOMMENDATIONS_JSON, 1, 10);

            TraktPagedResponse<ITraktRecommendedShow> response = await client.Recommendations.GetShowRecommendationsAsync(null, null, true);

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().Be(1u);
            response.Limit.Should().Be(10u);
        }

        [Fact]
        public async Task Test_TraktRecommendationsModule_GetShowRecommendations_With_ExtendedInfo()
        {
            TraktClient client = TestUtility.GetOAuthMockClient($"{GET_SHOW_RECOMMENDATIONS_URI}?extended={EXTENDED_INFO}",
                                                                SHOW_RECOMMENDATIONS_JSON, 1, 10);

            TraktPagedResponse<ITraktRecommendedShow> response = await client.Recommendations.GetShowRecommendationsAsync(null, null, null, EXTENDED_INFO);

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().Be(1u);
            response.Limit.Should().Be(10u);
        }

        [Fact]
        public async Task Test_TraktRecommendationsModule_GetShowRecommendations_Complete()
        {
            TraktClient client = TestUtility.GetOAuthMockClient(
                $"{GET_SHOW_RECOMMENDATIONS_URI}?ignore_collected=true&ignore_watchlisted=true&extended={EXTENDED_INFO}&limit={LIMIT}",
                SHOW_RECOMMENDATIONS_JSON, 1, LIMIT);

            TraktPagedResponse<ITraktRecommendedShow> response = await client.Recommendations.GetShowRecommendationsAsync(LIMIT, true, true, EXTENDED_INFO);

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
            response.Page.Should().Be(1u);
            response.Limit.Should().Be(LIMIT);
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound, typeof(TraktNotFoundException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(TraktAuthorizationException))]
        [InlineData(HttpStatusCode.BadRequest, typeof(TraktBadRequestException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(TraktForbiddenException))]
        [InlineData(HttpStatusCode.MethodNotAllowed, typeof(TraktMethodNotFoundException))]
        [InlineData(HttpStatusCode.Conflict, typeof(TraktConflictException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(TraktServerException))]
        [InlineData(HttpStatusCode.BadGateway, typeof(TraktBadGatewayException))]
        [InlineData(HttpStatusCode.PreconditionFailed, typeof(TraktPreconditionFailedException))]
        [InlineData(HttpStatusCode.UnprocessableEntity, typeof(TraktValidationException))]
        [InlineData(HttpStatusCode.TooManyRequests, typeof(TraktRateLimitException))]
        [InlineData(HttpStatusCode.ServiceUnavailable, typeof(TraktServerUnavailableException))]
        [InlineData(HttpStatusCode.GatewayTimeout, typeof(TraktServerUnavailableException))]
        [InlineData((HttpStatusCode)520, typeof(TraktServerUnavailableException))]
        [InlineData((HttpStatusCode)521, typeof(TraktServerUnavailableException))]
        [InlineData((HttpStatusCode)522, typeof(TraktServerUnavailableException))]
        public async Task Test_TraktRecommendationsModule_GetShowRecommendations_Throws_API_Exception(HttpStatusCode statusCode, Type exceptionType)
        {
            TraktClient client = TestUtility.GetOAuthMockClient(GET_SHOW_RECOMMENDATIONS_URI, statusCode);

            try
            {
                await client.Recommendations.GetShowRecommendationsAsync();
                Assert.False(true);
            }
            catch (Exception exception)
            {
                (exception.GetType() == exceptionType).Should().BeTrue();
            }
        }
    }
}
