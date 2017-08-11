﻿namespace TraktApiSharp.Objects.Get.History.JsonReader.Factories
{
    using Objects.JsonReader;

    internal class TraktHistoryItemJsonReaderFactory : ITraktJsonReaderFactory<ITraktHistoryItem>
    {
        public ITraktObjectJsonReader<ITraktHistoryItem> CreateObjectReader() => new TraktHistoryItemObjectJsonReader();

        public ITraktArrayJsonReader<ITraktHistoryItem> CreateArrayReader() => new TraktHistoryItemArrayJsonReader();
    }
}