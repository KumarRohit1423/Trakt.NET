﻿namespace TraktApiSharp.Tests.Requests.Comments.OAuth
{
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using Traits;
    using TraktApiSharp.Objects.Post.Comments.Implementations;
    using TraktApiSharp.Requests.Base;
    using TraktApiSharp.Requests.Comments.OAuth;
    using Xunit;

    [Category("Requests.Comments.OAuth")]
    public class CommentUpdateRequest_Tests
    {
        [Fact]
        public void Test_CommentUpdateRequest_Has_Valid_UriTemplate()
        {
            var request = new CommentUpdateRequest();
            request.UriTemplate.Should().Be("comments/{id}");
        }

        [Fact]
        public void Test_CommentUpdateRequest_Has_AuthorizationRequirement_Required()
        {
            var requestMock = new CommentUpdateRequest();
            requestMock.AuthorizationRequirement.Should().Be(AuthorizationRequirement.Required);
        }

        [Fact]
        public void Test_CommentUpdateRequest_Returns_Valid_RequestObjectType()
        {
            var request = new CommentUpdateRequest();
            request.RequestObjectType.Should().Be(RequestObjectType.Comments);
        }

        [Fact]
        public void Test_CommentUpdateRequest_Returns_Valid_UriPathParameters()
        {
            // only id
            var request = new CommentUpdateRequest { Id = "123", RequestBody = new TraktCommentUpdatePost() };

            request.GetUriPathParameters().Should().NotBeNull()
                                                   .And.HaveCount(1)
                                                   .And.Contain(new Dictionary<string, object>
                                                   {
                                                       ["id"] = "123"
                                                   });
        }

        [Fact]
        public void Test_CommentUpdateRequest_Validate_Throws_Exceptions()
        {
            // request body is null
            var request = new CommentUpdateRequest { Id = "123" };

            Action act = () => request.Validate();
            act.ShouldThrow<ArgumentNullException>();

            // id is null
            request = new CommentUpdateRequest { RequestBody = null };

            act = () => request.Validate();
            act.ShouldThrow<ArgumentNullException>();

            // empty id
            request = new CommentUpdateRequest { Id = string.Empty, RequestBody = new TraktCommentUpdatePost() };

            act = () => request.Validate();
            act.ShouldThrow<ArgumentException>();

            // id with spaces
            request = new CommentUpdateRequest { Id = "invalid id", RequestBody = new TraktCommentUpdatePost() };

            act = () => request.Validate();
            act.ShouldThrow<ArgumentException>();
        }
    }
}
