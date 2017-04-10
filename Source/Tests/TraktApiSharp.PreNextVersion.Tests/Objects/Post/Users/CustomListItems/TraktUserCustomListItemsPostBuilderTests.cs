﻿namespace TraktApiSharp.Tests.Objects.Post.Users.CustomListItems
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TraktApiSharp.Objects.Get.Movies;
    using TraktApiSharp.Objects.Get.Movies.Implementations;
    using TraktApiSharp.Objects.Get.People;
    using TraktApiSharp.Objects.Get.People.Implementations;
    using TraktApiSharp.Objects.Get.Shows;
    using TraktApiSharp.Objects.Get.Shows.Implementations;
    using TraktApiSharp.Objects.Post;
    using TraktApiSharp.Objects.Post.Users.CustomListItems;

    [TestClass]
    public class TraktUserCustomListItemsPostBuilderTests
    {
        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddMovie()
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

            var builder = TraktUserCustomListItemsPost.Builder();

            builder.AddMovie(movie1);

            var listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().NotBeNull().And.HaveCount(1);

            builder.AddMovie(movie1);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().NotBeNull().And.HaveCount(1);

            movie1.Ids.Trakt = 2;

            builder.AddMovie(movie1);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().NotBeNull().And.HaveCount(1);

            var movies = listItemsPost.Movies.ToArray();

            movies[0].Should().NotBeNull();
            movies[0].Ids.Should().NotBeNull();
            movies[0].Ids.Trakt.Should().Be(2U);
            movies[0].Ids.Slug.Should().Be("movie1");
            movies[0].Ids.Imdb.Should().Be("imdb1");
            movies[0].Ids.Tmdb.Should().Be(1234U);

            var movie2 = new TraktMovie
            {
                Ids = (ITraktMovieIds)new TraktMovieIds
                {
                    Trakt = 3,
                    Slug = "movie2",
                    Imdb = "imdb2",
                    Tmdb = 12345
                }
            };

            builder.AddMovie(movie2);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().NotBeNull().And.HaveCount(2);

            movies = listItemsPost.Movies.ToArray();

            movies[0].Should().NotBeNull();
            movies[0].Ids.Should().NotBeNull();
            movies[0].Ids.Trakt.Should().Be(2U);
            movies[0].Ids.Slug.Should().Be("movie1");
            movies[0].Ids.Imdb.Should().Be("imdb1");
            movies[0].Ids.Tmdb.Should().Be(1234U);

            movies[1].Should().NotBeNull();
            movies[1].Ids.Should().NotBeNull();
            movies[1].Ids.Trakt.Should().Be(3U);
            movies[1].Ids.Slug.Should().Be("movie2");
            movies[1].Ids.Imdb.Should().Be("imdb2");
            movies[1].Ids.Tmdb.Should().Be(12345U);
        }

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddMovieArgumentExceptions()
        {
            var builder = TraktUserCustomListItemsPost.Builder();

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

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddMovieCollection()
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

            var movie2 = new TraktMovie
            {
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

            var builder = TraktUserCustomListItemsPost.Builder();

            builder.AddMovies(movies);

            var listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().NotBeNull().And.HaveCount(2);

            builder.AddMovies(movies);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().NotBeNull().And.HaveCount(2);

            movie1.Ids.Trakt = 2;

            movies = new List<TraktMovie>
            {
                movie1,
                movie2
            };

            builder.AddMovies(movies);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().NotBeNull().And.HaveCount(2);

            var customListMovies = listItemsPost.Movies.ToArray();

            customListMovies[0].Should().NotBeNull();
            customListMovies[0].Ids.Should().NotBeNull();
            customListMovies[0].Ids.Trakt.Should().Be(2U);
            customListMovies[0].Ids.Slug.Should().Be("movie1");
            customListMovies[0].Ids.Imdb.Should().Be("imdb1");
            customListMovies[0].Ids.Tmdb.Should().Be(1234U);

            customListMovies[1].Should().NotBeNull();
            customListMovies[1].Ids.Should().NotBeNull();
            customListMovies[1].Ids.Trakt.Should().Be(3U);
            customListMovies[1].Ids.Slug.Should().Be("movie2");
            customListMovies[1].Ids.Imdb.Should().Be("imdb2");
            customListMovies[1].Ids.Tmdb.Should().Be(12345U);
        }

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddMovieCollectionArgumentExceptions()
        {
            var builder = TraktUserCustomListItemsPost.Builder();

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

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddPerson()
        {
            var person1 = new TraktPerson
            {
                Name = "Person 1 Name",
                Ids = new TraktPersonIds
                {
                    Trakt = 1,
                    Slug = "person1",
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    TvRage = 123456
                }
            };

            var builder = TraktUserCustomListItemsPost.Builder();

            builder.AddPerson(person1);

            var listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().BeNull();

            builder.AddPerson(person1);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().BeNull();

            person1.Name = "New Person Name";

            builder.AddPerson(person1);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().BeNull();

            var persons = listItemsPost.People.ToArray();

            persons[0].Should().NotBeNull();
            persons[0].Name.Should().Be("New Person Name");
            persons[0].Ids.Should().NotBeNull();
            persons[0].Ids.Trakt.Should().Be(1U);
            persons[0].Ids.Slug.Should().Be("person1");
            persons[0].Ids.Imdb.Should().Be("imdb1");
            persons[0].Ids.Tmdb.Should().Be(1234U);
            persons[0].Ids.TvRage.Should().Be(123456U);

            var person2 = new TraktPerson
            {
                Name = "Person 2 Name",
                Ids = new TraktPersonIds
                {
                    Trakt = 2,
                    Slug = "person2",
                    Imdb = "imdb2",
                    Tmdb = 12345,
                    TvRage = 1234567
                }
            };

            builder.AddPerson(person2);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().NotBeNull().And.HaveCount(2);
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().BeNull();

            persons = listItemsPost.People.ToArray();

            persons[0].Should().NotBeNull();
            persons[0].Name.Should().Be("New Person Name");
            persons[0].Ids.Should().NotBeNull();
            persons[0].Ids.Trakt.Should().Be(1U);
            persons[0].Ids.Slug.Should().Be("person1");
            persons[0].Ids.Imdb.Should().Be("imdb1");
            persons[0].Ids.Tmdb.Should().Be(1234U);
            persons[0].Ids.TvRage.Should().Be(123456U);

            persons[1].Should().NotBeNull();
            persons[1].Name.Should().Be("Person 2 Name");
            persons[1].Ids.Should().NotBeNull();
            persons[1].Ids.Trakt.Should().Be(2U);
            persons[1].Ids.Slug.Should().Be("person2");
            persons[1].Ids.Imdb.Should().Be("imdb2");
            persons[1].Ids.Tmdb.Should().Be(12345U);
            persons[1].Ids.TvRage.Should().Be(1234567U);
        }

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddPersonArgumentExceptions()
        {
            var builder = TraktUserCustomListItemsPost.Builder();

            Action act = () => builder.AddPerson(null);
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddPerson(new TraktPerson());
            act.ShouldThrow<ArgumentNullException>();

            act = () => builder.AddPerson(new TraktPerson { Ids = new TraktPersonIds(), Name = "Person Name" });
            act.ShouldThrow<ArgumentException>();

            var ids = new TraktPersonIds { Trakt = 1, Slug = "person" };
            act = () => builder.AddPerson(new TraktPerson { Ids = ids, Name = null });
            act.ShouldThrow<ArgumentException>();

            act = () => builder.AddPerson(new TraktPerson { Ids = ids, Name = string.Empty });
            act.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddPersonCollection()
        {
            var person1 = new TraktPerson
            {
                Name = "Person 1 Name",
                Ids = new TraktPersonIds
                {
                    Trakt = 1,
                    Slug = "person1",
                    Imdb = "imdb1",
                    Tmdb = 1234,
                    TvRage = 123456
                }
            };

            var person2 = new TraktPerson
            {
                Name = "Person 2 Name",
                Ids = new TraktPersonIds
                {
                    Trakt = 2,
                    Slug = "person2",
                    Imdb = "imdb2",
                    Tmdb = 12345,
                    TvRage = 1234567
                }
            };

            var persons = new List<TraktPerson>
            {
                person1,
                person2
            };

            var builder = TraktUserCustomListItemsPost.Builder();

            builder.AddPersons(persons);

            var listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().NotBeNull().And.HaveCount(2);
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().BeNull();

            builder.AddPersons(persons);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().NotBeNull().And.HaveCount(2);
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().BeNull();

            person1.Name = "New Person Name";

            persons = new List<TraktPerson>
            {
                person1,
                person2
            };

            builder.AddPersons(persons);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().NotBeNull().And.HaveCount(2);
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.Movies.Should().BeNull();

            var customListPersons = listItemsPost.People.ToArray();

            customListPersons[0].Should().NotBeNull();
            customListPersons[0].Name.Should().Be("New Person Name");
            customListPersons[0].Ids.Should().NotBeNull();
            customListPersons[0].Ids.Trakt.Should().Be(1U);
            customListPersons[0].Ids.Slug.Should().Be("person1");
            customListPersons[0].Ids.Imdb.Should().Be("imdb1");
            customListPersons[0].Ids.Tmdb.Should().Be(1234U);
            customListPersons[0].Ids.TvRage.Should().Be(123456U);

            customListPersons[1].Should().NotBeNull();
            customListPersons[1].Name.Should().Be("Person 2 Name");
            customListPersons[1].Ids.Should().NotBeNull();
            customListPersons[1].Ids.Trakt.Should().Be(2U);
            customListPersons[1].Ids.Slug.Should().Be("person2");
            customListPersons[1].Ids.Imdb.Should().Be("imdb2");
            customListPersons[1].Ids.Tmdb.Should().Be(12345U);
            customListPersons[1].Ids.TvRage.Should().Be(1234567U);
        }

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddPersonCollectionArgumentExceptions()
        {
            var builder = TraktUserCustomListItemsPost.Builder();

            Action act = () => builder.AddPersons(null);
            act.ShouldThrow<ArgumentNullException>();

            var persons = new List<TraktPerson>
            {
                new TraktPerson()
            };

            act = () => builder.AddPersons(persons);
            act.ShouldThrow<ArgumentNullException>();

            persons = new List<TraktPerson>
            {
                new TraktPerson { Ids = new TraktPersonIds(), Name = "Person Name" }
            };

            act = () => builder.AddPersons(persons);
            act.ShouldThrow<ArgumentException>();

            var ids = new TraktPersonIds { Trakt = 1, Slug = "person" };

            persons = new List<TraktPerson>
            {
                new TraktPerson { Ids = ids, Name = null }
            };

            act = () => builder.AddPersons(persons);
            act.ShouldThrow<ArgumentException>();

            persons = new List<TraktPerson>
            {
                new TraktPerson { Ids = ids, Name = string.Empty }
            };

            act = () => builder.AddPersons(persons);
            act.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddShow()
        {
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

            var builder = TraktUserCustomListItemsPost.Builder();

            builder.AddShow(show1);

            var listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            builder.AddShow(show1);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            show1.Ids.Trakt = 2;

            builder.AddShow(show1);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            var shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);

            var show2 = new TraktShow
            {
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

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(2);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);

            shows[1].Should().NotBeNull();
            shows[1].Ids.Should().NotBeNull();
            shows[1].Ids.Trakt.Should().Be(3U);
            shows[1].Ids.Slug.Should().Be("show2");
            shows[1].Ids.Imdb.Should().Be("imdb2");
            shows[1].Ids.Tmdb.Should().Be(12345U);
            shows[1].Ids.Tvdb.Should().Be(123456U);
            shows[1].Ids.TvRage.Should().Be(1234567U);
        }

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddShowArgumentExceptions()
        {
            var builder = TraktUserCustomListItemsPost.Builder();

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

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddShowCollection()
        {
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

            var builder = TraktUserCustomListItemsPost.Builder();

            builder.AddShows(shows);

            var listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(2);
            listItemsPost.Movies.Should().BeNull();

            builder.AddShows(shows);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(2);
            listItemsPost.Movies.Should().BeNull();

            show1.Ids.Trakt = 2;

            shows = new List<TraktShow>
            {
                show1,
                show2
            };

            builder.AddShows(shows);

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(2);
            listItemsPost.Movies.Should().BeNull();

            var customListShows = listItemsPost.Shows.ToArray();

            customListShows[0].Should().NotBeNull();
            customListShows[0].Ids.Should().NotBeNull();
            customListShows[0].Ids.Trakt.Should().Be(2U);
            customListShows[0].Ids.Slug.Should().Be("show1");
            customListShows[0].Ids.Imdb.Should().Be("imdb1");
            customListShows[0].Ids.Tmdb.Should().Be(1234U);
            customListShows[0].Ids.Tvdb.Should().Be(12345U);
            customListShows[0].Ids.TvRage.Should().Be(123456U);

            customListShows[1].Should().NotBeNull();
            customListShows[1].Ids.Should().NotBeNull();
            customListShows[1].Ids.Trakt.Should().Be(3U);
            customListShows[1].Ids.Slug.Should().Be("show2");
            customListShows[1].Ids.Imdb.Should().Be("imdb2");
            customListShows[1].Ids.Tmdb.Should().Be(12345U);
            customListShows[1].Ids.Tvdb.Should().Be(123456U);
            customListShows[1].Ids.TvRage.Should().Be(1234567U);
        }

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddShowCollectionArgumentExceptions()
        {
            var builder = TraktUserCustomListItemsPost.Builder();

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

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddShowWithSeasons()
        {
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

            var builder = TraktUserCustomListItemsPost.Builder();

            builder.AddShow(show1, 1);

            var listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            var shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(2);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(3);

            shows[1].Should().NotBeNull();
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
        public void TestTraktUserCustomListItemsPostBuilderAddShowWithSeasonsArgumentExceptions()
        {
            var builder = TraktUserCustomListItemsPost.Builder();

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

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddShowWithSeasonsArray()
        {
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

            var builder = TraktUserCustomListItemsPost.Builder();

            var seasons = new int[] { 1 };

            builder.AddShow(show1, seasons);

            var listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            var shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(2);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(3);

            shows[1].Should().NotBeNull();
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
        public void TestTraktUserCustomListItemsPostBuilderAddShowWithSeasonsArrayArgumentExceptions()
        {
            var builder = TraktUserCustomListItemsPost.Builder();

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

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddShowWithSeasonsAndEpisodes()
        {
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

            var builder = TraktUserCustomListItemsPost.Builder();

            builder.AddShow(show1, new PostSeasons { { 1, new PostEpisodes { 1 } } }); // season 1 - episode 1

            var listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            var shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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
                { 1, new PostEpisodes { 1, 2, 3 } },    // season 1 - episode 1, 2, 3
                { 2, new PostEpisodes { 4 } }           // season 2 - episode 4
            });

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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
                { 1, new PostEpisodes { 1, 2, 3 } },    // season 1 - episode 1, 2, 3
                { 2, new PostEpisodes { 4, 5 } }        // season 2 - episode 4, 5
            });

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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
                { 1, new PostEpisodes { 1, 2, 3 } },    // season 1 - episode 1, 2, 3
                { 2, new PostEpisodes { 4, 5, 6 } }     // season 2 - episode 4, 5, 6
            });

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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
                { 1, new PostEpisodes { 1, 2, 3 } },    // season 1 - episode 1, 2, 3
                { 2, new PostEpisodes { 4, 5, 6 } }     // season 2 - episode 4, 5, 6
            });

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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
                { 1, new PostEpisodes { 1, 2, 3 } },    // season 1 - episode 1, 2, 3
                { 2, new PostEpisodes { 4, 5, 6 } }     // season 2 - episode 4, 5, 6
            });

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(2);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
            shows[0].Ids.Should().NotBeNull();
            shows[0].Ids.Trakt.Should().Be(2U);
            shows[0].Ids.Slug.Should().Be("show1");
            shows[0].Ids.Imdb.Should().Be("imdb1");
            shows[0].Ids.Tmdb.Should().Be(1234U);
            shows[0].Ids.Tvdb.Should().Be(12345U);
            shows[0].Ids.TvRage.Should().Be(123456U);
            shows[0].Seasons.Should().NotBeNull().And.HaveCount(2);

            shows[1].Should().NotBeNull();
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

            builder = TraktUserCustomListItemsPost.Builder();

            builder.AddShow(show2, new PostSeasons { 1 });  // season 1

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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

            builder = TraktUserCustomListItemsPost.Builder();

            builder.AddShow(show2, new PostSeasons {
                { 1, new PostEpisodes { 1, 2 } },   // season 1 - episode 1, 2
                2                                   // season 2
            });

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.People.Should().BeNull();
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Movies.Should().BeNull();

            shows = listItemsPost.Shows.ToArray();

            shows[0].Should().NotBeNull();
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
        public void TestTraktUserCustomListItemsPostBuilderAddShowWithSeasonsAndEpisodesArgumentExceptions()
        {
            var builder = TraktUserCustomListItemsPost.Builder();

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

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostSeasons { { 1, new PostEpisodes { 1, 2, 3 } },
                                                                                                                  { -1, new PostEpisodes { 1, 2, 3 } } });
            act.ShouldThrow<ArgumentOutOfRangeException>();

            act = () => builder.AddShow(new TraktShow { Ids = (ITraktShowIds)new TraktShowIds { Trakt = 1 } }, new PostSeasons { { 1, new PostEpisodes { 1, 2, 3 } },
                                                                                                                  { 2, new PostEpisodes { 1, -1, 3 } } });
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderReset()
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

            var person1 = new TraktPerson
            {
                Name = "Person 1 Name",
                Ids = new TraktPersonIds
                {
                    Trakt = 1,
                    Slug = "person1",
                    Imdb = "imdb1",
                    Tmdb = 1234,
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

            var builder = TraktUserCustomListItemsPost.Builder();

            var listItemsPost = builder.AddMovie(movie1)
                                    .AddPerson(person1)
                                    .AddShow(show1)
                                    .Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.Movies.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.People.Should().NotBeNull().And.HaveCount(1);

            builder.Reset();

            listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.Movies.Should().BeNull();
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.People.Should().BeNull();
        }

        // ----------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------

        [TestMethod]
        public void TestTraktUserCustomListItemsPostBuilderAddAll()
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

            var person1 = new TraktPerson
            {
                Name = "Person 1 Name",
                Ids = new TraktPersonIds
                {
                    Trakt = 1,
                    Slug = "person1",
                    Imdb = "imdb1",
                    Tmdb = 1234,
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

            var builder = TraktUserCustomListItemsPost.Builder();

            var listItemsPost = builder.Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.Movies.Should().BeNull();
            listItemsPost.Shows.Should().BeNull();
            listItemsPost.People.Should().BeNull();

            // ------------------------------------------------------

            listItemsPost = builder.AddMovie(movie1)
                                .AddPerson(person1)
                                .AddShow(show1)
                                .AddShow(show2, 1, 2, 3)
                                .AddShow(show3, new PostSeasons { { 1, new PostEpisodes { 1, 2 } },
                                                                    { 2, new PostEpisodes { 1, 2 } } })
                                .Build();

            listItemsPost.Should().NotBeNull();
            listItemsPost.Movies.Should().NotBeNull().And.HaveCount(1);
            listItemsPost.Shows.Should().NotBeNull().And.HaveCount(3);
            listItemsPost.People.Should().NotBeNull().And.HaveCount(1);
        }
    }
}
