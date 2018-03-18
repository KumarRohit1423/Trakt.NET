﻿namespace TraktApiSharp.Tests.Authentication
{
    using FluentAssertions;
    using Newtonsoft.Json;
    using System;
    using Traits;
    using TraktApiSharp.Authentication;
    using TraktApiSharp.Enums;
    using Xunit;

    [Category("Authentication")]
    public class TraktAuthorization_Tests
    {
        [Fact]
        public void Test_TraktAuthorization_DefaultConstructor()
        {
            var dtNowUtc = DateTime.UtcNow;

            var token = new TraktAuthorization();

            token.AccessToken.Should().BeNullOrEmpty();
            token.AccessScope.Should().BeNull();
            token.TokenType.Should().BeNull();
            token.ExpiresInSeconds.Should().Be(0);
            token.RefreshToken.Should().BeNullOrEmpty();
            token.IsValid.Should().BeFalse();
            token.IsRefreshPossible.Should().BeFalse();
            token.IsExpired.Should().BeTrue();
            token.Created.Should().BeCloseTo(dtNowUtc);
            token.IgnoreExpiration.Should().BeFalse();
        }

        [Fact]
        public void Test_TraktAuthorization_ReadFromJson()
        {
            var token = JsonConvert.DeserializeObject<TraktAuthorization>(JSON);

            token.Should().NotBeNull();
            token.AccessToken.Should().Be("dbaf9757982a9e738f05d249b7b5b4a266b3a139049317c4909f2f263572c781");
            token.TokenType.Should().Be(TraktAccessTokenType.Bearer);
            token.ExpiresInSeconds.Should().Be(7200);
            token.RefreshToken.Should().Be("76ba4c5c75c96f6087f58a4de10be6c00b29ea1ddc3b2022ee2016d1363e3a7c");
            token.AccessScope.Should().Be(TraktAccessScope.Public);
            token.IsExpired.Should().BeFalse();
            token.IsValid.Should().BeTrue();
            token.IsRefreshPossible.Should().BeTrue();
            token.IgnoreExpiration.Should().BeFalse();
        }

        [Fact]
        public void Test_TraktAuthorization_IsValid()
        {
            var token = new TraktAuthorization();
            token.IsValid.Should().BeFalse();

            token.AccessToken = string.Empty;
            token.IsValid.Should().BeFalse();

            token.AccessToken = "access token";
            token.IsValid.Should().BeFalse();

            token.AccessToken = "accessToken";
            token.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Test_TraktAuthorization_IsRefreshPossible()
        {
            var token = new TraktAuthorization();
            token.IsRefreshPossible.Should().BeFalse();

            token.RefreshToken = string.Empty;
            token.IsRefreshPossible.Should().BeFalse();

            token.RefreshToken = "refresh token";
            token.IsRefreshPossible.Should().BeFalse();

            token.RefreshToken = "refreshToken";
            token.IsRefreshPossible.Should().BeTrue();
        }

        [Fact]
        public void Test_TraktAuthorization_IsExpired()
        {
            var token = new TraktAuthorization();
            token.IsExpired.Should().BeTrue();

            token.AccessToken = string.Empty;
            token.IsExpired.Should().BeTrue();

            token.AccessToken = "access token";
            token.IsExpired.Should().BeTrue();

            token.AccessToken = "accessToken";
            token.IsExpired.Should().BeTrue();

            token.ExpiresInSeconds = 1;
            token.IsExpired.Should().BeFalse();
        }

        [Fact]
        public void Test_TraktAuthorization_IsExpiredWithIgnoreExpiration()
        {
            var token = new TraktAuthorization
            {
                IgnoreExpiration = true,
                ExpiresInSeconds = 0
            };

            token.IsExpired.Should().BeTrue();

            token.AccessToken = string.Empty;
            token.IsExpired.Should().BeTrue();

            token.AccessToken = "access token";
            token.IsExpired.Should().BeTrue();

            token.AccessToken = "accessToken";
            token.IsExpired.Should().BeFalse();
        }

        [Fact]
        public void Test_TraktAuthorization_CreateWithAccessToken()
        {
            const string accessToken = "accessToken";

            var authorization = TraktAuthorization.CreateWith(accessToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().Be(accessToken);
            authorization.RefreshToken.Should().NotBeNull().And.BeEmpty();
            authorization.AccessScope.Should().Be(TraktAccessScope.Public);
            authorization.TokenType.Should().Be(TraktAccessTokenType.Bearer);
            authorization.IgnoreExpiration.Should().BeTrue();
            authorization.ExpiresInSeconds.Should().Be(0);
        }

        [Fact]
        public void Test_TraktAuthorization_CreateWithAccessTokenAndRefreshToken()
        {
            const string accessToken = "accessToken";
            const string refreshToken = "refreshToken";

            var authorization = TraktAuthorization.CreateWith(accessToken, refreshToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().Be(accessToken);
            authorization.RefreshToken.Should().Be(refreshToken);
            authorization.AccessScope.Should().Be(TraktAccessScope.Public);
            authorization.TokenType.Should().Be(TraktAccessTokenType.Bearer);
            authorization.IgnoreExpiration.Should().BeTrue();
            authorization.ExpiresInSeconds.Should().Be(0);
        }

        [Fact]
        public void Test_TraktAuthorization_CreateWithExpiresInAndAccessToken()
        {
            const int expiresIn = 3600 * 24 * 90;
            const string accessToken = "accessToken";

            var authorization = TraktAuthorization.CreateWith(expiresIn, accessToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().Be(accessToken);
            authorization.RefreshToken.Should().NotBeNull().And.BeEmpty();
            authorization.AccessScope.Should().Be(TraktAccessScope.Public);
            authorization.TokenType.Should().Be(TraktAccessTokenType.Bearer);
            authorization.IgnoreExpiration.Should().BeFalse();
            authorization.Created.Should().BeCloseTo(DateTime.UtcNow, 1800 * 1000);
            authorization.ExpiresInSeconds.Should().Be(expiresIn);
        }

        [Fact]
        public void Test_TraktAuthorization_CreateWithExpiresInAndAccessTokenAndRefreshToken()
        {
            const int expiresIn = 3600 * 24 * 90;
            const string accessToken = "accessToken";
            const string refreshToken = "refreshToken";

            var authorization = TraktAuthorization.CreateWith(expiresIn, accessToken, refreshToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().Be(accessToken);
            authorization.RefreshToken.Should().Be(refreshToken);
            authorization.AccessScope.Should().Be(TraktAccessScope.Public);
            authorization.TokenType.Should().Be(TraktAccessTokenType.Bearer);
            authorization.IgnoreExpiration.Should().BeFalse();
            authorization.Created.Should().BeCloseTo(DateTime.UtcNow, 1800 * 1000);
            authorization.ExpiresInSeconds.Should().Be(expiresIn);
        }

        [Fact]
        public void Test_TraktAuthorization_CreateWithCreatedAtAndAccessToken()
        {
            var createdAt = DateTime.Now;
            const int expiresIn = 3600 * 24 * 90;
            const string accessToken = "accessToken";

            var authorization = TraktAuthorization.CreateWith(createdAt, accessToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().Be(accessToken);
            authorization.RefreshToken.Should().NotBeNull().And.BeEmpty();
            authorization.AccessScope.Should().Be(TraktAccessScope.Public);
            authorization.TokenType.Should().Be(TraktAccessTokenType.Bearer);
            authorization.IgnoreExpiration.Should().BeFalse();
            authorization.Created.Should().Be(createdAt.ToUniversalTime());
            authorization.ExpiresInSeconds.Should().Be(expiresIn);
        }

        [Fact]
        public void Test_TraktAuthorization_CreateWithCreatedAtAndAccessTokenAndRefreshToken()
        {
            var createdAt = DateTime.Now;
            const int expiresIn = 3600 * 24 * 90;
            const string accessToken = "accessToken";
            const string refreshToken = "refreshToken";

            var authorization = TraktAuthorization.CreateWith(createdAt, accessToken, refreshToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().Be(accessToken);
            authorization.RefreshToken.Should().Be(refreshToken);
            authorization.AccessScope.Should().Be(TraktAccessScope.Public);
            authorization.TokenType.Should().Be(TraktAccessTokenType.Bearer);
            authorization.IgnoreExpiration.Should().BeFalse();
            authorization.Created.Should().Be(createdAt.ToUniversalTime());
            authorization.ExpiresInSeconds.Should().Be(expiresIn);
        }

        [Fact]
        public void Test_TraktAuthorization_CreateWithCreatedAtAndExpiresInAndAccessToken()
        {
            var createdAt = DateTime.Now;
            const int expiresIn = 3600 * 24 * 90;
            const string accessToken = "accessToken";

            var authorization = TraktAuthorization.CreateWith(createdAt, expiresIn, accessToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().Be(accessToken);
            authorization.RefreshToken.Should().NotBeNull().And.BeEmpty();
            authorization.AccessScope.Should().Be(TraktAccessScope.Public);
            authorization.TokenType.Should().Be(TraktAccessTokenType.Bearer);
            authorization.IgnoreExpiration.Should().BeFalse();
            authorization.Created.Should().Be(createdAt.ToUniversalTime());
            authorization.ExpiresInSeconds.Should().Be(expiresIn);
        }

        [Fact]
        public void Test_TraktAuthorization_CreateWithCreatedAtAndExpiresInAndAccessTokenAndRefreshToken()
        {
            var createdAt = DateTime.Now;
            const int expiresIn = 3600 * 24 * 90;
            const string accessToken = "accessToken";
            const string refreshToken = "refreshToken";

            var authorization = TraktAuthorization.CreateWith(createdAt, expiresIn, accessToken, refreshToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().Be(accessToken);
            authorization.RefreshToken.Should().Be(refreshToken);
            authorization.AccessScope.Should().Be(TraktAccessScope.Public);
            authorization.TokenType.Should().Be(TraktAccessTokenType.Bearer);
            authorization.IgnoreExpiration.Should().BeFalse();
            authorization.Created.Should().Be(createdAt.ToUniversalTime());
            authorization.ExpiresInSeconds.Should().Be(expiresIn);
        }

        [Fact]
        public void Test_TraktAuthorization_CreateWithNullValues()
        {
            var createdAt = DateTime.Now;
            const int expiresIn = 3600 * 24 * 90;
            const string accessToken = "accessToken";
            const string refreshToken = "refreshToken";

            var authorization = TraktAuthorization.CreateWith(null, refreshToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().NotBeNull().And.BeEmpty();
            authorization.RefreshToken.Should().Be(refreshToken);

            authorization = TraktAuthorization.CreateWith(accessToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().Be(accessToken);
            authorization.RefreshToken.Should().NotBeNull().And.BeEmpty();

            // ------------------------------------------------------

            authorization = TraktAuthorization.CreateWith(expiresIn, null, refreshToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().NotBeNull().And.BeEmpty();
            authorization.RefreshToken.Should().Be(refreshToken);

            authorization = TraktAuthorization.CreateWith(expiresIn, accessToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().Be(accessToken);
            authorization.RefreshToken.Should().NotBeNull().And.BeEmpty();

            // ------------------------------------------------------

            authorization = TraktAuthorization.CreateWith(createdAt, null, refreshToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().NotBeNull().And.BeEmpty();
            authorization.RefreshToken.Should().Be(refreshToken);

            authorization = TraktAuthorization.CreateWith(createdAt, accessToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().Be(accessToken);
            authorization.RefreshToken.Should().NotBeNull().And.BeEmpty();

            // ------------------------------------------------------

            authorization = TraktAuthorization.CreateWith(createdAt, expiresIn, null, refreshToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().NotBeNull().And.BeEmpty();
            authorization.RefreshToken.Should().Be(refreshToken);

            authorization = TraktAuthorization.CreateWith(createdAt, expiresIn, accessToken);

            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().Be(accessToken);
            authorization.RefreshToken.Should().NotBeNull().And.BeEmpty();
        }

        private const string JSON =
            @"{
                ""access_token"": ""dbaf9757982a9e738f05d249b7b5b4a266b3a139049317c4909f2f263572c781"",
                ""token_type"": ""bearer"",
                ""expires_in"": 7200,
                ""refresh_token"": ""76ba4c5c75c96f6087f58a4de10be6c00b29ea1ddc3b2022ee2016d1363e3a7c"",
                ""scope"": ""public""
              }";
    }
}
