﻿namespace TraktApiSharp.Tests.Objects.Post.Syncs.History
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TraktApiSharp.Objects.Get.Episodes;
    using TraktApiSharp.Objects.Get.Episodes.Implementations;
    using TraktApiSharp.Objects.Get.Movies;
    using TraktApiSharp.Objects.Get.Movies.Implementations;
    using TraktApiSharp.Objects.Get.Shows;
    using TraktApiSharp.Objects.Get.Shows.Implementations;
    using TraktApiSharp.Objects.Post;
    using TraktApiSharp.Objects.Post.Syncs.History;

    [TestClass]
    public class TraktSyncHistoryRemovePostBuilderTests
    {
        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddMovie()
        {
            var movie1 = new TraktMovie
            {
                Title = "movie1",
                Year = 2016,
                Ids = (ITraktMovieIds)new TraktMovieIds
                {
                    Trakt = 1,
                    Slug = "movie1",
                    Imdb = "imdb1",
                    Tmdb = 1234
                }
            };

            var builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddMovie(movie1);

            var historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().NotBeNull().And.HaveCount(1);
            historyPost.HistoryIds.Should().BeNull();

            builder.AddMovie(movie1);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().NotBeNull().And.HaveCount(1);
            historyPost.HistoryIds.Should().BeNull();

            movie1.Ids.Trakt = 2;

            builder.AddMovie(movie1);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().NotBeNull().And.HaveCount(1);
            historyPost.HistoryIds.Should().BeNull();

            var movies = historyPost.Movies.ToArray();

            movies[0].Should().NotBeNull();
            movies[0].Title.Should().Be("movie1");
            movies[0].Year.Should().Be(2016);
            movies[0].Ids.Should().NotBeNull();
            movies[0].Ids.Trakt.Should().Be(2U);
            movies[0].Ids.Slug.Should().Be("movie1");
            movies[0].Ids.Imdb.Should().Be("imdb1");
            movies[0].Ids.Tmdb.Should().Be(1234U);
            movies[0].WatchedAt.Should().NotHaveValue();

            var movie2 = new TraktMovie
            {
                Title = "movie2",
                Year = 2016,
                Ids = (ITraktMovieIds)new TraktMovieIds
                {
                    Trakt = 3,
                    Slug = "movie2",
                    Imdb = "imdb2",
                    Tmdb = 12345
                }
            };

            builder.AddMovie(movie2);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().NotBeNull().And.HaveCount(2);
            historyPost.HistoryIds.Should().BeNull();

            movies = historyPost.Movies.ToArray();

            movies[0].Should().NotBeNull();
            movies[0].Title.Should().Be("movie1");
            movies[0].Year.Should().Be(2016);
            movies[0].Ids.Should().NotBeNull();
            movies[0].Ids.Trakt.Should().Be(2U);
            movies[0].Ids.Slug.Should().Be("movie1");
            movies[0].Ids.Imdb.Should().Be("imdb1");
            movies[0].Ids.Tmdb.Should().Be(1234U);
            movies[0].WatchedAt.Should().NotHaveValue();

            movies[1].Should().NotBeNull();
            movies[1].Title.Should().Be("movie2");
            movies[1].Year.Should().Be(2016);
            movies[1].Ids.Should().NotBeNull();
            movies[1].Ids.Trakt.Should().Be(3U);
            movies[1].Ids.Slug.Should().Be("movie2");
            movies[1].Ids.Imdb.Should().Be("imdb2");
            movies[1].Ids.Tmdb.Should().Be(12345U);
            movies[1].WatchedAt.Should().NotHaveValue();
        }

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddMovieArgumentExceptions()
        {
            var builder = TraktSyncHistoryRemovePost.Builder();

            Action act = () => builder.AddMovie(null);
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddMovie(new TraktMovie());
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddMovie(new TraktMovie { Ids = (ITraktMovieIds)new TraktMovieIds() });
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddMovie(new TraktMovie { Ids = (ITraktMovieIds)new TraktMovieIds { Trakt = 1 }, Year = 123 });
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddMovie(new TraktMovie { Ids = (ITraktMovieIds)new TraktMovieIds { Trakt = 1 }, Year = 12345 });
            act.ShouldThrow<ArgumentException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddMoviesCollection()
        {
            var movie1 = new TraktMovie
            {
                Title = "movie1",
                Year = 2016,
                Ids = (ITraktMovieIds)new TraktMovieIds
                {
                    Trakt = 1,
                    Slug = "movie1",
                    Imdb = "imdb1",
                    Tmdb = 1234
                }
            };

            var movie2 = new TraktMovie
            {
                Title = "movie2",
                Year = 2016,
                Ids = (ITraktMovieIds)new TraktMovieIds
                {
                    Trakt = 3,
                    Slug = "movie2",
                    Imdb = "imdb2",
                    Tmdb = 12345
                }
            };

            var movies = new List<TraktMovie>
            {
                movie1,
                movie2
            };

            var builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddMovies(movies);

            var historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().NotBeNull().And.HaveCount(2);
            historyPost.HistoryIds.Should().BeNull();

            builder.AddMovies(movies);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().NotBeNull().And.HaveCount(2);
            historyPost.HistoryIds.Should().BeNull();

            movie1.Ids.Trakt = 2;

            movies = new List<TraktMovie>
            {
                movie1,
                movie2
            };

            builder.AddMovies(movies);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().NotBeNull().And.HaveCount(2);
            historyPost.HistoryIds.Should().BeNull();

            var historyMovies = historyPost.Movies.ToArray();

            historyMovies[0].Should().NotBeNull();
            historyMovies[0].Title.Should().Be("movie1");
            historyMovies[0].Year.Should().Be(2016);
            historyMovies[0].Ids.Should().NotBeNull();
            historyMovies[0].Ids.Trakt.Should().Be(2U);
            historyMovies[0].Ids.Slug.Should().Be("movie1");
            historyMovies[0].Ids.Imdb.Should().Be("imdb1");
            historyMovies[0].Ids.Tmdb.Should().Be(1234U);
            historyMovies[0].WatchedAt.Should().NotHaveValue();

            historyMovies[1].Should().NotBeNull();
            historyMovies[1].Title.Should().Be("movie2");
            historyMovies[1].Year.Should().Be(2016);
            historyMovies[1].Ids.Should().NotBeNull();
            historyMovies[1].Ids.Trakt.Should().Be(3U);
            historyMovies[1].Ids.Slug.Should().Be("movie2");
            historyMovies[1].Ids.Imdb.Should().Be("imdb2");
            historyMovies[1].Ids.Tmdb.Should().Be(12345U);
            historyMovies[1].WatchedAt.Should().NotHaveValue();
        }

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddMoviesCollectionArgumentExceptions()
        {
            var builder = TraktSyncHistoryRemovePost.Builder();

            Action act = () => builder.AddMovies(null);
            act.ShouldThrow<ArgumentNullException>();

            var movies = new List<TraktMovie>
            {
                new TraktMovie()
            };

            act = () => builder.AddMovies(movies);
            act.ShouldThrow<ArgumentNullException>();

            movies = new List<TraktMovie>
            {
                new TraktMovie { Ids = (ITraktMovieIds)new TraktMovieIds() }
            };

            act = () => builder.AddMovies(movies);
            act.ShouldThrow<ArgumentException>();

            movies = new List<TraktMovie>
            {
                new TraktMovie { Ids = (ITraktMovieIds)new TraktMovieIds { Trakt = 1 }, Year = 123 }
            };

            act = () => builder.AddMovies(movies);
            act.ShouldThrow<ArgumentException>();

            movies = new List<TraktMovie>
            {
                new TraktMovie { Ids = (ITraktMovieIds)new TraktMovieIds { Trakt = 1 }, Year = 12345 }
            };

            act = () => builder.AddMovies(movies);
            act.ShouldThrow<ArgumentException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddEpisode()
        {
            var episode1 = new TraktEpisode
            {
                Ids = new TraktEpisodeIds
                {
                    Trakt = 1,
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    Tvdb = 12345,
                    TvRage = 123456
                }
            };

            var builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddEpisode(episode1);

            var historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().NotBeNull().And.HaveCount(1);
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            builder.AddEpisode(episode1);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().NotBeNull().And.HaveCount(1);
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            episode1.Ids.Trakt = 2;

            builder.AddEpisode(episode1);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().NotBeNull().And.HaveCount(1);
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            var episodes = historyPost.Episodes.ToArray();

            episodes[0].Should().NotBeNull();
            episodes[0].Ids.Should().NotBeNull();
            episodes[0].Ids.Trakt.Should().Be(2U);
            episodes[0].Ids.Imdb.Should().Be("imdb1");
            episodes[0].Ids.Tmdb.Should().Be(1234U);
            episodes[0].Ids.Tvdb.Should().Be(12345U);
            episodes[0].Ids.TvRage.Should().Be(123456U);
            episodes[0].WatchedAt.Should().NotHaveValue();

            var episode2 = new TraktEpisode
            {
                Ids = new TraktEpisodeIds
                {
                    Trakt = 3,
                    Imdb = "imdb2",
                    Tmdb = 12345,
                    Tvdb = 123456,
                    TvRage = 1234567
                }
            };

            builder.AddEpisode(episode2);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().NotBeNull().And.HaveCount(2);
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            episodes = historyPost.Episodes.ToArray();

            episodes[0].Should().NotBeNull();
            episodes[0].Ids.Should().NotBeNull();
            episodes[0].Ids.Trakt.Should().Be(2U);
            episodes[0].Ids.Imdb.Should().Be("imdb1");
            episodes[0].Ids.Tmdb.Should().Be(1234U);
            episodes[0].Ids.Tvdb.Should().Be(12345U);
            episodes[0].Ids.TvRage.Should().Be(123456U);
            episodes[0].WatchedAt.Should().NotHaveValue();

            episodes[1].Should().NotBeNull();
            episodes[1].Ids.Should().NotBeNull();
            episodes[1].Ids.Trakt.Should().Be(3U);
            episodes[1].Ids.Imdb.Should().Be("imdb2");
            episodes[1].Ids.Tmdb.Should().Be(12345U);
            episodes[1].Ids.Tvdb.Should().Be(123456U);
            episodes[1].Ids.TvRage.Should().Be(1234567U);
            episodes[1].WatchedAt.Should().NotHaveValue();
        }

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddEpisodeArgumentExceptions()
        {
            var builder = TraktSyncHistoryRemovePost.Builder();

            Action act = () => builder.AddEpisode(null);
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddEpisode(new TraktEpisode());
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddEpisode(new TraktEpisode { Ids = new TraktEpisodeIds() });
            act.ShouldThrow<ArgumentException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddEpisodesCollection()
        {
            var episode1 = new TraktEpisode
            {
                Ids = new TraktEpisodeIds
                {
                    Trakt = 1,
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    Tvdb = 12345,
                    TvRage = 123456
                }
            };

            var episode2 = new TraktEpisode
            {
                Ids = new TraktEpisodeIds
                {
                    Trakt = 3,
                    Imdb = "imdb2",
                    Tmdb = 12345,
                    Tvdb = 123456,
                    TvRage = 1234567
                }
            };

            var episodes = new List<TraktEpisode>
            {
                episode1,
                episode2
            };

            var builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddEpisodes(episodes);

            var historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().NotBeNull().And.HaveCount(2);
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            builder.AddEpisodes(episodes);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().NotBeNull().And.HaveCount(2);
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            episode1.Ids.Trakt = 2;

            episodes = new List<TraktEpisode>
            {
                episode1,
                episode2
            };

            builder.AddEpisodes(episodes);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().NotBeNull().And.HaveCount(2);
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            var historyEpisodes = historyPost.Episodes.ToArray();

            historyEpisodes[0].Should().NotBeNull();
            historyEpisodes[0].Ids.Should().NotBeNull();
            historyEpisodes[0].Ids.Trakt.Should().Be(2U);
            historyEpisodes[0].Ids.Imdb.Should().Be("imdb1");
            historyEpisodes[0].Ids.Tmdb.Should().Be(1234U);
            historyEpisodes[0].Ids.Tvdb.Should().Be(12345U);
            historyEpisodes[0].Ids.TvRage.Should().Be(123456U);
            historyEpisodes[0].WatchedAt.Should().NotHaveValue();

            historyEpisodes[1].Should().NotBeNull();
            historyEpisodes[1].Ids.Should().NotBeNull();
            historyEpisodes[1].Ids.Trakt.Should().Be(3U);
            historyEpisodes[1].Ids.Imdb.Should().Be("imdb2");
            historyEpisodes[1].Ids.Tmdb.Should().Be(12345U);
            historyEpisodes[1].Ids.Tvdb.Should().Be(123456U);
            historyEpisodes[1].Ids.TvRage.Should().Be(1234567U);
            historyEpisodes[1].WatchedAt.Should().NotHaveValue();
        }

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddEpisodesCollectionArgumentExceptions()
        {
            var builder = TraktSyncHistoryRemovePost.Builder();

            Action act = () => builder.AddEpisodes(null);
            act.ShouldThrow<ArgumentNullException>();

            var episodes = new List<TraktEpisode>
            {
                new TraktEpisode()
            };

            act = () => builder.AddEpisodes(episodes);
            act.ShouldThrow<ArgumentNullException>();

            episodes = new List<TraktEpisode>
            {
                new TraktEpisode { Ids = new TraktEpisodeIds() }
            };

            act = () => builder.AddEpisodes(episodes);
            act.ShouldThrow<ArgumentException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddShow()
        {
            var show1 = new TraktShow
            {
                Title = "show1",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 1,
                    Slug = "show1",
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    Tvdb = 12345,
                    TvRage = 123456
                }
            };

            var builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddShow(show1);

            var historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            builder.AddShow(show1);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            show1.Ids.Trakt = 2;

            builder.AddShow(show1);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            var shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().BeNull();

            var show2 = new TraktShow
            {
                Title = "show2",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 3,
                    Slug = "show2",
                    Imdb = "imdb2",
                    Tmdb = 12345,
                    Tvdb = 123456,
                    TvRage = 1234567
                }
            };

            builder.AddShow(show2);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(2);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().BeNull();

            shows[1].Should().NotBeNull();
            shows[1].Title.Should().Be("show2");
            shows[1].Year.Should().Be(2016);
            shows[1].Ids.Should().NotBeNull();
            shows[1].Ids.Trakt.Should().Be(3U);
            shows[1].Ids.Slug.Should().Be("show2");
            shows[1].Ids.Imdb.Should().Be("imdb2");
            shows[1].Ids.Tmdb.Should().Be(12345U);
            shows[1].Ids.Tvdb.Should().Be(123456U);
            shows[1].Ids.TvRage.Should().Be(1234567U);
            shows[1].WatchedAt.Should().NotHaveValue();
            shows[1].Seasons.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddShowArgumentExceptions()
        {
            var builder = TraktSyncHistoryRemovePost.Builder();

            Action act = () => builder.AddShow(null);
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddShow(new TraktShow());
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds() });
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 }, Year = 123 });
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 }, Year = 12345 });
            act.ShouldThrow<ArgumentException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddShowsCollection()
        {
            var show1 = new TraktShow
            {
                Title = "show1",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 1,
                    Slug = "show1",
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    Tvdb = 12345,
                    TvRage = 123456
                }
            };

            var show2 = new TraktShow
            {
                Title = "show2",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 3,
                    Slug = "show2",
                    Imdb = "imdb2",
                    Tmdb = 12345,
                    Tvdb = 123456,
                    TvRage = 1234567
                }
            };

            var shows = new List<TraktShow>
            {
                show1,
                show2
            };

            var builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddShows(shows);

            var historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(2);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            builder.AddShows(shows);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(2);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            show1.Ids.Trakt = 2;

            shows = new List<TraktShow>
            {
                show1,
                show2
            };

            builder.AddShows(shows);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(2);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            var historyShows = historyPost.Shows.ToArray();

            historyShows[0].Should().NotBeNull();
            historyShows[0].Title.Should().Be("show1");
            historyShows[0].Year.Should().Be(2016);
            historyShows[0].Ids.Should().NotBeNull();
            historyShows[0].Ids.Trakt.Should().Be(2U);
            historyShows[0].Ids.Slug.Should().Be("show1");
            historyShows[0].Ids.Imdb.Should().Be("imdb1");
            historyShows[0].Ids.Tmdb.Should().Be(1234U);
            historyShows[0].Ids.Tvdb.Should().Be(12345U);
            historyShows[0].Ids.TvRage.Should().Be(123456U);
            historyShows[0].WatchedAt.Should().NotHaveValue();
            historyShows[0].Seasons.Should().BeNull();

            historyShows[1].Should().NotBeNull();
            historyShows[1].Title.Should().Be("show2");
            historyShows[1].Year.Should().Be(2016);
            historyShows[1].Ids.Should().NotBeNull();
            historyShows[1].Ids.Trakt.Should().Be(3U);
            historyShows[1].Ids.Slug.Should().Be("show2");
            historyShows[1].Ids.Imdb.Should().Be("imdb2");
            historyShows[1].Ids.Tmdb.Should().Be(12345U);
            historyShows[1].Ids.Tvdb.Should().Be(123456U);
            historyShows[1].Ids.TvRage.Should().Be(1234567U);
            historyShows[1].WatchedAt.Should().NotHaveValue();
            historyShows[1].Seasons.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddShowsCollectionArgumentExceptions()
        {
            var builder = TraktSyncHistoryRemovePost.Builder();

            Action act = () => builder.AddShows(null);
            act.ShouldThrow<ArgumentNullException>();

            var shows = new List<TraktShow>
            {
                new TraktShow()
            };

            act = () => builder.AddShows(shows);
            act.ShouldThrow<ArgumentNullException>();

            shows = new List<TraktShow>
            {
                new TraktShow { Ids = (ITraktShowIds)new TraktShowIds() }
            };

            act = () => builder.AddShows(shows);
            act.ShouldThrow<ArgumentException>();

            shows = new List<TraktShow>
            {
                new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 }, Year = 123 }
            };

            act = () => builder.AddShows(shows);
            act.ShouldThrow<ArgumentException>();

            shows = new List<TraktShow>
            {
                new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 }, Year = 12345 }
            };

            act = () => builder.AddShows(shows);
            act.ShouldThrow<ArgumentException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddShowWithSeasons()
        {
            var show1 = new TraktShow
            {
                Title = "show1",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 1,
                    Slug = "show1",
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    Tvdb = 12345,
                    TvRage = 123456
                }
            };

            var builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddShow(show1, 1);

            var historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            var shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(1U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(1);

            var show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            builder.AddShow(show1, 1, 2);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(1U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            builder.AddShow(show1, 1, 2, 3);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(1U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(3);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            show1Seasons[2].Number.Should().Be(3);
            show1Seasons[2].Episodes.Should().BeNull();
            show1Seasons[2].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            show1.Ids.Trakt = 2;

            builder.AddShow(show1, 1, 2, 3);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(3);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            show1Seasons[2].Number.Should().Be(3);
            show1Seasons[2].Episodes.Should().BeNull();
            show1Seasons[2].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            var show2 = new TraktShow
            {
                Title = "show2",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 3,
                    Slug = "show2",
                    Imdb = "imdb2",
                    Tmdb = 12345,
                    Tvdb = 123456,
                    TvRage = 1234567
                }
            };

            builder.AddShow(show2, 1, 2, 3);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(2);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(3);

            shows[1].Should().NotBeNull();
            shows[1].Title.Should().Be("show2");
            shows[1].Year.Should().Be(2016);
            shows[1].Ids.Should().NotBeNull();
            shows[1].Ids.Trakt.Should().Be(3U);
            shows[1].Ids.Slug.Should().Be("show2");
            shows[1].Ids.Imdb.Should().Be("imdb2");
            shows[1].Ids.Tmdb.Should().Be(12345U);
            shows[1].Ids.Tvdb.Should().Be(123456U);
            shows[1].Ids.TvRage.Should().Be(1234567U);
            shows[1].WatchedAt.Should().NotHaveValue();
            shows[1].Seasons.Should().NotBeNull().And.HaveCount(3);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            show1Seasons[2].Number.Should().Be(3);
            show1Seasons[2].Episodes.Should().BeNull();
            show1Seasons[2].WatchedAt.Should().NotHaveValue();

            var show2Seasons = shows[1].Seasons.ToArray();

            show2Seasons[0].Number.Should().Be(1);
            show2Seasons[0].Episodes.Should().BeNull();
            show2Seasons[0].WatchedAt.Should().NotHaveValue();

            show2Seasons[1].Number.Should().Be(2);
            show2Seasons[1].Episodes.Should().BeNull();
            show2Seasons[1].WatchedAt.Should().NotHaveValue();

            show2Seasons[2].Number.Should().Be(3);
            show2Seasons[2].Episodes.Should().BeNull();
            show2Seasons[2].WatchedAt.Should().NotHaveValue();
        }

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddShowWithSeasonsArgumentExceptions()
        {
            var builder = TraktSyncHistoryRemovePost.Builder();

            Action act = () => builder.AddShow(null, 1, 2, 3, 4);
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddShow(new TraktShow(), 1, 2, 3, 4);
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds() }, 1, 2, 3, 4);
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 }, Year = 123 }, 1, 2, 3, 4);
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 }, Year = 12345 }, 1, 2, 3, 4);
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, -1);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, 1, 2, -1);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostHistorySeasons { -1 });
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostHistorySeasons { 1, -1 });
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddShowWithSeasonsArray()
        {
            var show1 = new TraktShow
            {
                Title = "show1",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 1,
                    Slug = "show1",
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    Tvdb = 12345,
                    TvRage = 123456
                }
            };

            var seasons = new int[] { 1 };

            var builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddShow(show1, seasons);

            var historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            var shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(1U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(1);

            var show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            seasons = new int[] { 1, 2 };

            builder.AddShow(show1, seasons);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(1U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            seasons = new int[] { 1, 2, 3 };

            builder.AddShow(show1, seasons);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(1U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(3);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            show1Seasons[2].Number.Should().Be(3);
            show1Seasons[2].Episodes.Should().BeNull();
            show1Seasons[2].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            show1.Ids.Trakt = 2;

            seasons = new int[] { 1, 2, 3 };

            builder.AddShow(show1, seasons);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(3);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            show1Seasons[2].Number.Should().Be(3);
            show1Seasons[2].Episodes.Should().BeNull();
            show1Seasons[2].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            var show2 = new TraktShow
            {
                Title = "show2",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 3,
                    Slug = "show2",
                    Imdb = "imdb2",
                    Tmdb = 12345,
                    Tvdb = 123456,
                    TvRage = 1234567
                }
            };

            seasons = new int[] { 1, 2, 3 };

            builder.AddShow(show2, seasons);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(2);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(3);

            shows[1].Should().NotBeNull();
            shows[1].Title.Should().Be("show2");
            shows[1].Year.Should().Be(2016);
            shows[1].Ids.Should().NotBeNull();
            shows[1].Ids.Trakt.Should().Be(3U);
            shows[1].Ids.Slug.Should().Be("show2");
            shows[1].Ids.Imdb.Should().Be("imdb2");
            shows[1].Ids.Tmdb.Should().Be(12345U);
            shows[1].Ids.Tvdb.Should().Be(123456U);
            shows[1].Ids.TvRage.Should().Be(1234567U);
            shows[1].WatchedAt.Should().NotHaveValue();
            shows[1].Seasons.Should().NotBeNull().And.HaveCount(3);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            show1Seasons[2].Number.Should().Be(3);
            show1Seasons[2].Episodes.Should().BeNull();
            show1Seasons[2].WatchedAt.Should().NotHaveValue();

            var show2Seasons = shows[1].Seasons.ToArray();

            show2Seasons[0].Number.Should().Be(1);
            show2Seasons[0].Episodes.Should().BeNull();
            show2Seasons[0].WatchedAt.Should().NotHaveValue();

            show2Seasons[1].Number.Should().Be(2);
            show2Seasons[1].Episodes.Should().BeNull();
            show2Seasons[1].WatchedAt.Should().NotHaveValue();

            show2Seasons[2].Number.Should().Be(3);
            show2Seasons[2].Episodes.Should().BeNull();
            show2Seasons[2].WatchedAt.Should().NotHaveValue();
        }

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddShowWithSeasonsArrayArgumentExceptions()
        {
            var builder = TraktSyncHistoryRemovePost.Builder();

            var seasons = new int[] { 1, 2, 3, 4 };

            Action act = () => builder.AddShow(null, seasons);
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddShow(new TraktShow(), seasons);
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds() }, seasons);
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 }, Year = 123 }, seasons);
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 }, Year = 12345 }, seasons);
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, default(int[]));
            act.ShouldThrow<ArgumentNullException>();

            seasons = new int[] { -1 };

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, seasons);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            seasons = new int[] { 1, 2, -1 };

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, seasons);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostHistorySeasons { -1 });
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostHistorySeasons { 1, -1 });
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddShowWithSeasonsAndEpisodes()
        {
            var show1 = new TraktShow
            {
                Title = "show1",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 1,
                    Slug = "show1",
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    Tvdb = 12345,
                    TvRage = 123456
                }
            };

            var builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddShow(show1, new PostHistorySeasons { { 1, new PostHistoryEpisodes { 1 } } }); // season 1 - episode 1

            var historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            var shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(1U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(1);

            var show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(1);
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            var show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[0].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            builder.AddShow(show1, new PostHistorySeasons { { 1, new PostHistoryEpisodes { 1, 2 } } }); // season 1 - episode 1, 2

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(1U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(1);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(2);
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[0].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[1].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            builder.AddShow(show1, new PostHistorySeasons { { 1, new PostHistoryEpisodes { 1, 2, 3 } } }); // season 1 - episode 1, 2, 3

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(1U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(1);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[0].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[1].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[2].Number.Should().Be(3);
            show1Season1Episodes[2].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            builder.AddShow(show1, new PostHistorySeasons {
                { 1, new PostHistoryEpisodes { 1, 2, 3 } }, // season 1 - episode 1, 2, 3
                { 2, new PostHistoryEpisodes { 4 } }        // season 2 - episode 4
            });

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(1U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().NotBeNull().And.HaveCount(1);
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[0].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[1].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[2].Number.Should().Be(3);
            show1Season1Episodes[2].WatchedAt.Should().NotHaveValue();

            var show1Season2Episodes = show1Seasons[1].Episodes.ToArray();

            show1Season2Episodes[0].Number.Should().Be(4);
            show1Season2Episodes[0].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            builder.AddShow(show1, new PostHistorySeasons {
                { 1, new PostHistoryEpisodes { 1, 2, 3 } }, // season 1 - episode 1, 2, 3
                { 2, new PostHistoryEpisodes { 4, 5 } }     // season 2 - episode 4, 5
            });

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(1U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().NotBeNull().And.HaveCount(2);
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[0].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[1].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[2].Number.Should().Be(3);
            show1Season1Episodes[2].WatchedAt.Should().NotHaveValue();

            show1Season2Episodes = show1Seasons[1].Episodes.ToArray();

            show1Season2Episodes[0].Number.Should().Be(4);
            show1Season2Episodes[0].WatchedAt.Should().NotHaveValue();

            show1Season2Episodes[1].Number.Should().Be(5);
            show1Season2Episodes[1].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            builder.AddShow(show1, new PostHistorySeasons {
                { 1, new PostHistoryEpisodes { 1, 2, 3 } }, // season 1 - episode 1, 2, 3
                { 2, new PostHistoryEpisodes { 4, 5, 6 } }  // season 2 - episode 4, 5, 6
            });

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(1U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().NotBeNull().And.HaveCount(3);
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[0].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[1].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[2].Number.Should().Be(3);
            show1Season1Episodes[2].WatchedAt.Should().NotHaveValue();

            show1Season2Episodes = show1Seasons[1].Episodes.ToArray();

            show1Season2Episodes[0].Number.Should().Be(4);
            show1Season2Episodes[0].WatchedAt.Should().NotHaveValue();

            show1Season2Episodes[1].Number.Should().Be(5);
            show1Season2Episodes[1].WatchedAt.Should().NotHaveValue();

            show1Season2Episodes[2].Number.Should().Be(6);
            show1Season2Episodes[2].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            show1.Ids.Trakt = 2;

            builder.AddShow(show1, new PostHistorySeasons {
                { 1, new PostHistoryEpisodes { 1, 2, 3 } }, // season 1 - episode 1, 2, 3
                { 2, new PostHistoryEpisodes { 4, 5, 6 } }  // season 2 - episode 4, 5, 6
            });

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().NotBeNull().And.HaveCount(3);
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[0].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[1].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[2].Number.Should().Be(3);
            show1Season1Episodes[2].WatchedAt.Should().NotHaveValue();

            show1Season2Episodes = show1Seasons[1].Episodes.ToArray();

            show1Season2Episodes[0].Number.Should().Be(4);
            show1Season2Episodes[0].WatchedAt.Should().NotHaveValue();

            show1Season2Episodes[1].Number.Should().Be(5);
            show1Season2Episodes[1].WatchedAt.Should().NotHaveValue();

            show1Season2Episodes[2].Number.Should().Be(6);
            show1Season2Episodes[2].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            var show2 = new TraktShow
            {
                Title = "show2",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 3,
                    Slug = "show2",
                    Imdb = "imdb2",
                    Tmdb = 12345,
                    Tvdb = 123456,
                    TvRage = 1234567
                }
            };

            builder.AddShow(show2, new PostHistorySeasons {
                { 1, new PostHistoryEpisodes { 1, 2, 3 } }, // season 1 - episode 1, 2, 3
                { 2, new PostHistoryEpisodes { 4, 5, 6 } }  // season 2 - episode 4, 5, 6
            });

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(2);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show1");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            shows[1].Should().NotBeNull();
            shows[1].Title.Should().Be("show2");
            shows[1].Year.Should().Be(2016);
            shows[1].Ids.Should().NotBeNull();
            shows[1].Ids.Trakt.Should().Be(3U);
            shows[1].Ids.Slug.Should().Be("show2");
            shows[1].Ids.Imdb.Should().Be("imdb2");
            shows[1].Ids.Tmdb.Should().Be(12345U);
            shows[1].Ids.Tvdb.Should().Be(123456U);
            shows[1].Ids.TvRage.Should().Be(1234567U);
            shows[1].WatchedAt.Should().NotHaveValue();
            shows[1].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().NotBeNull().And.HaveCount(3);
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[0].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[1].WatchedAt.Should().NotHaveValue();

            show1Season1Episodes[2].Number.Should().Be(3);
            show1Season1Episodes[2].WatchedAt.Should().NotHaveValue();

            show1Season2Episodes = show1Seasons[1].Episodes.ToArray();

            show1Season2Episodes[0].Number.Should().Be(4);
            show1Season2Episodes[0].WatchedAt.Should().NotHaveValue();

            show1Season2Episodes[1].Number.Should().Be(5);
            show1Season2Episodes[1].WatchedAt.Should().NotHaveValue();

            show1Season2Episodes[2].Number.Should().Be(6);
            show1Season2Episodes[2].WatchedAt.Should().NotHaveValue();

            var show2Seasons = shows[1].Seasons.ToArray();

            show2Seasons[0].Number.Should().Be(1);
            show2Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);
            show2Seasons[0].WatchedAt.Should().NotHaveValue();

            show2Seasons[1].Number.Should().Be(2);
            show2Seasons[1].Episodes.Should().NotBeNull().And.HaveCount(3);
            show2Seasons[1].WatchedAt.Should().NotHaveValue();

            var show2Season1Episodes = show2Seasons[0].Episodes.ToArray();

            show2Season1Episodes[0].Number.Should().Be(1);
            show2Season1Episodes[0].WatchedAt.Should().NotHaveValue();

            show2Season1Episodes[1].Number.Should().Be(2);
            show2Season1Episodes[1].WatchedAt.Should().NotHaveValue();

            show2Season1Episodes[2].Number.Should().Be(3);
            show2Season1Episodes[2].WatchedAt.Should().NotHaveValue();

            var show2Season2Episodes = show2Seasons[1].Episodes.ToArray();

            show2Season2Episodes[0].Number.Should().Be(4);
            show2Season2Episodes[0].WatchedAt.Should().NotHaveValue();

            show2Season2Episodes[1].Number.Should().Be(5);
            show2Season2Episodes[1].WatchedAt.Should().NotHaveValue();

            show2Season2Episodes[2].Number.Should().Be(6);
            show2Season2Episodes[2].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddShow(show2, new PostHistorySeasons { 1 });  // season 1

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show2");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(3U);
            shows[0].Ids.Slug.Should().Be("show2");
            shows[0].Ids.Imdb.Should().Be("imdb2");
            shows[0].Ids.Tmdb.Should().Be(12345U);
            shows[0].Ids.Tvdb.Should().Be(123456U);
            shows[0].Ids.TvRage.Should().Be(1234567U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(1);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddShow(show2, new PostHistorySeasons {
                { 1, new PostHistoryEpisodes { 1, 2 } },    // season 1 - episode 1, 2
                2                                           // season 2
            });

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show2");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(3U);
            shows[0].Ids.Slug.Should().Be("show2");
            shows[0].Ids.Imdb.Should().Be("imdb2");
            shows[0].Ids.Tmdb.Should().Be(12345U);
            shows[0].Ids.Tvdb.Should().Be(123456U);
            shows[0].Ids.TvRage.Should().Be(1234567U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(2);
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();
            show1Seasons[1].WatchedAt.Should().NotHaveValue();

            // ---------------------------------------------------------

            builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddShow(show2, new PostHistorySeasons { 1, 2 }); // seasons 1, 2

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            shows = historyPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Title.Should().Be("show2");
            shows[0].Year.Should().Be(2016);
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(3U);
            shows[0].Ids.Slug.Should().Be("show2");
            shows[0].Ids.Imdb.Should().Be("imdb2");
            shows[0].Ids.Tmdb.Should().Be(12345U);
            shows[0].Ids.Tvdb.Should().Be(123456U);
            shows[0].Ids.TvRage.Should().Be(1234567U);
            shows[0].WatchedAt.Should().NotHaveValue();
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();
            show1Seasons[0].WatchedAt.Should().NotHaveValue();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();
            show1Seasons[1].WatchedAt.Should().NotHaveValue();
        }

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddShowWithSeasonsAndEpisodesArgumentExceptions()
        {
            var builder = TraktSyncHistoryRemovePost.Builder();

            Action act = () => builder.AddShow(null, new PostHistorySeasons { 1 });
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddShow(new TraktShow(), new PostHistorySeasons { 1 });
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds() }, new PostHistorySeasons { 1 });
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 }, Year = 123 }, new PostHistorySeasons { 1 });
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 }, Year = 12345 }, new PostHistorySeasons { 1 });
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, default(PostHistorySeasons));
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostHistorySeasons { -1 });
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostHistorySeasons { 1, -1 });
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostHistorySeasons { { -1, new PostHistoryEpisodes { 1, 2 } } });
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostHistorySeasons { { 1, new PostHistoryEpisodes { 1, -1 } } });
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostHistorySeasons { { 1, new PostHistoryEpisodes { 1, 2 } },
                                                                                                                         { 1, new PostHistoryEpisodes { 1, -1} } });
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostHistorySeasons { { 1, new PostHistoryEpisodes { 1, 2 } },
                                                                                                                         { -1, new PostHistoryEpisodes { 1, 2} } });
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddHistoryIds()
        {
            var builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddHistoryIds(1, 2, 3, 4);

            var historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().NotBeNull().And.HaveCount(4);
            historyPost.HistoryIds.Should().Contain(new List<ulong> { 1, 2, 3, 4 });

            builder.AddHistoryIds(5, 6, 7, 8);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().NotBeNull().And.HaveCount(8);
            historyPost.HistoryIds.Should().Contain(new List<ulong> { 1, 2, 3, 4, 5, 6, 7, 8 });
        }

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddHistoryIdsArgumentExceptions()
        {
            var builder = TraktSyncHistoryRemovePost.Builder();

            Action act = () => builder.AddHistoryIds(0, 2, 1, 4);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddHistoryIds(1, 2, 0, 4);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddHistoryIds(1, 2, 4, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddHistoryIdsArray()
        {
            var historyIds = new ulong[] { 1, 2, 3, 4 };

            var builder = TraktSyncHistoryRemovePost.Builder();

            builder.AddHistoryIds(historyIds);

            var historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().NotBeNull().And.HaveCount(4);
            historyPost.HistoryIds.Should().Contain(new List<ulong> { 1, 2, 3, 4 });

            historyIds = new ulong[] { 5, 6, 7, 8 };

            builder.AddHistoryIds(historyIds);

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.HistoryIds.Should().NotBeNull().And.HaveCount(8);
            historyPost.HistoryIds.Should().Contain(new List<ulong> { 1, 2, 3, 4, 5, 6, 7, 8 });
        }

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddHistoryIdsArrayArgumentExceptions()
        {
            var builder = TraktSyncHistoryRemovePost.Builder();

            Action act = () => builder.AddHistoryIds(default(ulong[]));
            act.ShouldThrow<ArgumentNullException>();

            var historyIds = new ulong[] { 0, 2, 1, 4 };

            act = () => builder.AddHistoryIds(historyIds);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            historyIds = new ulong[] { 1, 2, 0, 4 };

            act = () => builder.AddHistoryIds(historyIds);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            historyIds = new ulong[] { 1, 2, 4, 0 };

            act = () => builder.AddHistoryIds(historyIds);
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderReset()
        {
            var movie1 = new TraktMovie
            {
                Title = "movie1",
                Year = 2016,
                Ids = (ITraktMovieIds)new TraktMovieIds
                {
                    Trakt = 1,
                    Slug = "movie1",
                    Imdb = "imdb1",
                    Tmdb = 1234
                }
            };

            var episode1 = new TraktEpisode
            {
                Ids = new TraktEpisodeIds
                {
                    Trakt = 1,
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    Tvdb = 12345,
                    TvRage = 123456
                }
            };

            var show1 = new TraktShow
            {
                Title = "show1",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 1,
                    Slug = "show1",
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    Tvdb = 12345,
                    TvRage = 123456
                }
            };

            var builder = TraktSyncHistoryRemovePost.Builder();

            var historyPost = builder.AddMovie(movie1)
                                    .AddEpisode(episode1)
                                    .AddShow(show1)
                                    .AddHistoryIds(1)
                                    .Build();

            historyPost.Should().NotBeNull();
            historyPost.Movies.Should().NotBeNull().And.HaveCount(1);
            historyPost.Shows.Should().NotBeNull().And.HaveCount(1);
            historyPost.Episodes.Should().NotBeNull().And.HaveCount(1);
            historyPost.HistoryIds.Should().NotBeNull().And.HaveCount(1);

            builder.Reset();

            historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncHistoryRemovePostBuilderAddAll()
        {
            var movie1 = new TraktMovie
            {
                Title = "movie1",
                Year = 2016,
                Ids = (ITraktMovieIds)new TraktMovieIds
                {
                    Trakt = 1,
                    Slug = "movie1",
                    Imdb = "imdb1",
                    Tmdb = 1234
                }
            };

            var movie2 = new TraktMovie
            {
                Title = "movie2",
                Year = 2016,
                Ids = (ITraktMovieIds)new TraktMovieIds
                {
                    Trakt = 3,
                    Slug = "movie2",
                    Imdb = "imdb2",
                    Tmdb = 12345
                }
            };

            var episode1 = new TraktEpisode
            {
                Ids = new TraktEpisodeIds
                {
                    Trakt = 1,
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    Tvdb = 12345,
                    TvRage = 123456
                }
            };

            var episode2 = new TraktEpisode
            {
                Ids = new TraktEpisodeIds
                {
                    Trakt = 3,
                    Imdb = "imdb2",
                    Tmdb = 12345,
                    Tvdb = 123456,
                    TvRage = 1234567
                }
            };

            var show1 = new TraktShow
            {
                Title = "show1",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 1,
                    Slug = "show1",
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    Tvdb = 12345,
                    TvRage = 123456
                }
            };

            var show2 = new TraktShow
            {
                Title = "show2",
                Year = 2016,
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 2,
                    Slug = "show2",
                    Imdb = "imdb2",
                    Tmdb = 12345,
                    Tvdb = 123456,
                    TvRage = 1234567
                }
            };

            var show3 = new TraktShow
            {
                Ids = (ITraktShowIds)new TraktShowIds
                {
                    Trakt = 3,
                    Slug = "show3",
                    Imdb = "imdb3",
                    Tmdb = 123456,
                    Tvdb = 1234567,
                    TvRage = 12345678
                }
            };

            var builder = TraktSyncHistoryRemovePost.Builder();

            var historyPost = builder.Build();

            historyPost.Should().NotBeNull();
            historyPost.Movies.Should().BeNull();
            historyPost.Shows.Should().BeNull();
            historyPost.Episodes.Should().BeNull();
            historyPost.HistoryIds.Should().BeNull();

            // ------------------------------------------------------

            historyPost = builder.AddMovie(movie1)
                                .AddMovie(movie2)
                                .AddEpisode(episode1)
                                .AddEpisode(episode2)
                                .AddShow(show1)
                                .AddShow(show2, 1, 2)
                                .AddShow(show3, new PostHistorySeasons { { 1, new PostHistoryEpisodes { 1, 2, 3 } },
                                                                            { 2, new PostHistoryEpisodes { 2} } })
                                .AddHistoryIds(1, 2, 3, 4)
                                .Build();

            historyPost.Should().NotBeNull();
            historyPost.Movies.Should().NotBeNull().And.HaveCount(2);
            historyPost.Shows.Should().NotBeNull().And.HaveCount(3);
            historyPost.Episodes.Should().NotBeNull().And.HaveCount(2);
            historyPost.HistoryIds.Should().NotBeNull().And.HaveCount(4);
        }
    }
}
