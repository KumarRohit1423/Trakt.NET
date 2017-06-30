﻿namespace TraktApiSharp.Tests.Objects.Post.Responses.JsonReader
{
    using FluentAssertions;
    using Newtonsoft.Json;
    using System.IO;
    using System.Threading.Tasks;
    using Traits;
    using TraktApiSharp.Objects.Post.Responses.JsonReader;
    using Xunit;

    [Category("Objects.Post.Responses.JsonReader")]
    public partial class TraktPostResponseNotFoundEpisodeObjectJsonReader_Tests
    {
        [Fact]
        public async Task Test_TraktPostResponseNotFoundEpisodeObjectJsonReader_ReadObject_From_JsonReader_Complete()
        {
            var traktJsonReader = new TraktPostResponseNotFoundEpisodeObjectJsonReader();

            using (var reader = new StringReader(JSON_COMPLETE))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var postResponseNotFoundEpisode = await traktJsonReader.ReadObjectAsync(jsonReader);

                postResponseNotFoundEpisode.Should().NotBeNull();
                postResponseNotFoundEpisode.Ids.Should().NotBeNull();
                postResponseNotFoundEpisode.Ids.Trakt.Should().Be(73640U);
                postResponseNotFoundEpisode.Ids.Tvdb.Should().Be(3254641U);
                postResponseNotFoundEpisode.Ids.Imdb.Should().Be("tt1480055");
                postResponseNotFoundEpisode.Ids.Tmdb.Should().Be(63056U);
                postResponseNotFoundEpisode.Ids.TvRage.Should().Be(1065008299U);
            }
        }

        [Fact]
        public async Task Test_TraktPostResponseNotFoundEpisodeObjectJsonReader_ReadObject_From_JsonReader_Not_Valid()
        {
            var traktJsonReader = new TraktPostResponseNotFoundEpisodeObjectJsonReader();

            using (var reader = new StringReader(JSON_NOT_VALID))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var postResponseNotFoundEpisode = await traktJsonReader.ReadObjectAsync(jsonReader);

                postResponseNotFoundEpisode.Should().NotBeNull();
                postResponseNotFoundEpisode.Ids.Should().BeNull();
            }
        }

        [Fact]
        public async Task Test_TraktPostResponseNotFoundEpisodeObjectJsonReader_ReadObject_From_JsonReader_Null()
        {
            var traktJsonReader = new TraktPostResponseNotFoundEpisodeObjectJsonReader();

            var postResponseNotFoundEpisode = await traktJsonReader.ReadObjectAsync(default(JsonTextReader));
            postResponseNotFoundEpisode.Should().BeNull();
        }

        [Fact]
        public async Task Test_TraktPostResponseNotFoundEpisodeObjectJsonReader_ReadObject_From_JsonReader_Empty()
        {
            var traktJsonReader = new TraktPostResponseNotFoundEpisodeObjectJsonReader();

            using (var reader = new StringReader(string.Empty))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var postResponseNotFoundEpisode = await traktJsonReader.ReadObjectAsync(jsonReader);
                postResponseNotFoundEpisode.Should().BeNull();
            }
        }
    }
}
