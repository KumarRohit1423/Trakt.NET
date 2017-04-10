﻿namespace TraktApiSharp.Modules
{
    using Exceptions;
    using Objects.Get.Calendars.Implementations;
    using Requests.Calendars;
    using Requests.Calendars.OAuth;
    using Requests.Handler;
    using Requests.Parameters;
    using Responses;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides access to data retrieving methods specific to calendars.
    /// <para>
    /// This module contains all methods of the <a href ="http://docs.trakt.apiary.io/#reference/calendars">"Trakt API Doc - Calendars"</a> section.
    /// </para>
    /// </summary>
    public class TraktCalendarModule : TraktBaseModule
    {
        internal TraktCalendarModule(TraktClient client) : base(client) { }

        /// <summary>
        /// Gets all users <see cref="TraktCalendarShow" />s airing during the given time period.
        /// <para>OAuth authorization required.</para>
        /// <para>
        /// See <a href="http://docs.trakt.apiary.io/#reference/calendars/my-shows/get-shows">"Trakt API Doc - Calendars: My Shows"</a> for more information.
        /// </para>
        /// </summary>
        /// <param name="startDate">The date, on which the time period should start. Defaults to today. Will be converted to the Trakt date-format.</param>
        /// <param name="days">1 - 31 days, specifying the length of the time period. Defaults to 7 days.</param>
        /// <param name="extendedInfo">
        /// The extended info, which determines how much data about the shows should be queried.
        /// See also <seealso cref="TraktExtendedInfo" />.
        /// </param>
        /// <param name="filter">Optional filters for genres, languages, year, runtimes, ratings, etc. See also <seealso cref="TraktCalendarFilter" />.</param>
        /// <returns>A list of <see cref="TraktCalendarShow" /> instances.</returns>
        /// <exception cref="TraktException">Thrown, if the request fails.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if the given days value is not between 1 and 31.</exception>
        public async Task<TraktListResponse<TraktCalendarShow>> GetUserShowsAsync(DateTime? startDate = null, int? days = null,
                                                                                  TraktExtendedInfo extendedInfo = null,
                                                                                  TraktCalendarFilter filter = null)
        {
            var requestHandler = new TraktRequestHandler(Client);

            return await requestHandler.ExecuteListRequestAsync(new TraktCalendarUserShowsRequest
            {
                StartDate = startDate,
                Days = days,
                ExtendedInfo = extendedInfo,
                Filter = filter
            });
        }

        /// <summary>
        /// Gets all new users <see cref="TraktCalendarShow" />s airing during the given time period.
        /// <para>OAuth authorization required.</para>
        /// <para>
        /// See <a href="http://docs.trakt.apiary.io/#reference/calendars/my-new-shows/get-new-shows">"Trakt API Doc - Calendars: My New Shows"</a> for more information.
        /// </para>
        /// </summary>
        /// <param name="startDate">The date, on which the time period should start. Defaults to today. Will be converted to the Trakt date-format.</param>
        /// <param name="days">1 - 31 days, specifying the length of the time period. Defaults to 7 days.</param>
        /// <param name="extendedInfo">
        /// The extended info, which determines how much data about the shows should be queried.
        /// See also <seealso cref="TraktExtendedInfo" />.
        /// </param>
        /// <param name="filter">Optional filters for genres, languages, year, runtimes, ratings, etc. See also <seealso cref="TraktCalendarFilter" />.</param>
        /// <returns>A list of <see cref="TraktCalendarShow" /> instances.</returns>
        /// <exception cref="TraktException">Thrown, if the request fails.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if the given days value is not between 1 and 31.</exception>
        public async Task<TraktListResponse<TraktCalendarShow>> GetUserNewShowsAsync(DateTime? startDate = null, int? days = null,
                                                                                     TraktExtendedInfo extendedInfo = null,
                                                                                     TraktCalendarFilter filter = null)
        {
            var requestHandler = new TraktRequestHandler(Client);

            return await requestHandler.ExecuteListRequestAsync(new TraktCalendarUserNewShowsRequest
            {
                StartDate = startDate,
                Days = days,
                ExtendedInfo = extendedInfo,
                Filter = filter
            });
        }

        /// <summary>
        /// Gets all users season premieres airing during the given time period.
        /// <para>OAuth authorization required.</para>
        /// <para>
        /// See <a href="http://docs.trakt.apiary.io/#reference/calendars/my-season-premieres/get-season-premieres">"Trakt API Doc - Calendars: My Season Premieres"</a> for more information.
        /// </para>
        /// </summary>
        /// <param name="startDate">The date, on which the time period should start. Defaults to today. Will be converted to the Trakt date-format.</param>
        /// <param name="days">1 - 31 days, specifying the length of the time period. Defaults to 7 days.</param>
        /// <param name="extendedInfo">
        /// The extended info, which determines how much data about the shows should be queried.
        /// See also <seealso cref="TraktExtendedInfo" />.
        /// </param>
        /// <param name="filter">Optional filters for genres, languages, year, runtimes, ratings, etc. See also <seealso cref="TraktCalendarFilter" />.</param>
        /// <returns>A list of <see cref="TraktCalendarShow" /> instances.</returns>
        /// <exception cref="TraktException">Thrown, if the request fails.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if the given days value is not between 1 and 31.</exception>
        public async Task<TraktListResponse<TraktCalendarShow>> GetUserSeasonPremieresAsync(DateTime? startDate = null, int? days = null,
                                                                                            TraktExtendedInfo extendedInfo = null,
                                                                                            TraktCalendarFilter filter = null)
        {
            var requestHandler = new TraktRequestHandler(Client);

            return await requestHandler.ExecuteListRequestAsync(new TraktCalendarUserSeasonPremieresRequest
            {
                StartDate = startDate,
                Days = days,
                ExtendedInfo = extendedInfo,
                Filter = filter
            });
        }

        /// <summary>
        /// Gets all users <see cref="TraktCalendarMovie" />s airing during the given time period.
        /// <para>OAuth authorization required.</para>
        /// <para>
        /// See <a href="http://docs.trakt.apiary.io/#reference/calendars/my-movies/get-movies">"Trakt API Doc - Calendars: My Movies"</a> for more information.
        /// </para>
        /// </summary>
        /// <param name="startDate">The date, on which the time period should start. Defaults to today. Will be converted to the Trakt date-format.</param>
        /// <param name="days">1 - 31 days, specifying the length of the time period. Defaults to 7 days.</param>
        /// <param name="extendedInfo">
        /// The extended info, which determines how much data about the movies should be queried.
        /// See also <seealso cref="TraktExtendedInfo" />.
        /// </param>
        /// <param name="filter">Optional filters for genres, languages, year, runtimes, ratings, etc. See also <seealso cref="TraktCalendarFilter" />.</param>
        /// <returns>A list of <see cref="TraktCalendarMovie" /> instances.</returns>
        /// <exception cref="TraktException">Thrown, if the request fails.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if the given days value is not between 1 and 31.</exception>
        public async Task<TraktListResponse<TraktCalendarMovie>> GetUserMoviesAsync(DateTime? startDate = null, int? days = null,
                                                                                    TraktExtendedInfo extendedInfo = null,
                                                                                    TraktCalendarFilter filter = null)
        {
            var requestHandler = new TraktRequestHandler(Client);

            return await requestHandler.ExecuteListRequestAsync(new TraktCalendarUserMoviesRequest
            {
                StartDate = startDate,
                Days = days,
                ExtendedInfo = extendedInfo,
                Filter = filter
            });
        }

        /// <summary>
        /// Gets all users <see cref="TraktCalendarMovie" />s with a DVD release during the given time period.
        /// <para>OAuth authorization required.</para>
        /// <para>
        /// See <a href="http://docs.trakt.apiary.io/#reference/calendars/my-dvd/get-dvd-releases">"Trakt API Doc - Calendars: My DVD"</a> for more information.
        /// </para>
        /// </summary>
        /// <param name="startDate">The date, on which the time period should start. Defaults to today. Will be converted to the Trakt date-format.</param>
        /// <param name="days">1 - 31 days, specifying the length of the time period. Defaults to 7 days.</param>
        /// <param name="extendedInfo">
        /// The extended info, which determines how much data about the movies should be queried.
        /// See also <seealso cref="TraktExtendedInfo" />.
        /// </param>
        /// <param name="filter">Optional filters for genres, languages, year, runtimes, ratings, etc. See also <seealso cref="TraktCalendarFilter" />.</param>
        /// <returns>A list of <see cref="TraktCalendarMovie" /> instances.</returns>
        /// <exception cref="TraktException">Thrown, if the request fails.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if the given days value is not between 1 and 31.</exception>
        public async Task<TraktListResponse<TraktCalendarMovie>> GetUserDVDMoviesAsync(DateTime? startDate = null, int? days = null,
                                                                                       TraktExtendedInfo extendedInfo = null,
                                                                                       TraktCalendarFilter filter = null)
        {
            var requestHandler = new TraktRequestHandler(Client);

            return await requestHandler.ExecuteListRequestAsync(new TraktCalendarUserDVDMoviesRequest
            {
                StartDate = startDate,
                Days = days,
                ExtendedInfo = extendedInfo,
                Filter = filter
            });
        }

        /// <summary>
        /// Gets all <see cref="TraktCalendarShow" />s airing during the given time period.
        /// <para>OAuth authorization not required.</para>
        /// <para>
        /// See <a href="http://docs.trakt.apiary.io/#reference/calendars/all-shows/get-shows">"Trakt API Doc - Calendars: All Shows"</a> for more information.
        /// </para>
        /// </summary>
        /// <param name="startDate">The date, on which the time period should start. Defaults to today. Will be converted to the Trakt date-format.</param>
        /// <param name="days">1 - 31 days, specifying the length of the time period. Defaults to 7 days.</param>
        /// <param name="extendedInfo">
        /// The extended info, which determines how much data about the shows should be queried.
        /// See also <seealso cref="TraktExtendedInfo" />.
        /// </param>
        /// <param name="filter">Optional filters for genres, languages, year, runtimes, ratings, etc. See also <seealso cref="TraktCalendarFilter" />.</param>
        /// <returns>A list of <see cref="TraktCalendarShow" /> instances.</returns>
        /// <exception cref="TraktException">Thrown, if the request fails.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if the given days value is not between 1 and 31.</exception>
        public async Task<TraktListResponse<TraktCalendarShow>> GetAllShowsAsync(DateTime? startDate = null, int? days = null,
                                                                                 TraktExtendedInfo extendedInfo = null,
                                                                                 TraktCalendarFilter filter = null)
        {
            var requestHandler = new TraktRequestHandler(Client);

            return await requestHandler.ExecuteListRequestAsync(new TraktCalendarAllShowsRequest
            {
                StartDate = startDate,
                Days = days,
                ExtendedInfo = extendedInfo,
                Filter = filter
            });
        }

        /// <summary>
        /// Gets all new <see cref="TraktCalendarShow" />s airing during the given time period.
        /// <para>OAuth authorization not required.</para>
        /// <para>
        /// See <a href="http://docs.trakt.apiary.io/#reference/calendars/all-new-shows/get-new-shows">"Trakt API Doc - Calendars: All New Shows"</a> for more information.
        /// </para>
        /// </summary>
        /// <param name="startDate">The date, on which the time period should start. Defaults to today. Will be converted to the Trakt date-format.</param>
        /// <param name="days">1 - 31 days, specifying the length of the time period. Defaults to 7 days.</param>
        /// <param name="extendedInfo">
        /// The extended info, which determines how much data about the shows should be queried.
        /// See also <seealso cref="TraktExtendedInfo" />.
        /// </param>
        /// <param name="filter">Optional filters for genres, languages, year, runtimes, ratings, etc. See also <seealso cref="TraktCalendarFilter" />.</param>
        /// <returns>A list of <see cref="TraktCalendarShow" /> instances.</returns>
        /// <exception cref="TraktException">Thrown, if the request fails.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if the given days value is not between 1 and 31.</exception>
        public async Task<TraktListResponse<TraktCalendarShow>> GetAllNewShowsAsync(DateTime? startDate = null, int? days = null,
                                                                                    TraktExtendedInfo extendedInfo = null,
                                                                                    TraktCalendarFilter filter = null)
        {
            var requestHandler = new TraktRequestHandler(Client);

            return await requestHandler.ExecuteListRequestAsync(new TraktCalendarAllNewShowsRequest
            {
                StartDate = startDate,
                Days = days,
                ExtendedInfo = extendedInfo,
                Filter = filter
            });
        }

        /// <summary>
        /// Gets all season premieres airing during the given time period.
        /// <para>OAuth authorization not required.</para>
        /// <para>
        /// See <a href="http://docs.trakt.apiary.io/#reference/calendars/all-season-premieres/get-season-premieres">"Trakt API Doc - Calendars: All Season Premieres"</a> for more information.
        /// </para>
        /// </summary>
        /// <param name="startDate">The date, on which the time period should start. Defaults to today. Will be converted to the Trakt date-format.</param>
        /// <param name="days">1 - 31 days, specifying the length of the time period. Defaults to 7 days.</param>
        /// <param name="extendedInfo">
        /// The extended info, which determines how much data about the shows should be queried.
        /// See also <seealso cref="TraktExtendedInfo" />.
        /// </param>
        /// <param name="filter">Optional filters for genres, languages, year, runtimes, ratings, etc. See also <seealso cref="TraktCalendarFilter" />.</param>
        /// <returns>A list of <see cref="TraktCalendarShow" /> instances.</returns>
        /// <exception cref="TraktException">Thrown, if the request fails.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if the given days value is not between 1 and 31.</exception>
        public async Task<TraktListResponse<TraktCalendarShow>> GetAllSeasonPremieresAsync(DateTime? startDate = null, int? days = null,
                                                                                           TraktExtendedInfo extendedInfo = null,
                                                                                           TraktCalendarFilter filter = null)
        {
            var requestHandler = new TraktRequestHandler(Client);

            return await requestHandler.ExecuteListRequestAsync(new TraktCalendarAllSeasonPremieresRequest
            {
                StartDate = startDate,
                Days = days,
                ExtendedInfo = extendedInfo,
                Filter = filter
            });
        }

        /// <summary>
        /// Gets all <see cref="TraktCalendarMovie" />s airing during the given time period.
        /// <para>OAuth authorization not required.</para>
        /// <para>
        /// See <a href="http://docs.trakt.apiary.io/#reference/calendars/all-movies/get-movies">"Trakt API Doc - Calendars: All Movies"</a> for more information.
        /// </para>
        /// </summary>
        /// <param name="startDate">The date, on which the time period should start. Defaults to today. Will be converted to the Trakt date-format.</param>
        /// <param name="days">1 - 31 days, specifying the length of the time period. Defaults to 7 days.</param>
        /// <param name="extendedInfo">
        /// The extended info, which determines how much data about the movies should be queried.
        /// See also <seealso cref="TraktExtendedInfo" />.
        /// </param>
        /// <param name="filter">Optional filters for genres, languages, year, runtimes, ratings, etc. See also <seealso cref="TraktCalendarFilter" />.</param>
        /// <returns>A list of <see cref="TraktCalendarMovie" /> instances.</returns>
        /// <exception cref="TraktException">Thrown, if the request fails.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if the given days value is not between 1 and 31.</exception>
        public async Task<TraktListResponse<TraktCalendarMovie>> GetAllMoviesAsync(DateTime? startDate = null, int? days = null,
                                                                                   TraktExtendedInfo extendedInfo = null,
                                                                                   TraktCalendarFilter filter = null)
        {
            var requestHandler = new TraktRequestHandler(Client);

            return await requestHandler.ExecuteListRequestAsync(new TraktCalendarAllMoviesRequest
            {
                StartDate = startDate,
                Days = days,
                ExtendedInfo = extendedInfo,
                Filter = filter
            });
        }

        /// <summary>
        /// Gets all <see cref="TraktCalendarMovie" />s with a DVD release during the given time period.
        /// <para>OAuth authorization not required.</para>
        /// <para>
        /// See <a href="http://docs.trakt.apiary.io/#reference/calendars/all-movies/get-dvd-releases">"Trakt API Doc - Calendars: All DVD"</a> for more information.
        /// </para>
        /// </summary>
        /// <param name="startDate">The date, on which the time period should start. Defaults to today. Will be converted to the Trakt date-format.</param>
        /// <param name="days">1 - 31 days, specifying the length of the time period. Defaults to 7 days.</param>
        /// <param name="extendedInfo">
        /// The extended info, which determines how much data about the movies should be queried.
        /// See also <seealso cref="TraktExtendedInfo" />.
        /// </param>
        /// <param name="filter">Optional filters for genres, languages, year, runtimes, ratings, etc. See also <seealso cref="TraktCalendarFilter" />.</param>
        /// <returns>A list of <see cref="TraktCalendarMovie" /> instances.</returns>
        /// <exception cref="TraktException">Thrown, if the request fails.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if the given days value is not between 1 and 31.</exception>
        public async Task<TraktListResponse<TraktCalendarMovie>> GetAllDVDMoviesAsync(DateTime? startDate = null, int? days = null,
                                                                                      TraktExtendedInfo extendedInfo = null,
                                                                                      TraktCalendarFilter filter = null)
        {
            var requestHandler = new TraktRequestHandler(Client);

            return await requestHandler.ExecuteListRequestAsync(new TraktCalendarAllDVDMoviesRequest
            {
                StartDate = startDate,
                Days = days,
                ExtendedInfo = extendedInfo,
                Filter = filter
            });
        }
    }
}
