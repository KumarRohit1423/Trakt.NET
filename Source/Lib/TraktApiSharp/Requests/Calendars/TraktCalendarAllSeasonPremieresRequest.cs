﻿namespace TraktApiSharp.Requests.Calendars
{
    using Objects.Get.Calendars.Implementations;

    internal sealed class TraktCalendarAllSeasonPremieresRequest : ATraktCalendarRequest<TraktCalendarShow>
    {
        public override string UriTemplate => "calendars/all/shows/premieres{/start_date}{/days}{?extended,query,years,genres,languages,countries,runtimes,ratings}";
    }
}
