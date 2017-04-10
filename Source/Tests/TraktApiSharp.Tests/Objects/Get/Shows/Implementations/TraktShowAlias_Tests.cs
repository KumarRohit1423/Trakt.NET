﻿namespace TraktApiSharp.Tests.Objects.Get.Shows.Implementations
{
    using FluentAssertions;
    using System.Threading.Tasks;
    using Traits;
    using TraktApiSharp.Objects.Get.Shows;
    using TraktApiSharp.Objects.Get.Shows.Implementations;
    using TraktApiSharp.Objects.Get.Shows.JsonReader;
    using Xunit;

    [Category("Objects.Get.Shows.Implementations")]
    public class TraktShowAlias_Tests
    {
        [Fact]
        public void Test_TraktShowAlias_Implements_ITraktShowAlias_Interface()
        {
            typeof(TraktShowAlias).GetInterfaces().Should().Contain(typeof(ITraktShowAlias));
        }

        [Fact]
        public void Test_TraktShowAlias_Default_Constructor()
        {
            var showAlias = new TraktShowAlias();

            showAlias.Title.Should().BeNullOrEmpty();
            showAlias.CountryCode.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Test_TraktShowAlias_From_Json()
        {
            var jsonReader = new ITraktShowAliasObjectJsonReader();
            var showAlias = await jsonReader.ReadObjectAsync(JSON);

            showAlias.Should().NotBeNull();
            showAlias.Title.Should().Be("Game of Thrones- Das Lied von Eis und Feuer");
            showAlias.CountryCode.Should().Be("de");
        }

        private const string JSON =
            @"{
                ""title"": ""Game of Thrones- Das Lied von Eis und Feuer"",
                ""country"": ""de""
              }";
    }
}
