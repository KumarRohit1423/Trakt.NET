﻿namespace TraktApiSharp.Requests.Shows
{
    using Objects.Basic.Implementations;

    internal sealed class TraktShowStatisticsRequest : ATraktShowRequest<TraktStatistics>
    {
        public override string UriTemplate => "shows/{id}/stats";
    }
}
