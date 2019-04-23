﻿namespace TraktNet.Objects.Tests.Basic.Json.Reader
{
    using FluentAssertions;
    using System.IO;
    using System.Threading.Tasks;
    using Trakt.NET.Tests.Utility;
    using Trakt.NET.Tests.Utility.Traits;
    using TraktNet.Objects.Basic.Json.Reader;
    using Xunit;

    [Category("Objects.Basic.JsonReader")]
    public partial class LanguageObjectJsonReader_Tests
    {
        [Fact]
        public async Task Test_LanguageObjectJsonReader_ReadObject_From_Stream_Complete()
        {
            var traktJsonReader = new LanguageObjectJsonReader();

            using (var stream = JSON_COMPLETE.ToStream())
            {
                var traktLanguage = await traktJsonReader.ReadObjectAsync(stream);

                traktLanguage.Should().NotBeNull();
                traktLanguage.Name.Should().Be("English");
                traktLanguage.Code.Should().Be("en");
            }
        }

        [Fact]
        public async Task Test_LanguageObjectJsonReader_ReadObject_From_Stream_Incomplete_1()
        {
            var traktJsonReader = new LanguageObjectJsonReader();

            using (var stream = JSON_INCOMPLETE_1.ToStream())
            {
                var traktLanguage = await traktJsonReader.ReadObjectAsync(stream);

                traktLanguage.Should().NotBeNull();
                traktLanguage.Name.Should().BeNull();
                traktLanguage.Code.Should().Be("en");
            }
        }

        [Fact]
        public async Task Test_LanguageObjectJsonReader_ReadObject_From_Stream_Incomplete_2()
        {
            var traktJsonReader = new LanguageObjectJsonReader();

            using (var stream = JSON_INCOMPLETE_2.ToStream())
            {
                var traktLanguage = await traktJsonReader.ReadObjectAsync(stream);

                traktLanguage.Should().NotBeNull();
                traktLanguage.Name.Should().Be("English");
                traktLanguage.Code.Should().BeNull();
            }
        }

        [Fact]
        public async Task Test_LanguageObjectJsonReader_ReadObject_From_Stream_Not_Valid_1()
        {
            var traktJsonReader = new LanguageObjectJsonReader();

            using (var stream = JSON_NOT_VALID_1.ToStream())
            {
                var traktLanguage = await traktJsonReader.ReadObjectAsync(stream);

                traktLanguage.Should().NotBeNull();
                traktLanguage.Name.Should().BeNull();
                traktLanguage.Code.Should().Be("en");
            }
        }

        [Fact]
        public async Task Test_LanguageObjectJsonReader_ReadObject_From_Stream_Not_Valid_2()
        {
            var traktJsonReader = new LanguageObjectJsonReader();

            using (var stream = JSON_NOT_VALID_2.ToStream())
            {
                var traktLanguage = await traktJsonReader.ReadObjectAsync(stream);

                traktLanguage.Should().NotBeNull();
                traktLanguage.Name.Should().Be("English");
                traktLanguage.Code.Should().BeNull();
            }
        }

        [Fact]
        public async Task Test_LanguageObjectJsonReader_ReadObject_From_Stream_Not_Valid_3()
        {
            var traktJsonReader = new LanguageObjectJsonReader();

            using (var stream = JSON_NOT_VALID_3.ToStream())
            {
                var traktLanguage = await traktJsonReader.ReadObjectAsync(stream);

                traktLanguage.Should().NotBeNull();
                traktLanguage.Name.Should().BeNull();
                traktLanguage.Code.Should().BeNull();
            }
        }

        [Fact]
        public async Task Test_LanguageObjectJsonReader_ReadObject_From_Stream_Null()
        {
            var traktJsonReader = new LanguageObjectJsonReader();

            var traktLanguage = await traktJsonReader.ReadObjectAsync(default(Stream));
            traktLanguage.Should().BeNull();
        }

        [Fact]
        public async Task Test_LanguageObjectJsonReader_ReadObject_From_Stream_Empty()
        {
            var traktJsonReader = new LanguageObjectJsonReader();

            using (var stream = string.Empty.ToStream())
            {
                var traktLanguage = await traktJsonReader.ReadObjectAsync(stream);
                traktLanguage.Should().BeNull();
            }
        }
    }
}
