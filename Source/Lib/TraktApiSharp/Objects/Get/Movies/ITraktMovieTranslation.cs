﻿namespace TraktApiSharp.Objects.Get.Movies
{
    using Basic;

    public interface ITraktMovieTranslation : ITraktTranslation
    {
        string Tagline { get; set; }
    }
}
