﻿namespace TraktApiSharp.Objects.Get.Episodes.JsonReader
{
    using Newtonsoft.Json;
    using Objects.JsonReader;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    internal class ITraktEpisodeCollectionProgressArrayJsonReader : ITraktArrayJsonReader<ITraktEpisodeCollectionProgress>
    {
        public Task<IEnumerable<ITraktEpisodeCollectionProgress>> ReadArrayAsync(string json, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(json))
                return Task.FromResult(default(IEnumerable<ITraktEpisodeCollectionProgress>));

            using (var reader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return ReadArrayAsync(jsonReader, cancellationToken);
            }
        }

        public Task<IEnumerable<ITraktEpisodeCollectionProgress>> ReadArrayAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ITraktEpisodeCollectionProgress>> ReadArrayAsync(JsonTextReader jsonReader, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (jsonReader == null)
                return await Task.FromResult(default(IEnumerable<ITraktEpisodeCollectionProgress>));

            if (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.StartArray)
            {
                var episodeCollectionProgressReader = new ITraktEpisodeCollectionProgressObjectJsonReader();
                //var traktEpisodeCollectionProgressReadingTasks = new List<Task<ITraktEpisodeCollectionProgress>>();
                var traktEpisodeCollectionProgresses = new List<ITraktEpisodeCollectionProgress>();

                //traktEpisodeCollectionProgressReadingTasks.Add(episodeCollectionProgressReader.ReadObjectAsync(jsonReader, cancellationToken));
                ITraktEpisodeCollectionProgress traktEpisodeCollectionProgress = await episodeCollectionProgressReader.ReadObjectAsync(jsonReader, cancellationToken);

                while (traktEpisodeCollectionProgress != null)
                {
                    traktEpisodeCollectionProgresses.Add(traktEpisodeCollectionProgress);
                    //traktEpisodeCollectionProgressReadingTasks.Add(episodeCollectionProgressReader.ReadObjectAsync(jsonReader, cancellationToken));
                    traktEpisodeCollectionProgress = await episodeCollectionProgressReader.ReadObjectAsync(jsonReader, cancellationToken);
                }

                //var readEpisodeCollectionProgresses = await Task.WhenAll(traktEpisodeCollectionProgressReadingTasks);
                //return (IEnumerable<ITraktEpisodeCollectionProgress>)readEpisodeCollectionProgresses.GetEnumerator();
                return traktEpisodeCollectionProgresses;
            }

            return await Task.FromResult(default(IEnumerable<ITraktEpisodeCollectionProgress>));
        }
    }
}
