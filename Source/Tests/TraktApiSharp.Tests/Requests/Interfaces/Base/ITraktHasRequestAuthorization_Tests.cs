﻿namespace TraktApiSharp.Tests.Requests.Interfaces.Base
{
    using FluentAssertions;
    using System.Linq;
    using TraktApiSharp.Experimental.Requests.Interfaces.Base;
    using TraktApiSharp.Requests;
    using TraktApiSharp.Tests.Traits;
    using Xunit;

    [Category("Requests.Interfaces.Base")]
    public class ITraktHasRequestAuthorization_Tests
    {
        [Fact]
        public void Test_ITraktHasRequestAuthorization_Is_Interface()
        {
            typeof(ITraktHasRequestAuthorization).IsInterface.Should().BeTrue();
        }

        [Fact]
        public void Test_ITraktHasRequestAuthorization_Has_AuthorizationRequirement_Property()
        {
            var propertyInfo = typeof(ITraktHasRequestAuthorization).GetProperties()
                                                                    .Where(p => p.Name == "AuthorizationRequirement")
                                                                    .FirstOrDefault();

            propertyInfo.CanRead.Should().BeTrue();
            propertyInfo.CanWrite.Should().BeFalse();
            propertyInfo.PropertyType.Should().Be(typeof(TraktAuthorizationRequirement));
        }
    }
}
