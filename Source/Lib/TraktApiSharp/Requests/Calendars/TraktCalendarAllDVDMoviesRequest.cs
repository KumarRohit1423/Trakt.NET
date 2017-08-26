﻿namespace TraktApiSharp.Requests.Calendars
{
    using Objects.Get.Calendars;

    internal sealed class TraktCalendarAllDVDMoviesRequest : ACalendarRequest<ITraktCalendarMovie>
    {
        public override string UriTemplate => "calendars/all/dvd{/start_date}{/days}{?extended,query,years,genres,languages,countries,runtimes,ratings}";
    }
}
