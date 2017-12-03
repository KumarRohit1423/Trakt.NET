﻿namespace TraktApiSharp.Objects.Get.Movies.Json.Reader
{
    using Implementations;
    using Newtonsoft.Json;
    using Objects.Json;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    internal class MovieTranslationObjectJsonReader : IObjectJsonReader<ITraktMovieTranslation>
    {
        public Task<ITraktMovieTranslation> ReadObjectAsync(string json, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(json))
                return Task.FromResult(default(ITraktMovieTranslation));

            using (var reader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return ReadObjectAsync(jsonReader, cancellationToken);
            }
        }

        public Task<ITraktMovieTranslation> ReadObjectAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (stream == null)
                return Task.FromResult(default(ITraktMovieTranslation));

            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return ReadObjectAsync(jsonReader, cancellationToken);
            }
        }

        public async Task<ITraktMovieTranslation> ReadObjectAsync(JsonTextReader jsonReader, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (jsonReader == null)
                return await Task.FromResult(default(ITraktMovieTranslation));

            if (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.StartObject)
            {
                ITraktMovieTranslation traktMovieTranslation = new TraktMovieTranslation();

                while (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = jsonReader.Value.ToString();

                    switch (propertyName)
                    {
                        case JsonProperties.MOVIE_TRANSLATION_PROPERTY_NAME_TITLE:
                            traktMovieTranslation.Title = await jsonReader.ReadAsStringAsync(cancellationToken);
                            break;
                        case JsonProperties.MOVIE_TRANSLATION_PROPERTY_NAME_OVERVIEW:
                            traktMovieTranslation.Overview = await jsonReader.ReadAsStringAsync(cancellationToken);
                            break;
                        case JsonProperties.MOVIE_TRANSLATION_PROPERTY_NAME_LANGUAGE_CODE:
                            traktMovieTranslation.LanguageCode = await jsonReader.ReadAsStringAsync(cancellationToken);
                            break;
                        case JsonProperties.MOVIE_TRANSLATION_PROPERTY_NAME_TAGLINE:
                            traktMovieTranslation.Tagline = await jsonReader.ReadAsStringAsync(cancellationToken);
                            break;
                        default:
                            await JsonReaderHelper.ReadAndIgnoreInvalidContentAsync(jsonReader, cancellationToken);
                            break;
                    }
                }

                return traktMovieTranslation;
            }

            return await Task.FromResult(default(ITraktMovieTranslation));
        }
    }
}
