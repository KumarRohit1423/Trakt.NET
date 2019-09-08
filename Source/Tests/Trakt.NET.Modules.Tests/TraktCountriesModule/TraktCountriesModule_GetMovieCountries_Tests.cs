﻿namespace TraktNet.Modules.Tests.TraktCountriesModule
{
    using FluentAssertions;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Trakt.NET.Tests.Utility;
    using Trakt.NET.Tests.Utility.Traits;
    using TraktNet.Exceptions;
    using TraktNet.Objects.Basic;
    using TraktNet.Responses;
    using Xunit;

    [Category("Modules.Countries")]
    public partial class TraktCountriesModule_Tests
    {
        private const string COUNTRIES_MOVIES_URI = "countries/movies";

        [Fact]
        public async Task Test_TraktCountriesModule_GetMovieCountries()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, COUNTRIES_JSON);
            TraktListResponse<ITraktCountry> response = await client.Countries.GetMovieCountriesAsync();

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull().And.HaveCount(3);
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_NotFoundException()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, HttpStatusCode.NotFound);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktNotFoundException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_AuthorizationException()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, HttpStatusCode.Unauthorized);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktAuthorizationException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_BadRequestException()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, HttpStatusCode.BadRequest);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktBadRequestException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_ForbiddenException()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, HttpStatusCode.Forbidden);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktForbiddenException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_MethodNotFoundException()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, HttpStatusCode.MethodNotAllowed);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktMethodNotFoundException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_ConflictException()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, HttpStatusCode.Conflict);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktConflictException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_ServerErrorException()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, HttpStatusCode.InternalServerError);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktServerException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_BadGatewayException()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, HttpStatusCode.BadGateway);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktBadGatewayException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_PreconditionFailedException()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, (HttpStatusCode)412);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktPreconditionFailedException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_ValidationException()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, (HttpStatusCode)422);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktValidationException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_RateLimitException()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, (HttpStatusCode)429);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktRateLimitException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_ServerUnavailableException_503()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, (HttpStatusCode)503);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_ServerUnavailableException_504()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, (HttpStatusCode)504);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_ServerUnavailableException_520()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, (HttpStatusCode)520);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_ServerUnavailableException_521()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, (HttpStatusCode)521);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [Fact]
        public void Test_TraktCountriesModule_GetMovieCountries_Throws_ServerUnavailableException_522()
        {
            TraktClient client = TestUtility.GetMockClient(COUNTRIES_MOVIES_URI, (HttpStatusCode)522);
            Func<Task<TraktListResponse<ITraktCountry>>> act = () => client.Countries.GetMovieCountriesAsync();
            act.Should().Throw<TraktServerUnavailableException>();
        }
    }
}