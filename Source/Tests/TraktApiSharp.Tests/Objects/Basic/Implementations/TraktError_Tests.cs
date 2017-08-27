﻿namespace TraktApiSharp.Tests.Objects.Basic.Implementations
{
    using FluentAssertions;
    using System.Threading.Tasks;
    using Traits;
    using TraktApiSharp.Objects.Basic;
    using TraktApiSharp.Objects.Basic.Implementations;
    using TraktApiSharp.Objects.Basic.JsonReader;
    using Xunit;

    [Category("Objects.Basic.Implementations")]
    public class TraktError_Tests
    {
        [Fact]
        public void Test_TraktError_Implements_ITraktError_Interface()
        {
            typeof(TraktError).GetInterfaces().Should().Contain(typeof(ITraktError));
        }

        [Fact]
        public void Test_TraktError_Default_Constructor()
        {
            var traktError = new TraktError();

            traktError.Error.Should().BeNull();
            traktError.Description.Should().BeNull();
        }

        [Fact]
        public async Task Test_TraktError_From_Json()
        {
            var jsonReader = new ErrorObjectJsonReader();
            var traktError = await jsonReader.ReadObjectAsync(JSON) as TraktError;

            traktError.Should().NotBeNull();
            traktError.Error.Should().Be("trakt error");
            traktError.Description.Should().Be("trakt error description");
        }

        private const string JSON =
            @"{
                ""error"": ""trakt error"",
                ""error_description"": ""trakt error description""
              }";
    }
}
