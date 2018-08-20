﻿namespace TraktNet.Objects.Post.Syncs.Watchlist.Json.Factories
{
    using Objects.Json;
    using Reader;
    using Writer;

    internal class SyncWatchlistPostShowSeasonJsonIOFactory : IJsonIOFactory<ITraktSyncWatchlistPostShowSeason>
    {
        public IObjectJsonReader<ITraktSyncWatchlistPostShowSeason> CreateObjectReader() => new SyncWatchlistPostShowSeasonObjectJsonReader();

        public IArrayJsonReader<ITraktSyncWatchlistPostShowSeason> CreateArrayReader() => new SyncWatchlistPostShowSeasonArrayJsonReader();

        public IObjectJsonWriter<ITraktSyncWatchlistPostShowSeason> CreateObjectWriter() => new SyncWatchlistPostShowSeasonObjectJsonWriter();
    }
}
