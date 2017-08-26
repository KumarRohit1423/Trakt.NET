﻿namespace TraktApiSharp.Tests.Requests.Users.OAuth
{
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Traits;
    using TraktApiSharp.Requests.Base;
    using TraktApiSharp.Requests.Users.OAuth;
    using Xunit;

    [Category("Requests.Users.OAuth")]
    public class TraktUserUnfollowUserRequest_Tests
    {
        [Fact]
        public void Test_TraktUserUnfollowUserRequest_Is_Not_Abstract()
        {
            typeof(TraktUserUnfollowUserRequest).IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void Test_TraktUserUnfollowUserRequest_Is_Sealed()
        {
            typeof(TraktUserUnfollowUserRequest).IsSealed.Should().BeTrue();
        }

        [Fact]
        public void Test_TraktUserUnfollowUserRequest_Inherits_ATraktDeleteRequest()
        {
            typeof(TraktUserUnfollowUserRequest).IsSubclassOf(typeof(ADeleteRequest)).Should().BeTrue();
        }

        [Fact]
        public void Test_TraktUserUnfollowUserRequest_Has_Username_Property()
        {
            var propertyInfo = typeof(TraktUserUnfollowUserRequest)
                    .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.Name == "Username")
                    .FirstOrDefault();

            propertyInfo.CanRead.Should().BeTrue();
            propertyInfo.CanWrite.Should().BeTrue();
            propertyInfo.PropertyType.Should().Be(typeof(string));
        }

        [Fact]
        public void Test_TraktUserUnfollowUserRequest_Has_AuthorizationRequirement_Required()
        {
            var request = new TraktUserUnfollowUserRequest();
            request.AuthorizationRequirement.Should().Be(TraktAuthorizationRequirement.Required);
        }

        [Fact]
        public void Test_TraktUserUnfollowUserRequest_Has_Valid_UriTemplate()
        {
            var request = new TraktUserUnfollowUserRequest();
            request.UriTemplate.Should().Be("users/{username}/follow");
        }

        [Fact]
        public void Test_TraktUserUnfollowUserRequest_Returns_Valid_UriPathParameters()
        {
            var request = new TraktUserUnfollowUserRequest { Username = "username" };

            request.GetUriPathParameters().Should().NotBeNull()
                                                   .And.HaveCount(1)
                                                   .And.Contain(new Dictionary<string, object>
                                                   {
                                                       ["username"] = "username"
                                                   });
        }

        [Fact]
        public void Test_TraktUserUnfollowUserRequest_Validate_Throws_Exceptions()
        {
            // username is null
            var request = new TraktUserUnfollowUserRequest();

            Action act = () => request.Validate();
            act.ShouldThrow<ArgumentNullException>();

            // empty username
            request = new TraktUserUnfollowUserRequest { Username = string.Empty };

            act = () => request.Validate();
            act.ShouldThrow<ArgumentException>();

            // username with spaces
            request = new TraktUserUnfollowUserRequest { Username = "invalid username" };

            act = () => request.Validate();
            act.ShouldThrow<ArgumentException>();
        }
    }
}
