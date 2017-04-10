﻿namespace TraktApiSharp.Tests.Objects.Post.Syncs.Watchlist
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
    using TraktApiSharp.Objects.Post.Syncs.Watchlist;

    [TestClass]
    public class TraktSyncWatchlistPostBuilderTests
    {
        [TestMethod]
        public void TestTraktSyncWatchlistPostBuilderAddMovie()
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

            var builder = TraktSyncWatchlistPost.Builder();

            builder.AddMovie(movie1);

            var watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().BeNull();
            watchlistPost.Movies.Should().NotBeNull().And.HaveCount(1);

            builder.AddMovie(movie1);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().BeNull();
            watchlistPost.Movies.Should().NotBeNull().And.HaveCount(1);

            movie1.Ids.Trakt = 2;

            builder.AddMovie(movie1);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().BeNull();
            watchlistPost.Movies.Should().NotBeNull().And.HaveCount(1);

            var movies = watchlistPost.Movies.ToArray();

            movies[0].Should().NotBeNull();
            movies[0].Title.Should().Be("movie1");
            movies[0].Year.Should().Be(2016);
            movies[0].Ids.Should().NotBeNull();
            movies[0].Ids.Trakt.Should().Be(2U);
            movies[0].Ids.Slug.Should().Be("movie1");
            movies[0].Ids.Imdb.Should().Be("imdb1");
            movies[0].Ids.Tmdb.Should().Be(1234U);

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

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().BeNull();
            watchlistPost.Movies.Should().NotBeNull().And.HaveCount(2);

            movies = watchlistPost.Movies.ToArray();

            movies[0].Should().NotBeNull();
            movies[0].Title.Should().Be("movie1");
            movies[0].Year.Should().Be(2016);
            movies[0].Ids.Should().NotBeNull();
            movies[0].Ids.Trakt.Should().Be(2U);
            movies[0].Ids.Slug.Should().Be("movie1");
            movies[0].Ids.Imdb.Should().Be("imdb1");
            movies[0].Ids.Tmdb.Should().Be(1234U);

            movies[1].Should().NotBeNull();
            movies[1].Title.Should().Be("movie2");
            movies[1].Year.Should().Be(2016);
            movies[1].Ids.Should().NotBeNull();
            movies[1].Ids.Trakt.Should().Be(3U);
            movies[1].Ids.Slug.Should().Be("movie2");
            movies[1].Ids.Imdb.Should().Be("imdb2");
            movies[1].Ids.Tmdb.Should().Be(12345U);
        }

        [TestMethod]
        public void TestTraktSyncWatchlistPostBuilderAddMovieArgumentExceptions()
        {
            var builder = TraktSyncWatchlistPost.Builder();

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
        public void TestTraktSyncWatchlistPostBuilderAddMovieCollection()
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

            var builder = TraktSyncWatchlistPost.Builder();

            builder.AddMovies(movies);

            var watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().BeNull();
            watchlistPost.Movies.Should().NotBeNull().And.HaveCount(2);

            builder.AddMovies(movies);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().BeNull();
            watchlistPost.Movies.Should().NotBeNull().And.HaveCount(2);

            movie1.Ids.Trakt = 2;

            movies = new List<TraktMovie>
            {
                movie1,
                movie2
            };

            builder.AddMovies(movies);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().BeNull();
            watchlistPost.Movies.Should().NotBeNull().And.HaveCount(2);

            var watchlistMovies = watchlistPost.Movies.ToArray();

            watchlistMovies[0].Should().NotBeNull();
            watchlistMovies[0].Title.Should().Be("movie1");
            watchlistMovies[0].Year.Should().Be(2016);
            watchlistMovies[0].Ids.Should().NotBeNull();
            watchlistMovies[0].Ids.Trakt.Should().Be(2U);
            watchlistMovies[0].Ids.Slug.Should().Be("movie1");
            watchlistMovies[0].Ids.Imdb.Should().Be("imdb1");
            watchlistMovies[0].Ids.Tmdb.Should().Be(1234U);

            watchlistMovies[1].Should().NotBeNull();
            watchlistMovies[1].Title.Should().Be("movie2");
            watchlistMovies[1].Year.Should().Be(2016);
            watchlistMovies[1].Ids.Should().NotBeNull();
            watchlistMovies[1].Ids.Trakt.Should().Be(3U);
            watchlistMovies[1].Ids.Slug.Should().Be("movie2");
            watchlistMovies[1].Ids.Imdb.Should().Be("imdb2");
            watchlistMovies[1].Ids.Tmdb.Should().Be(12345U);
        }

        [TestMethod]
        public void TestTraktSyncWatchlistPostBuilderAddMovieCollectionArgumentExceptions()
        {
            var builder = TraktSyncWatchlistPost.Builder();

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
        public void TestTraktSyncCollectionPostBuilderAddEpisode()
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

            var builder = TraktSyncWatchlistPost.Builder();

            builder.AddEpisode(episode1);

            var collectionPost = builder.Build();

            collectionPost.Should().NotBeNull();
            collectionPost.Episodes.Should().NotBeNull().And.HaveCount(1);
            collectionPost.Shows.Should().BeNull();
            collectionPost.Movies.Should().BeNull();

            builder.AddEpisode(episode1);

            collectionPost = builder.Build();

            collectionPost.Should().NotBeNull();
            collectionPost.Episodes.Should().NotBeNull().And.HaveCount(1);
            collectionPost.Shows.Should().BeNull();
            collectionPost.Movies.Should().BeNull();

            episode1.Ids.Trakt = 2;

            builder.AddEpisode(episode1);

            collectionPost = builder.Build();

            collectionPost.Should().NotBeNull();
            collectionPost.Episodes.Should().NotBeNull().And.HaveCount(1);
            collectionPost.Shows.Should().BeNull();
            collectionPost.Movies.Should().BeNull();

            var episodes = collectionPost.Episodes.ToArray();

            episodes[0].Should().NotBeNull();
            episodes[0].Ids.Should().NotBeNull();
            episodes[0].Ids.Trakt.Should().Be(2U);
            episodes[0].Ids.Imdb.Should().Be("imdb1");
            episodes[0].Ids.Tmdb.Should().Be(1234U);
            episodes[0].Ids.Tvdb.Should().Be(12345U);
            episodes[0].Ids.TvRage.Should().Be(123456U);

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

            collectionPost = builder.Build();

            collectionPost.Should().NotBeNull();
            collectionPost.Episodes.Should().NotBeNull().And.HaveCount(2);
            collectionPost.Shows.Should().BeNull();
            collectionPost.Movies.Should().BeNull();

            episodes = collectionPost.Episodes.ToArray();

            episodes[0].Should().NotBeNull();
            episodes[0].Ids.Should().NotBeNull();
            episodes[0].Ids.Trakt.Should().Be(2U);
            episodes[0].Ids.Imdb.Should().Be("imdb1");
            episodes[0].Ids.Tmdb.Should().Be(1234U);
            episodes[0].Ids.Tvdb.Should().Be(12345U);
            episodes[0].Ids.TvRage.Should().Be(123456U);

            episodes[1].Should().NotBeNull();
            episodes[1].Ids.Should().NotBeNull();
            episodes[1].Ids.Trakt.Should().Be(3U);
            episodes[1].Ids.Imdb.Should().Be("imdb2");
            episodes[1].Ids.Tmdb.Should().Be(12345U);
            episodes[1].Ids.Tvdb.Should().Be(123456U);
            episodes[1].Ids.TvRage.Should().Be(1234567U);
        }

        [TestMethod]
        public void TestTraktSyncCollectionPostBuilderAddEpisodeArgumentExceptions()
        {
            var builder = TraktSyncWatchlistPost.Builder();

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
        public void TestTraktSyncCollectionPostBuilderAddEpisodeCollection()
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

            var builder = TraktSyncWatchlistPost.Builder();

            builder.AddEpisodes(episodes);

            var collectionPost = builder.Build();

            collectionPost.Should().NotBeNull();
            collectionPost.Episodes.Should().NotBeNull().And.HaveCount(2);
            collectionPost.Shows.Should().BeNull();
            collectionPost.Movies.Should().BeNull();

            builder.AddEpisodes(episodes);

            collectionPost = builder.Build();

            collectionPost.Should().NotBeNull();
            collectionPost.Episodes.Should().NotBeNull().And.HaveCount(2);
            collectionPost.Shows.Should().BeNull();
            collectionPost.Movies.Should().BeNull();

            episode1.Ids.Trakt = 2;

            episodes = new List<TraktEpisode>
            {
                episode1,
                episode2
            };

            builder.AddEpisodes(episodes);

            collectionPost = builder.Build();

            collectionPost.Should().NotBeNull();
            collectionPost.Episodes.Should().NotBeNull().And.HaveCount(2);
            collectionPost.Shows.Should().BeNull();
            collectionPost.Movies.Should().BeNull();

            var watchlistEpisodes = collectionPost.Episodes.ToArray();

            watchlistEpisodes[0].Should().NotBeNull();
            watchlistEpisodes[0].Ids.Should().NotBeNull();
            watchlistEpisodes[0].Ids.Trakt.Should().Be(2U);
            watchlistEpisodes[0].Ids.Imdb.Should().Be("imdb1");
            watchlistEpisodes[0].Ids.Tmdb.Should().Be(1234U);
            watchlistEpisodes[0].Ids.Tvdb.Should().Be(12345U);
            watchlistEpisodes[0].Ids.TvRage.Should().Be(123456U);

            watchlistEpisodes[1].Should().NotBeNull();
            watchlistEpisodes[1].Ids.Should().NotBeNull();
            watchlistEpisodes[1].Ids.Trakt.Should().Be(3U);
            watchlistEpisodes[1].Ids.Imdb.Should().Be("imdb2");
            watchlistEpisodes[1].Ids.Tmdb.Should().Be(12345U);
            watchlistEpisodes[1].Ids.Tvdb.Should().Be(123456U);
            watchlistEpisodes[1].Ids.TvRage.Should().Be(1234567U);
        }

        [TestMethod]
        public void TestTraktSyncCollectionPostBuilderAddEpisodeCollectionArgumentExceptions()
        {
            var builder = TraktSyncWatchlistPost.Builder();

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
        public void TestTraktSyncWatchlistPostBuilderAddShow()
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

            var builder = TraktSyncWatchlistPost.Builder();

            builder.AddShow(show1);

            var watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            builder.AddShow(show1);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            show1.Ids.Trakt = 2;

            builder.AddShow(show1);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            var shows = watchlistPost.Shows.ToArray();

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

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(2);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
        }

        [TestMethod]
        public void TestTraktSyncWatchlistPostBuilderAddShowArgumentExceptions()
        {
            var builder = TraktSyncWatchlistPost.Builder();

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
        public void TestTraktSyncWatchlistPostBuilderAddShowCollection()
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

            var builder = TraktSyncWatchlistPost.Builder();

            builder.AddShows(shows);

            var watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(2);
            watchlistPost.Movies.Should().BeNull();

            builder.AddShows(shows);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(2);
            watchlistPost.Movies.Should().BeNull();

            show1.Ids.Trakt = 2;

            shows = new List<TraktShow>
            {
                show1,
                show2
            };

            builder.AddShows(shows);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(2);
            watchlistPost.Movies.Should().BeNull();

            var watchlistShows = watchlistPost.Shows.ToArray();

            watchlistShows[0].Should().NotBeNull();
            watchlistShows[0].Title.Should().Be("show1");
            watchlistShows[0].Year.Should().Be(2016);
            watchlistShows[0].Ids.Should().NotBeNull();
            watchlistShows[0].Ids.Trakt.Should().Be(2U);
            watchlistShows[0].Ids.Slug.Should().Be("show1");
            watchlistShows[0].Ids.Imdb.Should().Be("imdb1");
            watchlistShows[0].Ids.Tmdb.Should().Be(1234U);
            watchlistShows[0].Ids.Tvdb.Should().Be(12345U);
            watchlistShows[0].Ids.TvRage.Should().Be(123456U);

            watchlistShows[1].Should().NotBeNull();
            watchlistShows[1].Title.Should().Be("show2");
            watchlistShows[1].Year.Should().Be(2016);
            watchlistShows[1].Ids.Should().NotBeNull();
            watchlistShows[1].Ids.Trakt.Should().Be(3U);
            watchlistShows[1].Ids.Slug.Should().Be("show2");
            watchlistShows[1].Ids.Imdb.Should().Be("imdb2");
            watchlistShows[1].Ids.Tmdb.Should().Be(12345U);
            watchlistShows[1].Ids.Tvdb.Should().Be(123456U);
            watchlistShows[1].Ids.TvRage.Should().Be(1234567U);
        }

        [TestMethod]
        public void TestTraktSyncWatchlistPostBuilderAddShowCollectionArgumentExceptions()
        {
            var builder = TraktSyncWatchlistPost.Builder();

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
        public void TestTraktSyncWatchlistPostBuilderAddShowWithSeasons()
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

            var builder = TraktSyncWatchlistPost.Builder();

            builder.AddShow(show1, 1);

            var watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            var shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(1);

            var show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();

            // ---------------------------------------------------------

            builder.AddShow(show1, 1, 2);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();

            // ---------------------------------------------------------

            builder.AddShow(show1, 1, 2, 3);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(3);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();

            show1Seasons[2].Number.Should().Be(3);
            show1Seasons[2].Episodes.Should().BeNull();

            // ---------------------------------------------------------

            show1.Ids.Trakt = 2;

            builder.AddShow(show1, 1, 2, 3);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(3);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();

            show1Seasons[2].Number.Should().Be(3);
            show1Seasons[2].Episodes.Should().BeNull();

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

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(2);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[1].Seasons.Should().NotBeNull().And.HaveCount(3);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();

            show1Seasons[2].Number.Should().Be(3);
            show1Seasons[2].Episodes.Should().BeNull();

            var show2Seasons = shows[1].Seasons.ToArray();

            show2Seasons[0].Number.Should().Be(1);
            show2Seasons[0].Episodes.Should().BeNull();

            show2Seasons[1].Number.Should().Be(2);
            show2Seasons[1].Episodes.Should().BeNull();

            show2Seasons[2].Number.Should().Be(3);
            show2Seasons[2].Episodes.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktSyncWatchlistPostBuilderAddShowWithSeasonsArgumentExceptions()
        {
            var builder = TraktSyncWatchlistPost.Builder();

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
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncWatchlistPostBuilderAddShowWithSeasonsArray()
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

            var builder = TraktSyncWatchlistPost.Builder();

            var seasons = new int[] { 1 };

            builder.AddShow(show1, seasons);

            var watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            var shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(1);

            var show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();

            // ---------------------------------------------------------

            seasons = new int[] { 1, 2 };

            builder.AddShow(show1, seasons);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();

            // ---------------------------------------------------------

            seasons = new int[] { 1, 2, 3 };

            builder.AddShow(show1, seasons);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(3);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();

            show1Seasons[2].Number.Should().Be(3);
            show1Seasons[2].Episodes.Should().BeNull();

            // ---------------------------------------------------------

            show1.Ids.Trakt = 2;

            seasons = new int[] { 1, 2, 3 };

            builder.AddShow(show1, seasons);

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(3);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();

            show1Seasons[2].Number.Should().Be(3);
            show1Seasons[2].Episodes.Should().BeNull();

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

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(2);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[1].Seasons.Should().NotBeNull().And.HaveCount(3);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();

            show1Seasons[2].Number.Should().Be(3);
            show1Seasons[2].Episodes.Should().BeNull();

            var show2Seasons = shows[1].Seasons.ToArray();

            show2Seasons[0].Number.Should().Be(1);
            show2Seasons[0].Episodes.Should().BeNull();

            show2Seasons[1].Number.Should().Be(2);
            show2Seasons[1].Episodes.Should().BeNull();

            show2Seasons[2].Number.Should().Be(3);
            show2Seasons[2].Episodes.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktSyncWatchlistPostBuilderAddShowWithSeasonsArrayArgumentExceptions()
        {
            var builder = TraktSyncWatchlistPost.Builder();

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

            seasons = new int[] { -1 };

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, seasons);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            seasons = new int[] { 1, 2, -1 };

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, seasons);
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, default(int[]));
            act.ShouldThrow<ArgumentNullException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncWatchlistPostBuilderAddShowWithSeasonsAndEpisodes()
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

            var builder = TraktSyncWatchlistPost.Builder();

            builder.AddShow(show1, new PostSeasons { { 1, new PostEpisodes { 1 } } }); // season 1 - episode 1

            var watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            var shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(1);

            var show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(1);

            var show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);

            // ---------------------------------------------------------

            builder.AddShow(show1, new PostSeasons { { 1, new PostEpisodes { 1, 2 } } }); // season 1 - episode 1, 2

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(1);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(2);

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[1].Number.Should().Be(2);

            // ---------------------------------------------------------

            builder.AddShow(show1, new PostSeasons { { 1, new PostEpisodes { 1, 2, 3 } } }); // season 1 - episode 1, 2, 3

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(1);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[2].Number.Should().Be(3);

            // ---------------------------------------------------------

            builder.AddShow(show1, new PostSeasons {
                { 1, new PostEpisodes { 1, 2, 3 } }, // season 1 - episode 1, 2, 3
                { 2, new PostEpisodes { 4 } }        // season 2 - episode 4
            });

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().NotBeNull().And.HaveCount(1);

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[2].Number.Should().Be(3);

            var show1Season2Episodes = show1Seasons[1].Episodes.ToArray();

            show1Season2Episodes[0].Number.Should().Be(4);

            // ---------------------------------------------------------

            builder.AddShow(show1, new PostSeasons {
                { 1, new PostEpisodes { 1, 2, 3 } }, // season 1 - episode 1, 2, 3
                { 2, new PostEpisodes { 4, 5 } }     // season 2 - episode 4, 5
            });

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().NotBeNull().And.HaveCount(2);

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[2].Number.Should().Be(3);

            show1Season2Episodes = show1Seasons[1].Episodes.ToArray();

            show1Season2Episodes[0].Number.Should().Be(4);
            show1Season2Episodes[1].Number.Should().Be(5);

            // ---------------------------------------------------------

            builder.AddShow(show1, new PostSeasons {
                { 1, new PostEpisodes { 1, 2, 3 } }, // season 1 - episode 1, 2, 3
                { 2, new PostEpisodes { 4, 5, 6 } }  // season 2 - episode 4, 5, 6
            });

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().NotBeNull().And.HaveCount(3);

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[2].Number.Should().Be(3);

            show1Season2Episodes = show1Seasons[1].Episodes.ToArray();

            show1Season2Episodes[0].Number.Should().Be(4);
            show1Season2Episodes[1].Number.Should().Be(5);
            show1Season2Episodes[2].Number.Should().Be(6);

            // ---------------------------------------------------------

            show1.Ids.Trakt = 2;

            builder.AddShow(show1, new PostSeasons {
                { 1, new PostEpisodes { 1, 2, 3 } }, // season 1 - episode 1, 2, 3
                { 2, new PostEpisodes { 4, 5, 6 } }  // season 2 - episode 4, 5, 6
            });

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().NotBeNull().And.HaveCount(3);

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[2].Number.Should().Be(3);

            show1Season2Episodes = show1Seasons[1].Episodes.ToArray();

            show1Season2Episodes[0].Number.Should().Be(4);
            show1Season2Episodes[1].Number.Should().Be(5);
            show1Season2Episodes[2].Number.Should().Be(6);

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

            builder.AddShow(show2, new PostSeasons {
                { 1, new PostEpisodes { 1, 2, 3 } }, // season 1 - episode 1, 2, 3
                { 2, new PostEpisodes { 4, 5, 6 } }  // season 2 - episode 4, 5, 6
            });

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(2);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[1].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().NotBeNull().And.HaveCount(3);

            show1Season1Episodes = show1Seasons[0].Episodes.ToArray();

            show1Season1Episodes[0].Number.Should().Be(1);
            show1Season1Episodes[1].Number.Should().Be(2);
            show1Season1Episodes[2].Number.Should().Be(3);

            show1Season2Episodes = show1Seasons[1].Episodes.ToArray();

            show1Season2Episodes[0].Number.Should().Be(4);
            show1Season2Episodes[1].Number.Should().Be(5);
            show1Season2Episodes[2].Number.Should().Be(6);

            var show2Seasons = shows[1].Seasons.ToArray();

            show2Seasons[0].Number.Should().Be(1);
            show2Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(3);

            show2Seasons[1].Number.Should().Be(2);
            show2Seasons[1].Episodes.Should().NotBeNull().And.HaveCount(3);

            var show2Season1Episodes = show2Seasons[0].Episodes.ToArray();

            show2Season1Episodes[0].Number.Should().Be(1);
            show2Season1Episodes[1].Number.Should().Be(2);
            show2Season1Episodes[2].Number.Should().Be(3);

            var show2Season2Episodes = show2Seasons[1].Episodes.ToArray();

            show2Season2Episodes[0].Number.Should().Be(4);
            show2Season2Episodes[1].Number.Should().Be(5);
            show2Season2Episodes[2].Number.Should().Be(6);

            // ---------------------------------------------------------

            builder = TraktSyncWatchlistPost.Builder();

            builder.AddShow(show2, new PostSeasons { 1 });  // season 1

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(1);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().BeNull();

            // ---------------------------------------------------------

            builder = TraktSyncWatchlistPost.Builder();

            builder.AddShow(show2, new PostSeasons {
                { 1, new PostEpisodes { 1, 2 } }, // season 1 - episode 1, 2
                2                                 // season 2
            });

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Episodes.Should().BeNull();
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Movies.Should().BeNull();

            shows = watchlistPost.Shows.ToArray();

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
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            show1Seasons = shows[0].Seasons.ToArray();

            show1Seasons[0].Number.Should().Be(1);
            show1Seasons[0].Episodes.Should().NotBeNull().And.HaveCount(2);

            show1Seasons[1].Number.Should().Be(2);
            show1Seasons[1].Episodes.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktSyncWatchlistPostBuilderAddShowWithSeasonsAndEpisodesArgumentExceptions()
        {
            var builder = TraktSyncWatchlistPost.Builder();

            Action act = () => builder.AddShow(null, new PostSeasons { { 1, new PostEpisodes { 1, 2, 3 } } });
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddShow(new TraktShow(), new PostSeasons { { 1, new PostEpisodes { 1, 2, 3 } } });
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds() }, new PostSeasons { { 1, new PostEpisodes { 1, 2, 3 } } });
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 }, Year = 123 }, new PostSeasons { { 1, new PostEpisodes { 1, 2, 3 } } });
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 }, Year = 12345 }, new PostSeasons { { 1, new PostEpisodes { 1, 2, 3 } } });
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, default(PostSeasons));
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostSeasons { { -1, new PostEpisodes { 1, 2, 3 } } });
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostSeasons { { 1, new PostEpisodes { 1, -1, 3 } } });
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostSeasons {
                { 1, new PostEpisodes { 1, 2, 3 } },
                { -1, new PostEpisodes { 1, 2, 3 }}
            });
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostSeasons {
                { 1, new PostEpisodes { 1, 2, 3 } },
                { 1, new PostEpisodes { 1, -1, 3 }}
            });
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncWatchlistPostBuilderReset()
        {
            var movie1 = new TraktMovie
            {
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

            var builder = TraktSyncWatchlistPost.Builder();

            var watchlistPost = builder.AddMovie(movie1)
                                    .AddEpisode(episode1)
                                    .AddShow(show1)
                                    .Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Movies.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Episodes.Should().NotBeNull().And.HaveCount(1);

            builder.Reset();

            watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Movies.Should().BeNull();
            watchlistPost.Shows.Should().BeNull();
            watchlistPost.Episodes.Should().BeNull();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktSyncWatchlistPostBuilderAddAll()
        {
            var movie1 = new TraktMovie
            {
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
                    Tmdb = 123454,
                    Tvdb = 1234565,
                    TvRage = 12345676
                }
            };

            var builder = TraktSyncWatchlistPost.Builder();

            var watchlistPost = builder.Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Movies.Should().BeNull();
            watchlistPost.Shows.Should().BeNull();
            watchlistPost.Episodes.Should().BeNull();

            // ------------------------------------------------------

            watchlistPost = builder.AddMovie(movie1)
                                .AddEpisode(episode1)
                                .AddShow(show1)
                                .AddShow(show2, 1, 2, 3)
                                .AddShow(show3, new PostSeasons {
                                    { 1, new PostEpisodes { 1, 2 } },
                                    { 2, new PostEpisodes { 1, 2 } },
                                    3, 4, 5
                                })
                                .Build();

            watchlistPost.Should().NotBeNull();
            watchlistPost.Movies.Should().NotBeNull().And.HaveCount(1);
            watchlistPost.Shows.Should().NotBeNull().And.HaveCount(3);
            watchlistPost.Episodes.Should().NotBeNull().And.HaveCount(1);
        }
    }
}
