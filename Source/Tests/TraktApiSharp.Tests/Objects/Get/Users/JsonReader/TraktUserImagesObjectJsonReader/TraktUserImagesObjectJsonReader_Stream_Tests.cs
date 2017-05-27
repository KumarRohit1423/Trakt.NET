﻿namespace TraktApiSharp.Tests.Objects.Get.Users.JsonReader
{
    using FluentAssertions;
    using System.IO;
    using System.Threading.Tasks;
    using TestUtils;
    using Traits;
    using TraktApiSharp.Objects.Get.Users.JsonReader;
    using Xunit;

    [Category("Objects.Get.Users.JsonReader")]
    public partial class TraktUserImagesObjectJsonReader_Tests
    {
        [Fact]
        public async Task Test_TraktUserImagesObjectJsonReader_ReadObject_From_Stream_Complete()
        {
            var jsonReader = new TraktUserImagesObjectJsonReader();

            using (var stream =  JSON_COMPLETE.ToStream())
            {
                var userImages = await jsonReader.ReadObjectAsync(stream);

                userImages.Should().NotBeNull();
                userImages.Avatar.Should().NotBeNull();
                userImages.Avatar.Full.Should().Be("https://walter.trakt.us/images/users/000/359/474/avatars/large/58cc281a20.gif");
            }
        }

        [Fact]
        public async Task Test_TraktUserImagesObjectJsonReader_ReadObject_From_Stream_Not_Valid()
        {
            var jsonReader = new TraktUserImagesObjectJsonReader();

            using (var stream =  JSON_NOT_VALID.ToStream())
            {
                var userImages = await jsonReader.ReadObjectAsync(stream);

                userImages.Should().NotBeNull();
                userImages.Avatar.Should().BeNull();
            }
        }

        [Fact]
        public async Task Test_TraktUserImagesObjectJsonReader_ReadObject_From_Stream_Null()
        {
            var jsonReader = new TraktUserImagesObjectJsonReader();

            var userImages = await jsonReader.ReadObjectAsync(default(Stream));
            userImages.Should().BeNull();
        }

        [Fact]
        public async Task Test_TraktUserImagesObjectJsonReader_ReadObject_From_Stream_Empty()
        {
            var jsonReader = new TraktUserImagesObjectJsonReader();

            using (var stream = string.Empty.ToStream())
            {
                var userImages = await jsonReader.ReadObjectAsync(stream);
                userImages.Should().BeNull();
            }
        }
    }
}
