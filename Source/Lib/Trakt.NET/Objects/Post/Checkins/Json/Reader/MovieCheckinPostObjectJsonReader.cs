﻿namespace TraktNet.Objects.Post.Checkins.Json.Reader
{
    using Basic.Json.Reader;
    using Get.Movies.Json.Reader;
    using Newtonsoft.Json;
    using Objects.Json;
    using System.Threading;
    using System.Threading.Tasks;

    internal class MovieCheckinPostObjectJsonReader : AObjectJsonReader<ITraktMovieCheckinPost>
    {
        public override async Task<ITraktMovieCheckinPost> ReadObjectAsync(JsonTextReader jsonReader, CancellationToken cancellationToken = default)
        {
            if (jsonReader == null)
                return await Task.FromResult(default(ITraktMovieCheckinPost));

            if (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.StartObject)
            {
                var sharingReader = new SharingObjectJsonReader();
                var movieReader = new MovieObjectJsonReader();
                ITraktMovieCheckinPost movieCheckinPost = new TraktMovieCheckinPost();

                while (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = jsonReader.Value.ToString();

                    switch (propertyName)
                    {
                        case JsonProperties.CHECKIN_POST_PROPERTY_NAME_SHARING:
                            movieCheckinPost.Sharing = await sharingReader.ReadObjectAsync(jsonReader, cancellationToken);
                            break;
                        case JsonProperties.CHECKIN_POST_PROPERTY_NAME_MESSAGE:
                            movieCheckinPost.Message = await jsonReader.ReadAsStringAsync(cancellationToken);
                            break;
                        case JsonProperties.CHECKIN_POST_PROPERTY_NAME_APP_VERSION:
                            movieCheckinPost.AppVersion = await jsonReader.ReadAsStringAsync(cancellationToken);
                            break;
                        case JsonProperties.CHECKIN_POST_PROPERTY_NAME_APP_DATE:
                            movieCheckinPost.AppDate = await jsonReader.ReadAsStringAsync(cancellationToken);
                            break;
                        case JsonProperties.CHECKIN_POST_PROPERTY_NAME_VENUE_ID:
                            movieCheckinPost.FoursquareVenueId = await jsonReader.ReadAsStringAsync(cancellationToken);
                            break;
                        case JsonProperties.CHECKIN_POST_PROPERTY_NAME_VENUE_NAME:
                            movieCheckinPost.FoursquareVenueName = await jsonReader.ReadAsStringAsync(cancellationToken);
                            break;
                        case JsonProperties.MOVIE_CHECKIN_POST_PROPERTY_NAME_MOVIE:
                            movieCheckinPost.Movie = await movieReader.ReadObjectAsync(jsonReader, cancellationToken);
                            break;
                        default:
                            await JsonReaderHelper.ReadAndIgnoreInvalidContentAsync(jsonReader, cancellationToken);
                            break;
                    }
                }

                return movieCheckinPost;
            }

            return await Task.FromResult(default(ITraktMovieCheckinPost));
        }
    }
}
